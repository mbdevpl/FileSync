using System;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Adds a 'binding' between enum element and a string
	/// </summary>
	[DataContract]
	public class StringValueAttribute : Attribute {

		private string value;
		[DataMember]
		public string Value {
			get { return value; }
		}

		public StringValueAttribute(string value) {
			this.value = value;
		}

	}
}
