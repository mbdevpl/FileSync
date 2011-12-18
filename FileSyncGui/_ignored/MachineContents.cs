using System;
using System.Collections.Generic;
using System.Text;

using FileSyncGui.GuiAbstracts;
using FileSyncLib;

namespace FileSyncGui.GuiObjects {

	/// <summary>
	/// Stores the identity of a machine and a list of identities of directories,
	/// which it contains.
	/// </summary>
	public class MachineContents : MachineIdentity {

		private List<DirContents> directories;
		public List<DirContents> Directories {
			get { return directories; }
		}

		public MachineContents(string name, string description = null,
			List<DirContents> directories = null)
			: base(name, description) {
			this.directories = directories;
		}

		/// <summary>
		/// Creates new contents of the machine.
		/// </summary>
		/// <param name="mid">ientity of the machine</param>
		/// <param name="directories">list of directories of the machine</param>
		public MachineContents(MachineIdentity mid, List<DirContents> directories = null)
			: this(mid.Name, mid.Description, directories) {
			//nothing needed here
		}

		public MachineContents(MachineContents mc)
			: this(mc.Name, mc.Description, mc.Directories) {
			//nothing needed here
		}

		/// <summary>
		/// Accesses the database, and downloads the content. Amount of data downloaded 
		/// can be adjusted with use of optional boolean arguments.
		/// </summary>
		/// <param name="cr"></param>
		/// <param name="mid"></param>
		/// <param name="downloadDirsFilesMetadata"></param>
		/// <param name="downloadDirsFilesContents"></param>
		/// <param name="addRealDirsContents"></param>
		public MachineContents(Credentials cr, MachineIdentity mid,
				bool downloadDirsFilesMetadata = false, bool downloadDirsFilesContents = false,
				bool addRealDirsContents = false)
			: this(FromDatabase(cr, mid, downloadDirsFilesMetadata, downloadDirsFilesContents,
				addRealDirsContents)) {
			//nothing needed here
		}

		public MachineContents(MachineModel m)
			: base(m) {
			if (m.Directories == null || m.Directories.Count == 0)
				return;

			directories = new List<DirContents>();
			foreach (DirModel d in m.Directories)
				directories.Add(new DirContents(d, true, false));
		}

		public ActionResult Upload(Credentials cr) {
			try {
				foreach (DirContents dc in Directories)
					if (!dc.Upload(cr, (MachineIdentity)this).WasSuccessful)
						throw new Exception("directory transfer silently failed");

				//DirActions.Upload(cr, mid, did, new DirContents(did, true));
			} catch (ActionException ex) {
				throw new ActionException("Couldn't upload the machine contents.\n\n"
					+ ex.Message, ActionType.Machine, MemeType.Fuuuuu, ex);
			} catch (Exception ex) {
				throw new ActionException("Error while uploading a whole machine.",
					ActionType.Machine, MemeType.Fuuuuu, ex);
			}

			return new ActionResult("Machine uploaded.",
				"Machine contents were uploaded to the database.", ActionType.Machine);
		}

		public bool SaveToDisk() {
			if (Directories == null || Directories.Count == 0)
				return true;

			foreach (DirContents dc in Directories) {
				dc.SaveToDisk();
			}

			return true;
		}

		private static MachineContents FromDatabase(Credentials cr, MachineIdentity mid,
			bool downloadDirsFilesMetadata = false, bool downloadDirsFilesContents = false,
				bool addRealDirsContents = false) {
			List<DirContents> dirs;
			try {
				if (cr == null)
					throw new ArgumentNullException("cr", "user credentials must be provided");
				if (mid == null)
					throw new ArgumentNullException("mid", "machine identity must be provided");

				CredentialsLib c = cr.ToLib();
				MachineModel m = mid.ToLib();
				DirManipulator.GetDirList(/*c, */m);
				dirs = new List<DirContents>();
				foreach (DirModel d in m.Directories) {
					var did = new DirIdentity(d);
					dirs.Add(new DirContents(cr, mid, did, downloadDirsFilesContents, 
						addRealDirsContents));

					//if (downloadDirsFilesMetadata) {
					//    FileManipulator.GetFileList(c, m, d);
					//    if (downloadDirsFilesContents) {
					//        foreach (FileModel f in d.Files)
					//            FileManipulator.GetFileContent(c, m, d, f);
					//    }
					//}
					//dirs.Add(new DirContents(d, addRealDirsContents, downloadDirsFilesMetadata));
				}
			} catch (Exception ex) {
				throw new ActionException("Unable to get list of directories belonging "
					+ "to the machine.", ActionType.Machine, MemeType.Fuuuuu, ex);
			}

			new ActionResult("Machine directories read.",
				"Got list of directories for your machine from database.", ActionType.Machine);

			return new MachineContents(mid, dirs);
		}

		//private static List<DirContents> DownloadDirsContents(Credentials cr, MachineContents mc) {
		//    try {
		//        if (cr == null)
		//            throw new ArgumentNullException("cr", "user credentials must be provided");
		//        if (mc == null)
		//            throw new ArgumentNullException("mc", "machine contents must be provided");

		//        List<DirContents> allDirs = new List<DirContents>();

		//        foreach (DirIdentity did in mc.Directories) {
		//            DirModel dir = new DirModel(did.Name, did.Description, did.LocalPath);
		//            FileManipulator.GetFileList(new CredentialsLib(cr.Login, cr.Password),
		//                new MachineModel(mc.Identity.Name, mc.Identity.Description),
		//                dir);
		//            List<FileIdentity> files = new List<FileIdentity>();
		//            foreach (FileModel f in dir.Files)
		//                files.Add(new FileIdentity(f.Name, f.Size, f.Hash, f.Uploaded,
		//                    f.Modified));
		//            allDirs.Add(new DirContents(did, files));
		//        }

		//        return allDirs;
		//    } catch (Exception ex) {
		//        throw new ActionException("Failed to get list of files for directories "
		//            + "in a machine.", ActionType.Directory, ex);
		//    }
		//}

		public override string ToString() {
			return new StringBuilder("[").Append(GetArguments()).Append(",Directories= [")
				.Append(String.Join(",", Directories)).Append("] ]").ToString();
		}

		public new MachineModel ToLib() {
			var m = base.ToLib();
			if (Directories == null || Directories.Count == 0)
				return m;

			m.Directories = new List<DirModel>();
			foreach (DirContents dc in Directories)
				m.Directories.Add(dc.ToLib());
			return m;
		}

	}

}
