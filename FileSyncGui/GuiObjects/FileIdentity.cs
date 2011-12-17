using System;
using System.Text;

using FileSyncGui.GuiAbstracts;
using FileSyncLib;
using System.IO;

namespace FileSyncGui.GuiObjects {

	/// <summary>
	/// Stores the identity of the file, without its contents.
	/// </summary>
	public class FileIdentity {

		private string name;
		public string Name {
			get { return name; }
		}

		private DateTime modified;
		public DateTime Modified {
			get { return modified; }
		}

		private DateTime uploaded;
		public DateTime Uploaded {
			get { return uploaded; }
		}

		private FileType type;
		public FileType Type {
			get { return type; }
		}

		protected long size;
		public long Size {
			get { return size; }
		}

		protected string hash;
		public string Hash {
			get { return hash; }
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
		public FileIdentity(string name, DateTime modified, DateTime? uploaded = null,
			FileType fileType = FileType.PlainText, long size = 0, string hash = null) {
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

		public FileIdentity(FileIdentity fid)
			: this(fid.Name, fid.Modified, fid.Uploaded, fid.Type, fid.Size, fid.Hash) {
			//nothing needed here
		}

		public FileIdentity(FileModel f)
			: this(f.Name, f.Modified, f.Uploaded, GetFileTypeFromLib(f.Typename), f.Size,
				f.Hash) {
			//nothing needed here
		}

		/// <summary>
		/// Creates file identity of a real file.
		/// </summary>
		/// <param name="filePath">path to file</param>
		public FileIdentity(string filePath)
			: this(FileIdentity.FromFile(filePath)) {
			//nothing needed here
		}

		protected static FileIdentity FromFile(string filePath) {
			if (filePath == null)
				throw new ActionException("No file path was provided.", ActionType.File,
					MemeType.Fuuuuu);

			int lastSlash = filePath.LastIndexOf('\\') + 1;

			string dirPath = filePath.Substring(0, lastSlash);
			string fileName = filePath.Substring(lastSlash);

			if (dirPath.Length == 0 || fileName.Length == 0
					|| filePath.Equals(DirIdentity.EmptyLocalPath))
				throw new ActionException("Unable to get file metadata from an ivalid path: '"
					+ filePath + "'.", ActionType.File, MemeType.Fuuuuu);

			return new FileIdentity(fileName, System.IO.File.GetLastWriteTime(filePath));
		}

		public ActionResult UpdateInDatabase(Credentials cr, MachineIdentity mid, DirIdentity did,
			FileIdentity newFid) {

			throw new ActionException("File modification is not properly handled.",
				ActionType.File);
		}

		public FileModel ToLib() {
			return new FileModel(Name, Size, Hash, StringEnum.GetStringValue(Type),
				Uploaded, Modified);
		}

		protected static FileType GetFileTypeFromLib(string typeName) {
			return FileType.PlainText;
		}

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
