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
			set { login = value; }
		}

		private string password;
		/// <summary>
		/// Stores hash of the password of the given user. Actual password is never stored, 
		/// to maximize security.
		/// TODO: make this var internal, is it possible?
		/// </summary>
		[DataMember]
		public string Password {
			get { return password; }
			set { password = value; }
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

		public Credentials() { }

		public Credentials(Credentials cr)
			: this(cr.Login, cr.Password) {
			//nothing needed here
		}

		public static Credentials NewInstance(string login, string password) {
			return new Credentials(login, password);
		}

		public bool Equals(string login, string password) {
			if (!this.Login.Equals(login))
				return false;

			if (!this.Password.Equals(password))
				return false;

			return true;
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
