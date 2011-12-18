using System;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	[DataContract]
	public class ActionException : Exception {

		public ActionException() : base() { }

		public ActionException(string desc)
			: base(desc) {
			// nothing needed here
		}

	}
}
