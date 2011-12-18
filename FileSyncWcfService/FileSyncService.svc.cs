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
			if (LoginExists(u.Login)) {
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
						  where c.Equals(u.user_login, u.user_pass)
						  select u).Single();
					UpdateLastLogin(LoginToId(c.Login));


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

		public void DelUser(Credentials c) {
			UserContents u = GetUser(c);
			using (filesyncEntities context = new filesyncEntities()) {
				try {
					int id = LoginToId(u.Login);
					var u1 = (from o in context.Users
							  where o.user_id == id
							  select o).Single();
					context.Users.DeleteObject(u1);
					context.SaveChanges();
				} catch {
					throw new Exception("no such user");
				}
			}
		}

		#endregion

		#region User (private)

		private int LoginToId(string login) {
			if (LoginExists(login)) {
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

		private void UpdateLastLogin(int id) {
			using (filesyncEntities context = new filesyncEntities()) {
				try {
					var u1 = (from o in context.Users
							  where o.user_id == id
							  select o).Single();
					u1.user_lastlogin = DateTime.Now;
					context.SaveChanges();
				} catch {
					throw new Exception("no such user");
				}
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
					u1 = (from u in context.Users
						  where c.Equals(u.user_login, u.user_pass)
						  select u).Single();
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
				d.Owner = LoginToId(c.Login);
				AddDir(d);
				m.Id = MachineNameToId(m.Name);
				AddMachDir(m, d);
			}
		}

		public void GetFileList(Credentials c, MachineContents m, DirectoryContents d) {
			List<FileContents> filelist = new List<FileContents>();
			GetDirList(c, m);
			d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
			using (filesyncEntities context = new filesyncEntities()) {
				foreach (var x in (from f in context.Files
								   join t in context.Types on f.type_id equals t.type_id
								   where f.dir_id == d.Id
								   select new { f, t.type_name })) {
					FileIdentity file = new FileIdentity(x.f.file_name, x.f.file_modified,
						x.f.file_uploaded, FileType.PlainText, x.f.file_size, x.f.file_hash);
					file.Content = x.f.content_id;
					file.Id = x.f.file_id;
					filelist.Add(new FileContents(file));
				}
			}
			d.Files = filelist;
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
			MachineDir md1 = MachineDir.CreateMachineDir(m.Id, d.Id, d.LocalPath);
			using (filesyncEntities context = new filesyncEntities()) {
				context.MachineDirs.AddObject(md1);
				context.SaveChanges();
			}
		}

		#endregion

		#region File

		public void AddFile(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			GetDirList(c, m);
			f.Dir = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
			if (!CheckFileExistence(c, m, d, f)) {
				AddFileContent(f);
				//TypeManipulator.TypeToId(f);
				File f1 = File.CreateFile(1, f.Dir, 1, f.Content, f.Name, f.Size, f.Hash,
					f.Uploaded, f.Modified);
				using (filesyncEntities context = new filesyncEntities()) {
					context.Files.AddObject(f1);
					context.SaveChanges();
				}
			} else {
				GetFileId(c, m, d, f);
				GetFileContentId(c, m, d, f);
				UpdateFileContent(f);
				//TypeManipulator.TypeToId(f);

				using (filesyncEntities context = new filesyncEntities()) {
					File f1 = (from o in context.Files where o.file_id == f.Id select o).Single();
					f1.file_hash = f.Hash;
					f1.file_modified = f.Modified;
					f1.file_size = f.Size;
					f1.file_uploaded = f.Uploaded;

					context.SaveChanges();
				}
			}
		}

		public void GetFileContent(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			GetFileContentId(c, m, d, f);
			using (filesyncEntities context = new filesyncEntities()) {
				Content c1 = (from o in context.Contents
							  where o.content_id == f.Content
							  select o).Single();
				f.Data = c1.content_data;
			}
		}

		#endregion

		#region File (private)

		private void AddFileContent(FileContents f) {
			int AddedContentId;
			Content f1 = Content.CreateContent(1, f.Data);
			using (filesyncEntities context = new filesyncEntities()) {

				context.Contents.AddObject(f1);
				context.SaveChanges();
				AddedContentId = (from c in context.Contents select c).ToList().Last().content_id;

			}
			f.Content = AddedContentId;
		}

		private static void UpdateFileContent(FileContents f) {
			using (filesyncEntities context = new filesyncEntities()) {
				Content c1 = (from o in context.Contents
							  where o.content_id == f.Content
							  select o).Single();
				c1.content_data = f.Data;
				context.SaveChanges();
			}
		}

		private void GetFileContentId(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			GetDirList(c, m);
			d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
			using (filesyncEntities context = new filesyncEntities()) {
				int content_id = (from o in context.Files
								  where (o.file_name == f.Name) && (o.dir_id == d.Id)
								  select o.content_id).Single();
				f.Content = content_id;
			}
		}

		private void GetFileId(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			GetDirList(c, m);
			d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
			using (filesyncEntities context = new filesyncEntities()) {

				int file_id = (from o in context.Files
							   where (o.file_name == f.Name) && (o.dir_id == d.Id)
							   select o.file_id).Single();
				f.Id = file_id;
			}
		}
		private bool CheckFileExistence(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			GetDirList(c, m);
			d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
			try {
				using (filesyncEntities context = new filesyncEntities()) {
					(from o in context.Files
					 where (o.file_name == f.Name) && (o.dir_id == d.Id)
					 select o.file_id).Single();
				}
			} catch {
				return false;
			}
			return true;
		}

		#endregion

	}

}
