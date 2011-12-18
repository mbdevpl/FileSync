using System;
using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Stores identity of a user ie. his credentials and personal information, 
	/// without machines, directories and files.
	/// </summary>
	[DataContract]
	public class UserIdentity : Credentials {

		private int id;
		public int Id {
			get { return id; }
			set { id = value; }
		}

		private string name;
		[DataMember]
		public string Name {
			get { return name; }
			set { name = value; }
		}

		private string email;
		[DataMember]
		public string Email {
			get { return email; }
			set { email = value; }
		}

		private DateTime lastLogin;
		[DataMember]
		public DateTime LastLogin {
			get { return lastLogin; }
			set { lastLogin = value; }
		}

		/// <summary>
		/// Creates new user identity.
		/// </summary>
		/// <param name="login">user login</param>
		/// <param name="password">user password</param>
		/// <param name="name">full name or some nickname of the user</param>
		/// <param name="email">email address of the user</param>
		public UserIdentity(string login, string password, string name = null,
				string email = null)
			: base(login, password) {
			this.name = name;
			this.email = email;
		}

		/// <summary>
		/// Creates new user identity.
		/// </summary>
		/// <param name="cr">user credentials</param>
		/// <param name="name">full name or some nickname of the user</param>
		/// <param name="email">email address of the user</param>
		public UserIdentity(Credentials cr, string name = null, string email = null)
			: this(cr.Login, cr.Password, name, email) {
			//nothing needed here
		}

		protected UserIdentity() : base() { }

		public UserIdentity(UserIdentity uid)
			: base((Credentials)uid) {
			this.name = uid.Name;
			this.email = uid.Email;
		}

	}

}
