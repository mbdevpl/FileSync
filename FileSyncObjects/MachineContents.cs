using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Stores the identity of a machine and a list of identities of directories,
	/// which it contains.
	/// </summary>
	[DataContract]
	public class MachineContents : MachineIdentity {

		private List<DirectoryContents> directories;
		[DataMember]
		public List<DirectoryContents> Directories {
			get { return directories; }
			set { directories = value; }
		}

		public MachineContents(string name, string description = null,
			List<DirectoryContents> directories = null)
			: base(name, description) {
			this.directories = directories;
		}

		/// <summary>
		/// Creates new contents of the machine.
		/// </summary>
		/// <param name="mid">ientity of the machine</param>
		/// <param name="directories">list of directories of the machine</param>
		public MachineContents(MachineIdentity mid, List<DirectoryContents> directories = null)
			: this(mid.Name, mid.Description, directories) {
			//nothing needed here
		}

		public MachineContents(MachineContents mc)
			: this(mc.Name, mc.Description, mc.Directories) {
			//nothing needed here
		}

		protected MachineContents() : base() { }

	}

}
