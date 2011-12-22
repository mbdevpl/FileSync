using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Types of actions that may be performed by the user of FileSync.
	/// </summary>
	[DataContract]
	public enum ActionType {
		[EnumMember]
		User,
		[EnumMember]
		Machine,
		[EnumMember]
		Directory,
		[EnumMember]
		File
	}
}
