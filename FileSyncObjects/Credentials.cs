using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Stores credentials of the user.
	/// </summary>
	[DataContract]
	public class Credentials {

		private string login;
		[DataMember]
		public string Login {
			get { return login; }
		}

		private string password;
		[DataMember]
		internal string Password {
			get { return password; }
		}

		/// <summary>
		/// Creates new user credentials.
		/// </summary>
		/// <param name="login">user login</param>
		/// <param name="password">user password (please hash it before putting it here)</param>
		public Credentials(string login, string password) {
			this.login = login;
			this.password = password;
		}

		protected Credentials() { }

		public Credentials(Credentials cr)
			: this(cr.Login, cr.Password) {
			//nothing needed here
		}

		public override bool Equals(object o) {
			if (!o.GetType().Equals(typeof(Credentials)))
				return base.Equals(o);

			Credentials c = (Credentials)o;

			if (c.Login.Equals(this.Login) && c.Password.Equals(this.Password))
				return true;

			return false;
		}

	}

}
