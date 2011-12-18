using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using FileSyncGui.GuiAbstracts;
using FileSyncLib;

namespace FileSyncGui.GuiObjects {

	/// <summary>
	/// Stores the directory identity (remote name and local path) with the identities 
	/// of the files belonging to that directory.
	/// </summary>
	public class DirContents : DirIdentity {

		private List<FileContents> files;
		public List<FileContents> Files {
			get { return files; }
		}

		/// <summary>
		/// Creates a new object representing contents of a single directory. It may be empty, 
		/// but may be also filled automatically using the real path stored in its identity.
		/// Also, additional unreal files may be added to the contents, both lists will be merged.
		/// </summary>
		/// <param name="remoteName">name on the remote side</param>
		/// <param name="localPath">real path on a current machine</param>
		/// <param name="description">optional description of the directory</param>
		/// <param name="files">initial files that the directory will contain</param>
		/// <param name="addRealFiles">files stored under the real path are added</param>
		public DirContents(string remoteName, string localPath = null, string description = null,
				List<FileContents> files = null, bool addRealFiles = false)
			: base(remoteName, localPath, description) {
			this.files = files;

			if (localPath != null && !localPath.Equals(EmptyLocalPath) && addRealFiles) {
				if (files == null)
					files = new List<FileContents>();
				AddRealFiles();

				//removing duplicates
				RemoveDuplicateFiles();
			}
		}

		/// <summary>
		/// Creates a directory that 
		/// </summary>
		/// <param name="did">identity of the directory</param>
		/// <param name="files">initial files that the directory will contain</param>
		/// <param name="addRealFiles">files stored under the real path are added</param>
		public DirContents(DirIdentity did, List<FileContents> files = null,
				bool addRealFiles = false)
			: this(did.Name, did.LocalPath, did.Description, files, addRealFiles) {
			//nothing needed here
		}

		public DirContents(DirContents dc, bool addRealFiles = false)
			: this(dc.Name, dc.LocalPath, dc.Description, dc.Files, addRealFiles) {
			//nothing needed here
		}

		public DirContents(Credentials cr, MachineIdentity mid, DirIdentity did,
				bool downloadFilesContents = false, bool addRealFiles = false)
			: this(FromDatabase(cr, mid, did, downloadFilesContents), addRealFiles) {
			//nothing needed here
		}

		public DirContents(DirModel d, bool addRealFiles = false, bool addRemoteFiles = true)
			: this(d.Name, d.Description, d.Path, null, addRealFiles) {

			if ((addRealFiles && LocalPath != null && !LocalPath.Equals(EmptyLocalPath))
				|| (addRemoteFiles && d.Files != null && d.Files.Count > 0))
				this.files = new List<FileContents>();

			if (addRemoteFiles) AddRemoteFiles(d.Files);
			if (addRealFiles) AddRealFiles();

			if (addRemoteFiles && addRealFiles)
				RemoveDuplicateFiles();
		}

		/// <summary>
		/// This function assumes that the files list is initialized. It also performs no cleanup 
		/// of duplicates in case of incorrect of consequtive use.
		/// </summary>
		private void AddRealFiles() {
			if (LocalPath == null || LocalPath.Equals(EmptyLocalPath))
				return;

			string[] filePaths = Directory.GetFiles(LocalPath);

			if (filePaths == null || filePaths.Length == 0)
				return;

			if (Files == null)
				files = new List<FileContents>();

			foreach (string path in filePaths) {
				FileContents fc = new FileContents(path);
				Files.Add(fc);
			}
		}

		/// <summary>
		/// Adds the files from the input list to the list of files of this object. This method
		/// performs no cleanup, it also does not detect if is being used for the first 
		/// or second time.
		/// </summary>
		/// <param name="files"></param>
		private void AddRemoteFiles(List<FileModel> files) {
			if (files == null || files.Count == 0)
				return;

			foreach (FileModel f in files)
				Files.Add(new FileContents(f));
		}

		/// <summary>
		/// Removes the duplicate files inside this directory. It performes basing on 
		/// the rule, which states that file names must not repeat.
		/// </summary>
		private void RemoveDuplicateFiles() {
			// Possible second rule example (not introduced):
			// If hash of file.txt equals to hash of file.bmp, both are preserved.
			// If, on the other hand, hash of file.txt equals to hash of file2.txt the later file 
			// is preserved.

			if (Files == null || Files.Count <= 1)
				return;

			List<FileContents> sortedFiles = new List<FileContents>(Files);
			sortedFiles.Sort(FileComparison);

			for (int i = 0; i < sortedFiles.Count - 1; i++) {
				if (sortedFiles[i].Name.Equals(sortedFiles[i + 1].Name)) {
					sortedFiles.RemoveAt(i);
					i--;
				}
			}

			files = sortedFiles;
		}

		public static int FileComparison(FileContents x, FileContents y) {
			int nameComparison = x.Name.CompareTo(y.Name);
			if (nameComparison != 0)
				return nameComparison;

			int modifiedComparison = x.Modified.CompareTo(y.Modified);
			if (modifiedComparison != 0)
				return modifiedComparison;

			int sizeComparison = x.Size.CompareTo(y.Size);
			if (sizeComparison != 0)
				return sizeComparison * (-1);

			int hashComparison = x.Hash.CompareTo(y.Hash);
			if (hashComparison != 0)
				return hashComparison;

			return 0;
		}

		public new DirModel ToLib() {
			DirModel d = base.ToLib();
			if (Files == null || Files.Count == 0)
				return d;

			d.Files = new List<FileModel>();
			foreach (FileContents f in Files)
				d.Files.Add(f.ToLib());

			return d;
		}

		public bool SaveToDisk() {
			if (Files == null || Files.Count == 0)
				return true;

			DirIdentity did = (DirIdentity)this;
			foreach (FileContents fc in Files)
				fc.SaveToDisk(did);

			return true;

			//throw new ActionException("PREVENTED HDD DATA MODIFICATION:"
			//    + "\n\nCurrent directory contents: " + new DirContents(this.Identity, true).ToString()
			//    + "\n\nContents downloaded: " + this.ToString(), ActionType.Directory);
		}

		public ActionResult Upload(Credentials cr, MachineIdentity mid) {
			CredentialsLib c = cr.ToLib();
			MachineModel m = mid.ToLib();
			DirModel d = this.ToLib();

			try {
				DirManipulator.AddDirectory(c, m, d);
			} catch (Exception ex) {
				throw new ActionException("Failed to create a remote directory for the files.",
					ActionType.Directory, MemeType.Fuuuuu, ex);
			}

			foreach (FileContents fc in Files) {
				FileContents fcUp = null;
				try {
					if (fc.Size == 0)
						fcUp = new FileContents(this.LocalPath + "\\" + fc.Name);
					else
						fcUp = fc;
					//if (!fid.Equals((FileIdentity)fc))
					//    throw new ActionException("Tried to upload a file that had "
					//        + "different identity than the contents generate.", ActionType.File);
					ActionResult fileSync = fcUp.Upload(cr, mid, this);
					if (!fileSync.WasSuccessful)
						return fileSync;
				} catch (ActionException ex) {
					throw new ActionException("Couldn't upload the directory contents.\n\n"
						+ ex.Message, ActionType.Directory, MemeType.Fuuuuu, ex);
				} catch (Exception ex) {
					throw new ActionException("Error while uploading a directory.",
						ActionType.Directory, MemeType.AreYouFuckingKiddingMe, ex);
				}
			}

			return new ActionResult("Directory was uploaded.",
				"The selected dir was successfully put into database.", ActionType.Directory);
		}

		private static DirContents FromDatabase(Credentials cr, MachineIdentity mid, 
			DirIdentity did, bool downloadFilesContents = false) {

			List<FileContents> files = new List<FileContents>();
			var c = cr.ToLib();
			var m = mid.ToLib();
			DirModel d = did.ToLib();

			try {
				FileManipulator.GetFileList(c, m, d);

				foreach (FileModel f in d.Files) {
					if (downloadFilesContents) {
						//adding contents to 'f'
						FileManipulator.GetFileContent(c, m, d, f);
					}
					//adding new gui object to list
					files.Add(new FileContents(f));
				}
			} catch (ActionException ex) {
				throw new ActionException("Unable to download directory contents.\n\n"
					+ ex.Message + ".", ActionType.Directory, MemeType.Fuuuuu, ex);
			} catch (Exception ex) {
				throw new ActionException("Error while downloading directory contents.",
					ActionType.Directory, MemeType.Fuuuuu, ex);
			}

			//does not add local files, because this is supposed to happen in the constructor that
			// launches this static private method
			return new DirContents(did, files, false);
		}

		public override string ToString() {
			return new StringBuilder("[").Append(GetArguments())
				.Append(",Files.Count=").Append(Files == null ? Files.Count : -1)
				.Append(",Files= [").Append(String.Join(",", Files)).Append("] ]").ToString();
		}

	}
}
