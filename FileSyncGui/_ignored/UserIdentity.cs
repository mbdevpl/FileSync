using System;

using FileSyncGui.GuiAbstracts;
using FileSyncLib;

namespace FileSyncGui.GuiObjects {

	/// <summary>
	/// Stores identity of a user ie. his credentials and personal information, 
	/// without machines, directories and files.
	/// </summary>
	public class UserIdentity : Credentials {

		private string name;
		public string Name {
			get { return name; }
		}

		private string email;
		public string Email {
			get { return email; }
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

		public UserIdentity(UserIdentity uid)
			: base((Credentials)uid) {
			this.name = uid.Name;
			this.email = uid.Email;
		}

		/// <summary>
		/// Creates new user identity.
		/// </summary>
		/// <param name="cr">user credentials</param>
		/// <param name="fromDatabase">if true, the object contents will be downloaded 
		/// from the database</param>
		public UserIdentity(Credentials cr, bool fromDatabase)
			: this(fromDatabase ? FromDatabase(cr) : (UserIdentity)cr) {
			//nothing needed here
		}

		public UserIdentity(UserModel u)
			: this(u.Login, u.Pass, u.Fullname, u.Email) {
			//nothing needed here
		}

		/// <summary>
		/// Downloads the content from database and puts it into the object.
		/// </summary>
		/// <param name="cr">credentials needed to download the data</param>
		/// <returns></returns>
		private static UserIdentity FromDatabase(Credentials cr) {
			try {
				UserModel u = UserManipulator.GetUser(cr.ToLib());
				return new UserIdentity(cr, u.Fullname, u.Email);
			} catch (Exception ex) {
				throw new ActionException("Unable to get user details for the given credentials.",
					ActionType.User, MemeType.Fuuuuu, ex);
			}
		}

		public new ActionResult AddToDatabase() {
			try {
				UserManipulator.Add(this.ToLib());
			} catch (Exception ex) {
				throw new ActionException("Unable to create new user account.", ActionType.User,
					MemeType.AreYouFuckingKiddingMe, ex);
			}
			return new ActionResult("New account creted.", "Your account was created successfully.",
				ActionType.User);
		}

		public ActionResult UpdateInDatabase(UserIdentity newUid) {
			throw new ActionException("Updating user details is not properly handled...",
				ActionType.User, MemeType.Fuuuuu);
		}

		/// <summary>
		/// Converts this object to its FileSyncLib representation. Convertion may not be lossless, 
		/// but will be as perfect as possible.
		/// </summary>
		/// <returns>closest FileSyncLib representation of this object</returns>
		public new UserModel ToLib() {
			return new UserModel(Login, Password, Name, Email);
		}

	}

}
