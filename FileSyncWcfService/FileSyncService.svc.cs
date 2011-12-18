using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using FileSyncObjects;
using FileSyncWcfService.EntityFramework;

namespace WcfServiceTest {
	/// <summary>
	/// Implementation of FileSync interface, which connects directly to the database.
	/// </summary>
	public class FileSyncService : IFileSyncModel {

		#region User (public)

		public void AddUser(Credentials c, UserContents u) {
			if (UserManipulator.LoginExists(u.Login)) {
				throw new Exception("user already exists");
			} else {
				//TODO: don't use password directly
				User u1 = User.CreateUser(1, u.Login, u.Password);
				u1.user_email = u.Email;
				u1.user_fullname = u.Name;
				u1.user_lastlogin = DateTime.Now;
				using (filesyncEntities context = new filesyncEntities()) {

					context.Users.AddObject(u1);
					context.SaveChanges();

				}
			}
		}

		public void Login(Credentials c) {
			using (filesyncEntities context = new filesyncEntities()) {
				User u1;
				try {
					u1 = (from u in context.Users
						  where c.Equals(u.user_login,u.user_pass)
						  select u).Single();
					UserManipulator.UpdateLastLogin(LoginToId(c.Login));


				} catch {
					throw new Exception("Authorization error.");
				}
			}
		}

		public UserContents GetUser(Credentials c) {
			if (LoginExists(c.Login)) {
				if (Authenticate(c)) {
					using (filesyncEntities context = new filesyncEntities()) {
						int id = LoginToId(c.Login);
						User u1 = (from o in context.Users
								   where o.user_id == id
								   select o).Single();
						UserContents u = new UserContents(u1.user_login, u1.user_pass, u1.user_fullname, u1.user_email);
						u.LastLogin = (DateTime)u1.user_lastlogin;
						u.Id = u1.user_id;
						return u;

					}
				} else {
					throw new Exception("wrong password");
				}
			} else {
				throw new Exception("no such user");
			}
		}

		public void GetMachineList(Credentials c, UserContents u) {
			List<MachineContents> machinelist = new List<MachineContents>();
			u.Id = LoginToId(u.Login);
			using (filesyncEntities context = new filesyncEntities()) {
				List<Machine> ml = (from o in context.Machines
									where o.user_id == u.Id
									select o).ToList();
				foreach (Machine m in ml) {
					MachineContents m1 = new MachineContents(m.machine_name, m.machine_description);
					m1.Id = m.machine_id;
					m1.User = m.user_id;
					machinelist.Add(m1);
				}
				u.Machines = machinelist;
			}
		}

		#endregion

		#region User (private)

		private int LoginToId(string login) {
			if (UserManipulator.LoginExists(login)) {
				using (filesyncEntities context = new filesyncEntities()) {
					User u1 = (from o in context.Users
							   where o.user_login == login
							   select o).Single();
					return u1.user_id;
				}
			} else {
				throw new Exception("no such user");
			}
		}

		private bool LoginExists(string login) {


			using (filesyncEntities context = new filesyncEntities()) {
				User u1;
				try {


					u1 = (from o in context.Users
						  where o.user_login == login
						  select o).Single();


				} catch {
					return false;
				}

				return true;
			}

		}

		private bool Authenticate(Credentials c) {
			using (filesyncEntities context = new filesyncEntities()) {
				User u1;
				try {
					//u1 = (from u in context.Users where u.user_login == c.Login && u.user_pass == c.Pass
					//      select u).Single();
				} catch {
					return false;
				}
				return true;
			}
		}

		#endregion

		#region Machine

		public void AddMachine(Credentials c, MachineContents m) {
			if (MachineNameExists(m.Name)) {
				throw new Exception("machine with given name already exists");
			} else {
				int user_id = LoginToId(c.Login);
				Machine m1 = Machine.CreateMachine(1, user_id, m.Name);
				m1.machine_description = m.Description;

				using (filesyncEntities context = new filesyncEntities()) {
					context.Machines.AddObject(m1);
					context.SaveChanges();
				}
			}
		}

		public void ChangeMachineDetails(Credentials c, MachineContents newM,
				MachineContents oldM) {
			oldM.Id = MachineNameToId(oldM.Name);
			using (filesyncEntities context = new filesyncEntities()) {
				Machine m1 = (from o in context.Machines
							  where o.machine_id == oldM.Id
							  select o).Single();
				m1.machine_name = newM.Name;
				m1.machine_description = newM.Description;
				context.SaveChanges();
			}
		}

		public void GetDirList(Credentials c, MachineContents m) {
			int mach_id = MachineNameToId(m.Name);
			List<DirectoryContents> dirlist = new List<DirectoryContents>();
			using (filesyncEntities context = new filesyncEntities()) {

				foreach (var x in (from md in context.MachineDirs
								   join d in context.Dirs on md.dir_id equals d.dir_id
								   where md.machine_id == mach_id
								   select new { md.dir_realpath, d })) {
					var dir = new DirectoryContents(x.d.dir_name, x.d.dir_description, x.dir_realpath);
					dir.Id = x.d.dir_id;
					dir.Owner = x.d.user_ownerid;
					dirlist.Add(dir);
				}
				m.Directories = dirlist;
			}
		}

		private int MachineNameToId(string name) {
			if (MachineNameExists(name)) {
				using (filesyncEntities context = new filesyncEntities()) {

					Machine m1 = (from o in context.Machines
								  where o.machine_name == name
								  select o).Single();

					return m1.machine_id;
				}
			}

			throw new Exception("no machine with given name found" + name.ToString());
		}

		private static bool MachineNameExists(string name) {
			bool result = true;
			using (filesyncEntities context = new filesyncEntities()) {
				Machine m1;
				try {
					m1 = (from o in context.Machines
						  where o.machine_name == name
						  select o).Single();
				} catch {
					result = false;
				}

			}
			return result;
		}

		#endregion

		#region Directory

		public void AddDirectory(Credentials c, MachineContents m, DirectoryContents d) {
			GetDirList(c, m);
			int NoSuchNameYet = (from o in m.Directories where o.Name == d.Name select o).Count();
			if (NoSuchNameYet != 0) {
				// throw new Exception("directory with given name already exists");
				//no action needed
			} else {
				d.Owner = UserManipulator.LoginToId(c.Login);
				AddDir(d);
				m.Id = MachManipulator.MachineNameToId(m.Name);
				AddMachDir(m, d);
			}
		}

		public void GetFileList(Credentials c, MachineContents m, DirectoryContents d) {
			throw new Exception("not implemented");
		}

		private static void AddDir(DirectoryContents d) {
			int AddedDirId;
			Dir d1 = Dir.CreateDir(1, d.Name, d.Owner);
			d1.dir_description = d.Description;

			using (filesyncEntities context = new filesyncEntities()) {
				context.Dirs.AddObject(d1);
				context.SaveChanges();
				AddedDirId = (from z in context.Dirs select z).ToList().Last().dir_id;
			}

			d.Id = AddedDirId;

		}

		private static void AddMachDir(MachineContents m, DirectoryContents d) {

			MachineDir md1 = MachineDir.CreateMachineDir(m.Id, d.Id, d.Path);
			using (filesyncEntities context = new filesyncEntities()) {
				context.MachineDirs.AddObject(md1);
				context.SaveChanges();

			}

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

		//public void AddDirectory(RemoteCredentialsLib c, RemoteMachineModel m, RemoteDirModel d)
		//{
		//    DirManipulator.AddDirectory(c, m, d);
		//}
		//public void GetDirList(RemoteMachineModel m)
		//{
		//    DirManipulator.GetDirList(m);
		//}
		//public void AddFile(RemoteCredentialsLib c, RemoteMachineModel m, RemoteDirModel d, RemoteFileModel f)
		//{
		//    FileManipulator.AddFile(c, m, d, f);
		//}
		//public void GetFileList(RemoteCredentialsLib c, RemoteMachineModel m, RemoteDirModel d)
		//{
		//    FileManipulator.GetFileList(c, m, d);
		//}
		//public void GetFileContent(RemoteCredentialsLib c, RemoteMachineModel m, RemoteDirModel d, RemoteFileModel f)
		//{
		//    FileManipulator.GetFileContent(c, m, d, f);
		//}
		//public void AddMachdir(RemoteMachdirModel md)
		//{
		//    MachDirManipulator.Add(md);
		//}
		//public void AddMachine(RemoteCredentialsLib c, RemoteMachineModel m)
		//{
		//    MachManipulator.AddMachine(c, m);
		//}
		//public void GetMachineList(RemoteUserModel user)
		//{
		//    MachManipulator.GetMachineList(user);
		//}
		//public void ChangeMachineDetails(RemoteCredentialsLib c, RemoteMachineModel newMachine, RemoteMachineModel oldMachine)
		//{
		//    MachManipulator.ChangeMachineDetails(c, newMachine, oldMachine);
		//}
		//public void AddType(RemoteFileModel f)
		//{
		//    TypeManipulator.AddType(f);
		//}
		//public List<string> GetTypeList()
		//{
		//    return TypeManipulator.GetTypeList();
		//}
		//public RemoteUserModel GetUser(RemoteCredentialsLib c)
		//{
		//    var u = UserManipulator.GetUser(c);
		//    var u1 = new RemoteUserModel();
		//    u1.Login = u.Login;
		//    u1.Pass = u.Pass;
		//    u1.Fullname = u.Fullname;
		//    u1.Email = u.Email;
		//    u1.Machines = u.Machines;
		//    u1.Id = u.Id;
		//    u1.Lastlogin = u.Lastlogin;
		//    return u1;

		//}
		//public void AddUser(RemoteUserModel u)
		//{
		//    UserManipulator.Add(u);
		//}
		//public bool LoginIn(RemoteCredentialsLib c)
		//{
		//    return UserManipulator.LoginIn(c);
		//}

		//public void AddDirectory(CredentialsLib c, MachineModel m, DirModel d)
		//{
		//    DirManipulator.AddDirectory(c, m, d);
		//}
		//public void GetDirList(MachineModel m)
		//{
		//    DirManipulator.GetDirList(m);
		//}
		//public void AddFile(CredentialsLib c, MachineModel m, DirModel d, FileModel f)
		//{
		//    FileManipulator.AddFile(c, m, d, f);
		//}
		//public void GetFileList(CredentialsLib c, MachineModel m, DirModel d)
		//{
		//    FileManipulator.GetFileList(c, m, d);
		//}
		//public void GetFileContent(CredentialsLib c, MachineModel m, DirModel d, FileModel f)
		//{
		//    FileManipulator.GetFileContent(c, m, d, f);
		//}
		//public void AddMachdir(MachdirModel md)
		//{
		//    MachDirManipulator.Add(md);
		//}
		//public void AddMachine(CredentialsLib c, MachineModel m)
		//{
		//    MachManipulator.AddMachine(c, m);
		//}
		//public void GetMachineList(UserModel user)
		//{
		//    MachManipulator.GetMachineList(user);
		//}
		//public  void ChangeMachineDetails(CredentialsLib c, MachineModel newMachine, MachineModel oldMachine)
		//{
		//    MachManipulator.ChangeMachineDetails(c, newMachine, oldMachine);
		//}
		//public void AddType(FileModel f)
		//{
		//    TypeManipulator.AddType(f);
		//}
		//public List<string> GetTypeList()
		//{
		//    return TypeManipulator.GetTypeList();
		//}
		//public UserModel GetUser(CredentialsLib c)
		//{
		//    var u= UserManipulator.GetUser(c);
		//    var u1 = new UserModel();
		//    u1.Login = u.Login;
		//    u1.Pass = u.Pass;
		//    u1.Fullname = u.Fullname;
		//    u1.Email = u.Email;
		//    u1.Machines = u.Machines;
		//    u1.Id = u.Id;
		//    u1.Lastlogin = u.Lastlogin;
		//    return u1;

		//}
		//public void AddUser(UserModel u)
		//{
		//    UserManipulator.Add(u);
		//}
		//public bool LoginIn(CredentialsLib c)
		//{
		//    return UserManipulator.LoginIn(c);
		//}
		//public bool Authenticate(CredentialsLib c)
		//{
		//    return UserManipulator.Authenticate(c);
		//}
	}

}
