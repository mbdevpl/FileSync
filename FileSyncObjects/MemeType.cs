using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Defines memes that may be shown beside the error messages and success reports.
	/// </summary>
	[DataContract]
	public enum MemeType {
		[EnumMember]
		AreYouFuckingKiddingMe,
		[EnumMember]
		FuckYea,
		[EnumMember]
		Fuuuuu
	}

}
