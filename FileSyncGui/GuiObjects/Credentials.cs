using System;

using FileSyncGui.GuiAbstracts;
using FileSyncLib;

namespace FileSyncGui.GuiObjects {

	/// <summary>
	/// Stores credentials of the user.
	/// </summary>
	public class Credentials {
		private string login;
		public string Login {
			get { return login; }
		}

		private string password;
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

		public Credentials(Credentials cr)
			: this(cr.Login, cr.Password) {
			//nothing needed here
		}

		private Credentials(CredentialsLib c)
			: this(c.Login, c.Pass) {
			//nothing needed here
		}

		/// <summary>
		/// Converts this object to its FileSyncLib representation. Convertion may not be lossless, 
		/// but will be as perfect as possible.
		/// </summary>
		/// <returns>closest FileSyncLib representation of this object</returns>
		public CredentialsLib ToLib() {
			return new CredentialsLib(Login, Password);
		}

		public ActionResult AddToDatabase() {
			try {
				UserManipulator.Add(((UserIdentity)this).ToLib());
			} catch (Exception ex) {
				throw new ActionException("Unable to create new user account.", ActionType.User,
					MemeType.AreYouFuckingKiddingMe, ex);
			}
			return new ActionResult("New account creted.", "Your account was created successfully.",
				ActionType.User);
		}

		public ActionResult UpdateInDatabase(Credentials newCr) {
			//try {
			//    //UserManipulator.Update()
			//} catch (Exception ex) {
			//    throw new ActionException("Unable to update credentials.", ActionType.User,
			//        MemeType.Fuuuuu, ex);
			//}
			//return new ActionResult("Credentials updated.", "",
			//    ActionType.User);
			throw new ActionException("Updating credentials is not properly handled...",
				ActionType.User, MemeType.Fuuuuu);
		}

		private ActionResult DeleteFromDatabase() {
			try {
				//UserManipulator.DelUser(this.ToLib());
			} catch (Exception ex) {
				throw new ActionException("Unable to delete the user account.", ActionType.User,
					MemeType.Fuuuuu, ex);
			}
			return new ActionResult("Acount deleted.", "Your account was deleted successfully.",
				ActionType.User);
		}

		public ActionResult LogIn() {
			try {
				if (!UserManipulator.LoginIn(this.ToLib()))
					throw new Exception("incorrect credentials");
			} catch (Exception ex) {
				throw new ActionException("Login was not successful.", ActionType.User,
					MemeType.AreYouFuckingKiddingMe, ex);
			}
			return new ActionResult("Logged in.", "You are now logged in.", ActionType.User);
		}

		public ActionResult LogOut() {
			throw new ActionException("Logout is not properly handled...", ActionType.User,
				MemeType.Fuuuuu);
		}

	}

}
