using System;
using System.Collections.Generic;

using FileSyncGui.GuiAbstracts;
using FileSyncLib;

namespace FileSyncGui.GuiObjects {

	/// <summary>
	/// Stores contents of a single user account.
	/// </summary>
	public class UserContents : UserIdentity {

		private List<MachineContents> machines;
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

		public UserContents(Credentials cr, bool fromDatabase)
			: this(fromDatabase ? FromDatabase(cr) : (UserContents)cr) {
			//nothing needed here
		}

		public UserContents(UserContents uc)
			: this((UserIdentity)uc, uc.Machines) {
			//nothing needed here
		}

		private static UserContents FromDatabase(Credentials cr) {
			try {
				UserModel u = UserManipulator.GetUser(cr.ToLib());
				MachManipulator.GetMachineList(u);
				List<MachineContents> machines = null;
                if (u.Machines.Count > 0) {
					machines = new List<MachineContents>();
					foreach (MachineModel m in u.Machines)
						machines.Add(new MachineContents(m));
				}
				return new UserContents(new UserIdentity(u), machines);
			} catch (Exception ex) {
				throw new ActionException("Unable to get list of machines for the user "
					+ "with the given credentials.", ActionType.User, MemeType.Fuuuuu, ex);
			}
		}

		/// <summary>
		/// Converts this object to its FileSyncLib representation. Convertion may not be lossless, 
		/// but will be as perfect as possible.
		/// </summary>
		/// <returns>closest FileSyncLib representation of this object</returns>
		public new UserModel ToLib() {
			UserModel user = base.ToLib();
			if (Machines == null)
				return user;

			user.Machines = new List<MachineModel>();
			foreach (MachineContents mc in Machines)
				user.Machines.Add(mc.ToLib());
			return user;
		}

	}

}
