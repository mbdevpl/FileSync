using System;

namespace FileSyncGui.GuiAbstracts {

	/// <summary>
	/// Adds a 'binding' between enum element and a string
	/// </summary>
	public class StringValueAttribute : Attribute {

		private string value;
		public string Value {
			get { return value; }
		}

		public StringValueAttribute(string value) {
			this.value = value;
		}

	}
}
