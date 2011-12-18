using System;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Defines the identity of a single machine.
	/// </summary>
	[DataContract]
	public class MachineIdentity {

		private int id;
		public int Id {
			get { return id; }
			set { id = value; }
		}

		int user;
		public int User {
			get { return user; }
			set { user = value; }
		}

		private string name;
		[DataMember]
		public string Name {
			get { return name; }
		}

		private string description;
		[DataMember]
		public string Description {
			get { return description; }
		}

		public MachineIdentity(int id, string name, string description = null) {
			this.id = id;
			this.name = name;
			this.description = description;
		}

		public MachineIdentity(string name, string description = null)
			: this(-1, name, description) {
			//nothing needed here
		}

		public MachineIdentity(MachineIdentity mid)
			: this(mid.Name, mid.Description) {
			//nothing needed here
		}

		public MachineIdentity() { }

		protected StringBuilder GetArguments() {
			return new StringBuilder("Name=").Append(Name).Append(",Description=")
				.Append(Description);
		}

		override public string ToString() {
			return new StringBuilder("[").Append(GetArguments()).Append("]").ToString();
		}

	}

}
