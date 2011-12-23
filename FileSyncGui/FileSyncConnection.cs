using System;

using FileSyncObjects;

namespace FileSyncGui {
	public class FileSyncConnection : IFileSyncModel {

		#region Tests

		public bool TestWCF() {
			bool result = false;
			using (var cl = new Ref.FileSyncModelClient()) {
				//result = cl.TestWCF();
			}
			return result;
		}

		public bool TestEF() {
			bool result = false;
			using (var cl = new Ref.FileSyncModelClient()) {
				//result = cl.TestEF();
			}
			return result;
		}

		#endregion

		#region User (connection)

		public bool AddUser(UserContents u) {
			try {
				bool result = false;
				using (var cl = new Ref.FileSyncModelClient()) {
					//result = cl.AddUser(u);
				}
				return result;
			} catch (Exception ex) {
				throw new ActionException("Unable to create new user account.", 
					ActionType.User, ex);
			}
		}

		public bool Login(Credentials c) {
			try {
				bool result = false;
				using (var cl = new Ref.FileSyncModelClient()) {
					//result = cl.Login(c);
				}
				return result;
			} catch (Exception ex) {
				throw new ActionException("Login process did not end in a very happy manner. "
					+ "In fact, it ended in a very unpleasant way.",
					ActionType.User, ex, MemeType.AreYouFuckingKiddingMe);
			}
		}

		public UserIdentity GetUser(Credentials c) {
			try {
				UserIdentity u = null;
				using (var cl = new Ref.FileSyncModelClient()) {
					u = cl.GetUser(c);
				}
				return u;
			} catch (Exception ex) {
				throw new ActionException("Unable to get user details for the given credentials.",
					ActionType.User, ex);
			}
		}

		public UserContents GetUserWithMachines(Credentials c) {
			try {
				UserContents u = null;
				using (var cl = new Ref.FileSyncModelClient()) {
					//u = cl.GetUserWithMachines(c);
				}
				return u;
			} catch (Exception ex) {
				throw new ActionException("Unable to get machines for a given user.",
					ActionType.User, ex);
			}
		}

		public bool DelUser(Credentials c) {
			try {
				bool result = false;
				using (var cl = new Ref.FileSyncModelClient()) {
					//result = cl.DelUser(c);
				}
				return result;
			} catch (Exception ex) {
				throw new ActionException("Unable to delete the user account.", 
					ActionType.User, ex);
			}
		}

		#endregion

		#region Machine (connection)

		public bool AddMachine(Credentials c, MachineContents m) {
			try {
				if (c == null)
					throw new ArgumentNullException("cr", "user credentials must be provided");
				if (m == null)
					throw new ArgumentNullException("m", "machine contents were null");

				bool result = false;
				using (var cl = new Ref.FileSyncModelClient()) {
					//result = cl.AddMachine(c, m);
				}
				return result;
			} catch (Exception ex) {
				throw new ActionException("Failed to create a new machine.", ActionType.Machine,
					MemeType.Fuuuuu, ex);
			}
		}

		public bool ChangeMachineDetails(Credentials c, MachineContents newM,
				MachineContents oldM) {
			try {
				if (c == null)
					throw new ArgumentNullException("c", "user credentials must be provided");
				if (newM == null)
					throw new ArgumentNullException("newM", "new machine identity must be provided");
				if (oldM == null)
					throw new ArgumentNullException("oldM", "old machine identity must be provided");

				bool result = false;
				using (var cl = new Ref.FileSyncModelClient()) {
					//result = cl.ChangeMachineDetails(c, newM, oldM);
				}
				return result;
			} catch (Exception ex) {
				throw new ActionException("Error while updating machine details.",
					ActionType.Machine, MemeType.Fuuuuu, ex);
			}
		}

		public MachineContents GetMachineWithDirs(Credentials c, MachineIdentity mid) {
			try {
				if (c == null)
					throw new ArgumentNullException("c", "user credentials must be provided");
				if (mid == null)
					throw new ArgumentNullException("mid", "machine identity was null");

				MachineContents m = null;
				using (var cl = new Ref.FileSyncModelClient()) {
					//m = cl.GetMachineWithDirs(c, mid);
				}
				return m;
			} catch (Exception ex) {
				throw new ActionException("Unable to get list of directories belonging "
					+ "to the machine.", ActionType.Machine, ex);
			}
		}

		public bool DelMachine(Credentials c, MachineIdentity mid) {
			throw new NotImplementedException();
		}


		#endregion

		#region Directory (connection)

		public bool AddDirectory(Credentials c, MachineContents m, DirectoryContents d) {
			try {
				bool result = false;
				using (var cl = new Ref.FileSyncModelClient()) {
					//result = cl.AddDirectory(c, m, d);
				}
				return result;
			} catch (Exception ex) {
				throw new ActionException("Unable to create a new directory in the database.",
					ActionType.Directory, MemeType.Fuuuuu, ex);
			}
		}

		public DirectoryContents GetDirectoryWithFiles(Credentials c, MachineContents m, 
				DirectoryIdentity did) {
			try {
				DirectoryContents d = null;
				using (var cl = new Ref.FileSyncModelClient()) {
					//d = cl.GetDirectoryWithFiles(c, m, d);
				}
				return d;
			} catch (ActionException ex) {
				throw new ActionException("Unable to download directory contents.",
					ActionType.Directory, MemeType.Fuuuuu, ex);
			} catch (Exception ex) {
				throw new ActionException("Error while downloading directory contents.",
					ActionType.Directory, MemeType.Fuuuuu, ex);
			}
		}

		public bool DelDirectory(Credentials c, MachineIdentity mid, DirectoryIdentity did) {
			throw new NotImplementedException();
		}

		#endregion

		#region File (connection)

		public bool AddFile(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			try {
				bool result = false;
				using (var cl = new Ref.FileSyncModelClient()) {
					//result = cl.AddFile(c, m, d, f);
				}
				return result;
			} catch (Exception ex) {
				throw new ActionException("Error occurred while file was uploaded.",
					ActionType.File, MemeType.Fuuuuu, ex);
			}
		}

		public FileContents GetFileWithContent(Credentials c, MachineContents m, 
				DirectoryContents d, FileIdentity fid) {
			try {
				FileContents f = null;
				using (var cl = new Ref.FileSyncModelClient()) {
					//f = cl.GetFileWithContent(c, m, d, f);
				}
				return f;
			} catch (Exception ex) {
				throw new ActionException("Error while downloading file contents from database.",
					ActionType.File, MemeType.Fuuuuu, ex);
			}
		}

		public bool DelFile(Credentials c, MachineIdentity mid, DirectoryIdentity did, 
				FileIdentity f) {
			throw new NotImplementedException();
		}

		#endregion

	}
}
