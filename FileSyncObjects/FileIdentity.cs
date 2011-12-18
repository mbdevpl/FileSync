using System;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Stores the identity of the file, without its contents.
	/// </summary>
	[DataContract]
	public class FileIdentity {

		private int id;
		/// <summary>
		/// Id of this file.
		/// </summary>
		public int Id {
			get { return id; }
			set { id = value; }
		}

		int dir;
		/// <summary>
		/// Id of dir containing this file.
		/// </summary>
		public int Dir {
			get { return dir; }
			set { dir = value; }
		}

		int content;
		/// <summary>
		/// Id for file contents.
		/// </summary>
		public int Content {
			get { return content; }
			set { content = value; }
		}

		private string name;
		[DataMember]
		public string Name {
			get { return name; }
			set { name = value; }
		}

		private DateTime modified;
		[DataMember]
		public DateTime Modified {
			get { return modified; }
			set { modified = value; }
		}

		private DateTime uploaded;
		[DataMember]
		public DateTime Uploaded {
			get { return uploaded; }
			set { uploaded = value; }
		}

		private FileType type;
		[DataMember]
		public FileType Type {
			get { return type; }
			set { type = value; }
		}

		protected long size;
		[DataMember]
		public long Size {
			get { return size; }
			set { size = value; }
		}

		protected string hash;
		[DataMember]
		public string Hash {
			get { return hash; }
			set { hash = value; }
		}

		/// <summary>
		/// Creates a complete identity of a single file.
		/// </summary>
		/// <param name="name">file name</param>
		/// <param name="modified">date when file was last modified</param>
		/// <param name="uploaded">date when file was uploaded</param>
		/// <param name="fileType">type of the file</param>
		/// <param name="size">lentgh of the files in bytes</param>
		/// <param name="hash">hash computed by some arbitrary algorithm</param>
		public FileIdentity(int id, string name, DateTime modified, DateTime? uploaded = null,
			FileType fileType = FileType.PlainText, long size = 0, string hash = null) {
			this.id = id;
			this.name = name;
			this.modified = modified;
			if (uploaded == null)
				this.uploaded = DateTime.Now;
			else
				this.uploaded = (DateTime)uploaded;
			this.type = fileType;
			this.size = size;
			this.hash = hash;
		}

		public FileIdentity(string name, DateTime modified, DateTime? uploaded = null,
			FileType fileType = FileType.PlainText, long size = 0, string hash = null)
			: this(-1, name, modified, uploaded, fileType, size, hash) {
		}

		public FileIdentity(FileIdentity fid)
			: this(fid.Name, fid.Modified, fid.Uploaded, fid.Type, fid.Size, fid.Hash) {
			//nothing needed here
		}

		public FileIdentity() { }

		protected StringBuilder GetArguments() {
			return new StringBuilder("Name=").Append(Name).Append(",Size=").Append(Size)
				.Append(",Type=").Append(Type.ToString()).Append(",Hash=").Append(Hash)
				.Append(",Uploaded=").Append(Uploaded).Append(",Modified=")
				.Append(Modified);
		}

		public override string ToString() {
			return new StringBuilder("[").Append(GetArguments()).Append("]").ToString();
		}

		/// <summary>
		/// Compares metadata of the identity of the file. Does not compare the actual contents,
		/// and does not check whether the given object contains the contents of the file (or only
		/// its identity).
		/// </summary>
		/// <param name="o">object to compare</param>
		/// <returns>true if: name, size, hash and modification date are all the same 
		/// in both objects</returns>
		public override bool Equals(object o) {
			if (!o.GetType().Equals(typeof(FileIdentity))
					&& !o.GetType().Equals(typeof(FileContents)))
				return false;

			FileIdentity fid = (FileIdentity)o;
			return (fid.Name.Equals(Name) && fid.Size == Size && fid.Hash.Equals(Hash)
				&& fid.Modified.Equals(Modified));
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

	}

}
