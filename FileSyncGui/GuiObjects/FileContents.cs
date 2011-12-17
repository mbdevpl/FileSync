using System;
using System.IO;
using System.Text;

using FileSyncGui.GuiAbstracts;
using FileSyncLib;
using FileSyncGui.GuiActions;

namespace FileSyncGui.GuiObjects {

	/// <summary>
	/// Stores the complete data of a single file, without the context eg. directory or machine.
	/// </summary>
	public class FileContents : FileIdentity {

		private string contents;
		protected string Contents {
			get { return contents; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="modified"></param>
		/// <param name="contents">string that contains contents of the file</param>
		/// <param name="uploaded"></param>
		/// <param name="fileType"></param>
		/// <param name="size"></param>
		/// <param name="hash"></param>
		public FileContents(string name, DateTime modified, string contents = null,
				DateTime? uploaded = null, FileType fileType = FileType.PlainText, long size = 0,
				string hash = null)
			: base(name, modified, uploaded, fileType, size, hash) {

			this.contents = contents;
			if (this.contents != null) {
				this.size = (long)contents.Length;
				this.hash = SafetyActions.ComputeHash(contents);
			}
		}

		public FileContents(FileContents fc)
			: this(fc.Name, fc.Modified, fc.Contents, fc.Uploaded, fc.Type, fc.Size, fc.Hash) {
			//nothing needed here
		}

		public FileContents(FileIdentity identity, string contents = null)
			: base(identity) {
			this.contents = contents;
		}

		/// <summary>
		/// Creates file contents from a file in a specified local path, and creates its identity,
		/// which is based on the contents and meta-data of that real file.
		/// </summary>
		/// <param name="filePath"></param>
		public FileContents(string filePath)
			: this(FileContents.FromFile(filePath)) {
			//nothing needed here
		}

		public FileContents(FileModel f)
			: this(f.Name, f.Modified, GetStringFromBytes(f.Data), f.Uploaded,
				GetFileTypeFromLib(f.Typename), f.Size, f.Hash) {
			//nothing needed here
		}

		private static string GetStringFromBytes(byte[] bytes) {
			if (bytes == null)
				return null;

			if (bytes.Length == 0)
				return "";

			UTF8Encoding encoding = new UTF8Encoding();
			return encoding.GetString(bytes);
		}

		protected new static FileContents FromFile(string filePath) {
			string s = ReadFrom(filePath);

			//just to perform basic error checking, to keep the code DRY:
			var i = FileIdentity.FromFile(filePath);

			return new FileContents(filePath.Substring(filePath.LastIndexOf('\\') + 1),
				System.IO.File.GetLastWriteTime(filePath), s, DateTime.Now, FileType.PlainText);
		}

		private static string ReadFrom(string filePath) {
			string contents = String.Empty;
			try {
				StreamReader reader = new StreamReader(filePath);
				contents = reader.ReadToEnd();
				reader.Close();
			} catch (Exception ex) {
				throw new ActionException("Error while reading file data from disk. "
					+ "File path was: '" + filePath + "'.", ActionType.File, MemeType.Fuuuuu, ex);
			}
			return contents;
		}

		private bool WriteTo(string filePath) {
			try {

				StreamWriter writer = new StreamWriter(filePath);
				writer.Write(contents);
				writer.Flush();
				writer.Close();
			} catch (Exception ex) {
				throw new ActionException("Error while writing file to disk, to path: '"
					+ filePath + "'.", ActionType.File, MemeType.Fuuuuu, ex);
			}
			return true;
		}

		public ActionResult Upload(Credentials cr, MachineIdentity mid, DirIdentity did) {
			try {
				FileManipulator.AddFile(cr.ToLib(), mid.ToLib(), did.ToLib(), this.ToLib());
			} catch (Exception ex) {
				throw new ActionException("Error occurred while file was uploaded.",
					ActionType.File, MemeType.Fuuuuu, ex);
			}

			return new ActionResult("File uploaded.", "File was put into the database.",
				ActionType.File);
		}

		private static FileContents FromDatabase(Credentials cr, MachineIdentity mid,
			DirIdentity did, FileIdentity fid) {
			
			FileModel f = null;
			try {
				f = fid.ToLib();
				FileManipulator.GetFileContent(cr.ToLib(), mid.ToLib(), did.ToLib(), f);
			} catch (Exception ex) {
				throw new ActionException("Error while downloading file contents from database.",
					ActionType.File, MemeType.Fuuuuu, ex);
			}

			return new FileContents(f);
		}

		public ActionResult DeleteFromDatabase(Credentials cr, MachineIdentity mid,
			DirIdentity did) {

			throw new ActionException("File deletion is not properly handled.",
				ActionType.File);
		}

		public bool SaveToDisk(DirIdentity did) {
			return WriteTo(did.LocalPath + "\\" + this.Name);
		}

		private bool IsIdentityValid() {
			return Size.Equals(contents.Length)
				&& Hash.Equals(SafetyActions.ComputeHash(contents));
		}

		public new FileModel ToLib() {
			FileModel f = new FileModel(Name, Size, Hash, StringEnum.GetStringValue(Type),
				Uploaded, Modified);

			System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
			f.Data = encoding.GetBytes(Contents);

			return f;
		}

		public override string ToString() {
			return new StringBuilder("[").Append(base.GetArguments())
				.Append(",Contents=").Append(Contents).Append("]").ToString();
		}

	}
}
