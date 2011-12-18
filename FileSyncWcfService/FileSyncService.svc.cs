using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using FileSyncObjects;
using FileSyncWcfService.EntityFramework;

namespace WcfServiceTest
{
    /// <summary>
    /// Implementation of FileSync interface, which connects directly to the database.
    /// </summary>
    public class FileSyncService : IFileSyncModel {

		#region User

		public void AddUser(Credentials c, UserContents u) {
			if (UserManipulator.LoginExists(u.Login)) {
				throw new Exception("user already exists");
			} else {
				User u1 = User.CreateUser(1, u.Login, u.Pass);
				u1.user_email = u.Email;
				u1.user_fullname = u.Fullname;
				u1.user_lastlogin = DateTime.Now;
				using (filesyncEntities context = new filesyncEntities()) {

					context.Users.AddObject(u1);
					context.SaveChanges();

				}
			}
		}

		public void Login(Credentials c) {
			throw new Exception("not implemented");
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
			throw new Exception("not implemented");
		}

		#endregion

		#region Machine

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
