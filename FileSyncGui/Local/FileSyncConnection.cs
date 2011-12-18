using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FileSyncGui.Ref;
using System.IO;

namespace FileSyncGui.Local {
	public class FileSyncConnection : IFileSyncModel {

		/// <summary>
		/// Used in directory, and file operations.
		/// </summary>
		internal static string EmptyLocalPath = "\\";

		#region User (connection)

		public void AddUser(Credentials c, UserContents u) {
			try {
				using (FileSyncModelClient cl = new FileSyncModelClient()) {
					cl.AddUser(c, u);
				}
			} catch (Exception ex) {
				throw new ActionException("Unable to create new user account.", ActionType.User,
					MemeType.Fuuuuu, ex);
			}
		}

		public void Login(Credentials c) {
			try {
				using (FileSyncModelClient cl = new FileSyncModelClient()) {
					cl.Login(c);
				}
			} catch (Exception ex) {
				throw new ActionException("Login was not successful.", ActionType.User,
					MemeType.AreYouFuckingKiddingMe, ex);
			}
		}

		//public void Logout() {
		//    throw new ActionException("Logout is not properly handled...", ActionType.User,
		//        MemeType.Fuuuuu);
		//}

		public UserContents GetUser(Credentials c) {
			try {
				UserContents u;
                using (FileSyncModelClient cl = new FileSyncModelClient()) {
					u = cl.GetUser(c);
				}
				return u;
			} catch (Exception ex) {
				throw new ActionException("Unable to get user details for the given credentials.",
					ActionType.User, MemeType.Fuuuuu, ex);
			}
		}

		public void GetMachineList(Credentials c, UserContents u) {
			throw new Exception("not implemented");
		}

		//public void UpdateUser(Credentials c, UserContents newU) {
		//    //try {
		//    //    //UserManipulator.Update()
		//    //} catch (Exception ex) {
		//    //    throw new ActionException("Unable to update credentials.", ActionType.User,
		//    //        MemeType.Fuuuuu, ex);
		//    //}
		//    //return new ActionResult("Credentials updated.", "",
		//    //    ActionType.User);
		//    throw new ActionException("Updating credentials is not properly handled...",
		//        ActionType.User, MemeType.Fuuuuu);
		//}

		public void DelUser(Credentials c) {
			try {
				using (FileSyncModelClient cl = new FileSyncModelClient()) {
					cl.DelUser(c);
				}
			} catch (Exception ex) {
				throw new ActionException("Unable to delete the user account.", ActionType.User,
					MemeType.Fuuuuu, ex);
			}
		}

		#endregion

		#region User (local)

		public UserContents GetUserWithMachines(Credentials c) {
			try {
				var u = this.GetUser(c);
				this.GetMachineList(c, u);
				return u;
			} catch (ActionException ex) {
				throw new ActionException("Error occured while getting user and his machines "
					+ "at the same time.", ex.Type, ex.Image, ex);
			} catch (Exception ex) {
				throw new ActionException("Unable to get list of machines for the user "
					+ "with the given credentials.", ActionType.User, MemeType.Fuuuuu, ex);
			}
		}

		#endregion

		#region Machine (connection)

		public void AddMachine(Credentials c, MachineContents m) {
			throw new Exception("not implemented");
		}

		public void ChangeMachineDetails(Credentials c, MachineContents newM,
				MachineContents oldM) {
			throw new Exception("not implemented");
		}

		public void GetDirList(Credentials c, MachineContents m) {
			throw new Exception("not implemented");
		}

		#endregion

		#region Machine (local)

		public void AddLocalDirs(MachineContents m, bool addLocalFiles = true) {

			if(addLocalFiles) {
				foreach (DirectoryContents d in m.Directories) {
					if (d.LocalPath == null || d.LocalPath.Equals(EmptyLocalPath))
						return;

					string[] filePaths = Directory.GetFiles(d.LocalPath);

					if (filePaths == null || filePaths.Length == 0)
						return;

					if (d.Files == null)
						d.Files = new List<FileContents>();

					foreach (string path in filePaths) {
						d.Files.Add(GetLocalFileContent(path));
					}
				}
			}
		}

		#endregion

		#region Directory

		public void AddDirectory(Credentials c, MachineContents m, DirectoryContents d) {
			throw new Exception("not implemented");
		}

		public void GetFileList(Credentials c, MachineContents m, DirectoryContents d) {
			throw new Exception("not implemented");
		}

		#endregion

		#region File

		public void AddFile(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			throw new Exception("not implemented");
		}

		public void GetFileContent(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			throw new Exception("not implemented");
		}

		#endregion

	}
}
