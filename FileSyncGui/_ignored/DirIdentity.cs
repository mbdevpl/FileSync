using System.Text;

using FileSyncGui.GuiAbstracts;
using FileSyncLib;

namespace FileSyncGui.GuiObjects {

	/// <summary>
	/// Stores the identity of the directory (ie. its abstract name 
	/// and machine-dependant real path).
	/// </summary>
	public class DirIdentity {

		/// <summary>
		/// Used by DirContents, and also by FileIdentity and FileContents classes.
		/// </summary>
		internal static string EmptyLocalPath = "\\";

		private string name;
		public string Name {
			get { return name; }
		}

		private string localPath;
		public string LocalPath {
			get { return localPath; }
		}

		private string description;
		public string Description {
			get { return description; }
		}

		/// <summary>
		/// Creates a new directory identity.
		/// </summary>
		/// <param name="remoteName">name on the remote side</param>
		/// <param name="localPath">real path on a current machine</param>
		/// <param name="description">optional description of the directory</param>
		public DirIdentity(string remoteName, string localPath = null,
			string description = null) {
			if (remoteName == null || remoteName.Length == 0)
				throw new ActionException("Remote name of the directory must be set.",
					ActionType.Directory);
			this.name = remoteName;
			if (localPath != null && localPath.Length == 0)
				throw new ActionException("Local path of the directory must be set.",
					ActionType.Directory);
			this.localPath = (localPath == null ? EmptyLocalPath : localPath);
			this.description = description;
		}

		public DirIdentity(DirIdentity did)
			: this(did.Name, did.LocalPath, did.Description) {
			//nothing needed here
		}

		public DirIdentity(DirModel d)
			: this(d.Name, d.Path, d.Description) {
			//nothing needed here
		}

		public DirModel ToLib() {
			return new DirModel(name, description, localPath);
		}

		private ActionResult UpdateInDatabase(Credentials cr, MachineIdentity mid,
			DirIdentity newDid) {

			throw new ActionException("Directory modification is not properly handled.",
				ActionType.Directory);
		}

		public ActionResult DeleteFromDatabase(Credentials cr, MachineIdentity mid) {

			throw new ActionException("Directory deletion is not properly handled.",
				ActionType.Directory);
		}

		protected StringBuilder GetArguments() {
			return new StringBuilder("Name=").Append(Name).Append(",LocalPath=").Append(LocalPath)
				.Append(",Description=").Append(Description);
		}

		public override string ToString() {
			return new StringBuilder("[").Append(GetArguments()).Append("]").ToString();
		}

	}

}
