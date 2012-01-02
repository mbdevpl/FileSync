using System;

using FileSyncObjects;

namespace FileSyncGui {
	public class FileSyncConnection : IFileSyncModel {

		#region Tests

		public bool TestWCF() {
			bool result = false;
			using (var cl = new Ref.FileSyncModelClient()) {
				result = cl.TestWCF();
			}
			return result;
		}

		public bool TestEF() {
			bool result = false;
			using (var cl = new Ref.FileSyncModelClient()) {
				result = cl.TestEF();
			}
			return result;
		}

		#endregion

		#region User (connection)

		public bool AddUser(UserContents u) {
			var cl = new Ref.FileSyncModelClient();
			try {
				bool result = false;
				result = cl.AddUser(u);
				cl.Close();
				return result;
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Unable to create new user account.", 
					ActionType.User, ex);
			}
		}

		public bool Login(Credentials c) {
			var cl = new Ref.FileSyncModelClient();
			try {
				bool result = false;
				result = cl.Login(c);
				cl.Close();
				return result;
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Login process did not end in a very happy manner. "
					+ "In fact, it ended in a very unpleasant way.",
					ActionType.User, ex, MemeType.AreYouFuckingKiddingMe);
			}
		}

		public UserIdentity GetUser(Credentials c) {
			var cl = new Ref.FileSyncModelClient();
			try {
				UserIdentity u = null;
				u = cl.GetUser(c);
				cl.Close();
				return u;
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Unable to get user details for the given credentials.",
					ActionType.User, ex);
			}
		}

		public UserContents GetUserWithMachines(Credentials c) {
			var cl = new Ref.FileSyncModelClient();
			try {
				UserContents u = null;
				u = cl.GetUserWithMachines(c);
				cl.Close();
				return u;
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Unable to get machines for a given user.",
					ActionType.User, ex);
			}
		}

		public bool DelUser(Credentials c) {
			var cl = new Ref.FileSyncModelClient();
			try {
				bool result = false;
				result = cl.DelUser(c);
				cl.Close();
				return result;
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Unable to delete the user account.", 
					ActionType.User, ex);
			}
		}

		#endregion

		#region Machine (connection)

		public bool AddMachine(Credentials c, MachineContents m) {
			var cl = new Ref.FileSyncModelClient();
			try {
				if (c == null)
					throw new ArgumentNullException("cr", "user credentials must be provided");
				if (m == null)
					throw new ArgumentNullException("m", "machine contents were null");

				bool result = false;
				result = cl.AddMachine(c, m);
				cl.Close();
				return result;
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Failed to create a new machine.", ActionType.Machine,
					MemeType.Fuuuuu, ex);
			}
		}

		public bool ChangeMachineDetails(Credentials c, MachineContents newM,
				MachineContents oldM) {
			var cl = new Ref.FileSyncModelClient();
			try {
				if (c == null)
					throw new ArgumentNullException("c", "user credentials must be provided");
				if (newM == null)
					throw new ArgumentNullException("newM", "new machine identity must be provided");
				if (oldM == null)
					throw new ArgumentNullException("oldM", "old machine identity must be provided");

				bool result = false;
				result = cl.ChangeMachineDetails(c, newM, oldM);
				cl.Close();
				return result;
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Error while updating machine details.",
					ActionType.Machine, MemeType.Fuuuuu, ex);
			}
		}

		public MachineContents GetMachineWithDirs(Credentials c, MachineIdentity mid) {
			var cl = new Ref.FileSyncModelClient();
			try {
				if (c == null)
					throw new ArgumentNullException("c", "user credentials must be provided");
				if (mid == null)
					throw new ArgumentNullException("mid", "machine identity was null");


				MachineContents newM = new MachineContents(mid);
				newM.Directories = cl.GetDirList(c, newM);
				cl.Close();
				return newM;

				//MachineContents m = null;
				//m = cl.GetMachineWithDirs(c, mid);
				//cl.Close();
				//return m;
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Unable to get list of directories belonging "
					+ "to the machine.", ActionType.Machine, ex);
			}
		}

		public bool DelMachine(Credentials c, MachineIdentity mid) {
			var cl = new Ref.FileSyncModelClient();
			cl.Abort();
			throw new NotImplementedException();
		}


		#endregion

		#region Directory (connection)

		public bool AddDirectory(Credentials c, MachineContents m, DirectoryContents d) {
			var cl = new Ref.FileSyncModelClient();
			try {
				bool result = false;
				result = cl.AddDirectory(c, m, d);
				cl.Close();
				return result;
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Unable to create a new directory in the database.",
					ActionType.Directory, MemeType.Fuuuuu, ex);
			}
		}

		public DirectoryContents GetDirectoryWithFiles(Credentials c, MachineContents m,
				DirectoryIdentity did) {
			var cl = new Ref.FileSyncModelClient();
			try {
				DirectoryContents d = null;
				d = cl.GetDirectoryWithFiles(c, m, did);
				cl.Close();
				return d;
			} catch (ActionException ex) {
				cl.Abort();
				throw new ActionException("Unable to download directory contents.",
					ActionType.Directory, MemeType.Fuuuuu, ex);
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Error while downloading directory contents.",
					ActionType.Directory, MemeType.Fuuuuu, ex);
			}
		}

		public bool DelDirectory(Credentials c, MachineIdentity mid, DirectoryIdentity did) {
			var cl = new Ref.FileSyncModelClient();
			cl.Abort();
			throw new NotImplementedException();
		}

		#endregion

		#region File (connection)

		public bool AddFile(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			var cl = new Ref.FileSyncModelClient();
			try {
				bool result = false;
				result = cl.AddFile(c, m, d, f);
				cl.Close();
				return result;
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Error occurred while file was uploaded.",
					ActionType.File, MemeType.Fuuuuu, ex);
			}
		}

		public FileContents GetFileWithContent(Credentials c, MachineContents m,
				DirectoryContents d, FileIdentity fid) {
			var cl = new Ref.FileSyncModelClient();
			try {
				FileContents f = null;
				f = cl.GetFileWithContent(c, m, d, f);
				cl.Close();
				return f;
			} catch (Exception ex) {
				cl.Abort();
				throw new ActionException("Error while downloading file contents from database.",
					ActionType.File, MemeType.Fuuuuu, ex);
			}
		}

		public bool DelFile(Credentials c, MachineIdentity mid, DirectoryIdentity did,
				FileIdentity f) {
			var cl = new Ref.FileSyncModelClient();
			cl.Abort();
			throw new NotImplementedException();
		}

		#endregion



		public System.Collections.Generic.List<DirectoryContents> GetDirList(Credentials c, MachineContents m) {
			throw new NotImplementedException();
		}
	}
}
