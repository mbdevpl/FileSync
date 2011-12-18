using System;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	[DataContract]
	public class ActionException : Exception {

		[DataMember]
		public static bool SHOW_EXCEPTION_DETAILS = false;

		private string title;
		/// <summary>
		/// Recommended title for the message box when informing about this exception.
		/// </summary>
		[DataMember]
		public string Title {
			get { return title; }
		}

		private string message;
		[DataMember]
		public string Message {
			get { return message; }
		}

		public ActionException() : base() { }

		public ActionException(string desc)
			: base(desc) {
			// nothing needed here
		}

	}
}
