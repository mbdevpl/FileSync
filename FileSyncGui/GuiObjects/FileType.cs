using FileSyncGui.GuiAbstracts;

namespace FileSyncGui.GuiObjects {

	/// <summary>
	/// Stores possible types of files. This set should be identical to the one stored 
	/// in Types table of FileSync database.
	/// </summary>
	public enum FileType {
		[StringValue("plain text")]
		PlainText,
		[StringValue("formatted text")]
		FormattedText,
		[StringValue("audio")]
		Audio,
		[StringValue("video")]
		Video,
		[StringValue("image")]
		Image,
		[StringValue("archive")]
		Archive,
		[StringValue("executable")]
		Executable,
		[StringValue("other")]
		Other
	}

}
