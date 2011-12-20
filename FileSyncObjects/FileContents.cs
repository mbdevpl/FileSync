using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Stores the complete data of a single file, without the context eg. directory or machine.
	/// </summary>
	[DataContract]
	public class FileContents : FileIdentity {

		private string contents;
		[DataMember]
		protected string Contents {
			get { return contents; }
			set { contents = value; }
		}

		/// <summary>
		/// It is a virtual property, performing on-the-fly conversion to byte[] or from string. 
		/// The real data is stored in Contents property.
		/// </summary>
		public byte[] Data {
			get { return GetBytesFromString(contents); }
			set { contents = GetStringFromBytes(value); }
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
				this.hash = Security.ComputeHash(contents);
			}
		}

		public FileContents(string name, DateTime modified, byte[] bytes = null,
				DateTime? uploaded = null, FileType fileType = FileType.PlainText, long size = 0,
				string hash = null)
			: this(name, modified, GetStringFromBytes(bytes), uploaded, fileType, size, hash) {
		}

		public FileContents(FileContents fc)
			: this(fc.Name, fc.Modified, fc.Contents, fc.Uploaded, fc.Type, fc.Size, fc.Hash) {
			//nothing needed here
		}

		public FileContents(FileIdentity identity, string contents = null)
			: base(identity) {
			this.contents = contents;
		}

		protected FileContents() : base() { }

		private static string GetStringFromBytes(byte[] bytes) {
			if (bytes == null)
				return null;

			if (bytes.Length == 0)
				return "";

			UTF8Encoding encoding = new UTF8Encoding();
			return encoding.GetString(bytes);
		}

		private static byte[] GetBytesFromString(string contents) {
			if (contents == null)
				return null;

			if(contents.Length == 0) 
				return new byte[0];

			UTF8Encoding encoding = new UTF8Encoding();
			return encoding.GetBytes(contents);
		}

		public override string ToString() {
			return new StringBuilder("[").Append(base.GetArguments())
				.Append(",Contents=").Append(Contents).Append("]").ToString();
		}

	}
}
