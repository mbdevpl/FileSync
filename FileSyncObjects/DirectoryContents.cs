using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Stores the directory identity (remote name and local path) with the identities 
	/// of the files belonging to that directory.
	/// </summary>
	[DataContract]
	public class DirectoryContents : DirectoryIdentity {

		private List<FileContents> files;
		[DataMember]
		public List<FileContents> Files {
			get { return files; }
			set { files = value; }
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
		public DirectoryContents(string remoteName, string localPath = null, 
				string description = null, List<FileContents> files = null)
			: base(remoteName, localPath, description) {
			this.files = files;
		}

		/// <summary>
		/// Creates a directory that 
		/// </summary>
		/// <param name="did">identity of the directory</param>
		/// <param name="files">initial files that the directory will contain</param>
		/// <param name="addRealFiles">files stored under the real path are added</param>
		public DirectoryContents(DirectoryIdentity did, List<FileContents> files = null)
			: this(did.Name, did.LocalPath, did.Description, files) {
			//nothing needed here
		}

		public DirectoryContents(DirectoryContents dc)
			: this(dc.Name, dc.LocalPath, dc.Description, dc.Files) {
			//nothing needed here
		}

		protected DirectoryContents() : base() { }

		public override string ToString() {
			return new StringBuilder("[").Append(GetArguments())
				.Append(",Files.Count=").Append(Files == null ? Files.Count : -1)
				.Append(",Files= [").Append(String.Join(",", Files)).Append("] ]").ToString();
		}

	}
}
