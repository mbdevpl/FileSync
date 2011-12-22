using System.Text;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Stores the identity of the directory (ie. its abstract name 
	/// and machine-dependant real path).
	/// </summary>
	[DataContract]
	public class DirectoryIdentity {

		/// <summary>
		/// Used by DirContents, and also by FileIdentity and FileContents classes.
		/// </summary>
		internal static string EmptyLocalPath = "\\";

		private int id;
		public int Id {
			get { return id; }
			set { id = value; }
		}

		int owner;
		public int Owner {
			get { return owner; }
			set { owner = value; }
		}

		private string name;
		[DataMember]
		public string Name {
			get { return name; }
			set { name = value; }
		}

		private string localPath;
		[DataMember]
		public string LocalPath {
			get { return localPath; }
			set { localPath = value; }
		}

		private string description;
		[DataMember]
		public string Description {
			get { return description; }
			set { description = value; }
		}

		/// <summary>
		/// Creates a new directory identity.
		/// </summary>
		/// <param name="remoteName">name on the remote side</param>
		/// <param name="localPath">real path on a current machine</param>
		/// <param name="description">optional description of the directory</param>
		public DirectoryIdentity(string remoteName, string localPath = null,
			string description = null) {
			if (remoteName == null || remoteName.Length == 0)
				throw new ActionException("Remote name of the directory must be set.",
					ActionType.Directory, MemeType.AreYouFuckingKiddingMe);
			this.name = remoteName;
			if (localPath != null && localPath.Length == 0)
				throw new ActionException("Local path of the directory must be set.",
					ActionType.Directory, MemeType.AreYouFuckingKiddingMe);
			this.localPath = (localPath == null ? EmptyLocalPath : localPath);
			this.description = description;
		}

		public DirectoryIdentity(DirectoryIdentity did)
			: this(did.Name, did.LocalPath, did.Description) {
			//nothing needed here
		}

		protected DirectoryIdentity() { }

		protected StringBuilder GetArguments() {
			return new StringBuilder("Name=").Append(Name).Append(",LocalPath=").Append(LocalPath)
				.Append(",Description=").Append(Description);
		}

		public override string ToString() {
			return new StringBuilder("[").Append(GetArguments()).Append("]").ToString();
		}

	}

}
