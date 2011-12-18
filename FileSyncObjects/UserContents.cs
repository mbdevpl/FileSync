using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Stores contents of a single user account.
	/// </summary>
	[DataContract]
	public class UserContents : UserIdentity {

		private List<MachineContents> machines;
		[DataMember]
		public List<MachineContents> Machines {
			get { return machines; }
		}

		/// <summary>
		/// Creates new representation of contents of the user account.
		/// </summary>
		/// <param name="login">user login</param>
		/// <param name="password">user password</param>
		/// <param name="name">full name or some nickname of the user</param>
		/// <param name="email">email address of the user</param>
		/// <param name="machines">list of contents' of the user's machines</param>
		public UserContents(string login, string password, string name = null,
				string email = null, List<MachineContents> machines = null)
			: base(login, password, name, email) {
			this.machines = machines;
		}

		/// <summary>
		/// Creates new representation of contents of the user account.
		/// </summary>
		/// <param name="identity">user identity</param>
		/// <param name="machines">list of contents' of the user's machines</param>
		public UserContents(UserIdentity identity, List<MachineContents> machines = null)
			: this(identity.Login, identity.Password, identity.Name,
				identity.Email, machines) {
			//nothing needed here
		}

		protected UserContents() : base() { }

		public UserContents(UserContents uc)
			: this((UserIdentity)uc, uc.Machines) {
			//nothing needed here
		}

	}

}
