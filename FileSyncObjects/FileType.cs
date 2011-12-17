using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Stores possible types of files. This set should be identical to the one stored 
	/// in Types table of FileSync database.
	/// </summary>
	[DataContract]
	public enum FileType {
		[StringValue("plain text")]
		[EnumMember]
		PlainText,
		[StringValue("formatted text")]
		[EnumMember]
		FormattedText,
		[StringValue("audio")]
		[EnumMember]
		Audio,
		[StringValue("video")]
		[EnumMember]
		Video,
		[StringValue("image")]
		[EnumMember]
		Image,
		[StringValue("archive")]
		[EnumMember]
		Archive,
		[StringValue("executable")]
		[EnumMember]
		Executable,
		[StringValue("other")]
		[EnumMember]
		Other
	}

}
