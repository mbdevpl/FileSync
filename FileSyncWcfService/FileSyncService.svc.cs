using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using FileSyncObjects;

namespace FileSyncWcfService {
	/// <summary>
	/// Implementation of FileSync interface, which connects directly to the database.
	/// </summary>
	public class FileSyncService : IFileSyncModel {

		#region Tests

		public bool TestWCF() {
			return true;
		}

		public bool TestEF() {
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				context.Users.Where(u => u.user_login == "adm").SingleOrDefault();
				context.Users.Where(u => u.user_login == "admin").SingleOrDefault();
				context.Users.Where(u => u.user_login == "administrator").SingleOrDefault();
			}
			return true;
		}

		#endregion

		#region User (public)

		public bool AddUser(UserContents u) {
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				if (LoginExists(context, u.Login)) {
					//throw new Exception("user already exists");
                    return false;
				} else {
					//TODO: don't use password directly
					User u1 = User.CreateUser(1, u.Login, u.Password);
					u1.user_email = u.Email;
					u1.user_fullname = u.Name;
					u1.user_lastlogin = DateTime.Now;
					context.Users.AddObject(u1);
					context.SaveChanges();
				}
			}
            return true;
		}

		public bool Login(Credentials c) {
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				//try {
					//User u1 = (from u in context.Users
					//           where c.Equals(u.user_login, u.user_pass)
					//           select u).Single();
					User u1 = context.Users.Where(u => u.user_login == c.Login).SingleOrDefault();
                    

					if (u1 == null) {
                        return false;
					}
					//(from u in context.Users
					//       where c.Login == u.user_login && c.Password == u.user_pass
					//       select u).Single();
                    if (!c.Equals(u1.user_login, u1.user_pass))
                        return false;

					UpdateLastLogin(context, LoginToId(context, c.Login));

				//} catch (Exception ex) {
					//if (ex.GetType().Equals(typeof(Exception)))
					//    throw ex;
					//throw new Exception("wrong credentials", ex);
				//}
                    return  true;
			}
		}

		public UserIdentity GetUser(Credentials c) {
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				if (LoginExists(context, c.Login)) {
					if (Authenticate(context, c)) {
						int id = LoginToId(context, c.Login);
						User u1 = (from o in context.Users
								   where o.user_id == id
								   select o).SingleOrDefault();
						UserIdentity u = new UserContents(u1.user_login, u1.user_pass, u1.user_fullname, u1.user_email);
						u.LastLogin = (DateTime)u1.user_lastlogin;
						u.Id = u1.user_id;
						return u;

					} else {
						//throw new Exception("wrong password");
                        return null;
					}
				} else {
					//throw new Exception("no such user");
                    return null;
				}
			}
		}

		public UserContents GetUserWithMachines(Credentials c) {
			UserContents u =(UserContents) GetUser(c);
			List<Machine> ml;
			List<MachineContents> machinelist;
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {

				if (!Authenticate(context, c))
					return null;
				//throw new Exception("unauthorized access detected");

				machinelist = new List<MachineContents>();
				u.Id = LoginToId(context, u.Login);
				ml = (from o in context.Machines
					  where o.user_id == u.Id
					  select o).ToList();
			}
			foreach (Machine m in ml) {
				MachineContents m1 = new MachineContents(m.machine_name, m.machine_description);
				m1.Id = m.machine_id;
				m1.User = m.user_id;
				machinelist.Add(m1);
			}
			u.Machines = machinelist;
			return u;
		}

		public bool DelUser(Credentials c) {
			UserContents u =(UserContents) GetUser(c);
			bool result = false;
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				if (!Authenticate(context, c))
					return result;

				int id = LoginToId(context, u.Login);
				var u1 = (from o in context.Users
						  where o.user_id == id
						  select o).SingleOrDefault();
				if (u1 != null) {
					context.Users.DeleteObject(u1);
					context.SaveChanges();
					result = true;
				}
			}
			return result;
		}

		#endregion

		#region User (private)

		private bool LoginExists(filesyncEntitiesNew context, string login) {
			User u1 = (from o in context.Users
					   where o.user_login == login
					   select o).SingleOrDefault();

			if (u1 == null)
				return false;

			return true;
		}

		private int LoginToId(filesyncEntitiesNew context, string login) {
			if (!LoginExists(context, login))
				return -1;

			User u1 = (from o in context.Users
					   where o.user_login == login
					   select o).SingleOrDefault();

			if (u1 == null || u1.user_id < 0)
				return -1;

			return u1.user_id;
		}

		private void UpdateLastLogin(filesyncEntitiesNew context, int id) {
			//try {
				var u1 = (from o in context.Users
						  where o.user_id == id
						  select o).SingleOrDefault();
                if (u1 != null)
                {
                    u1.user_lastlogin = DateTime.Now;
                    context.SaveChanges();
                }
			//} catch (Exception ex) {
			//	throw new Exception("no such user", ex);
			//}
		}

		private bool Authenticate(filesyncEntitiesNew context, Credentials c) {
			try {
				//User u1 = (from u in context.Users
				//           where c.Equals(u.user_login, u.user_pass)
				//           select u).Single();
				User u1 = (from u in context.Users
						   where c.Login == u.user_login && c.Password == u.user_pass
						   select u).Single();
			} catch {
				return false;
			}
			return true;
		}

		#endregion

		#region Machine

		public bool AddMachine(Credentials c, MachineContents m) {
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				if (MachineNameExists(context, m.Name)) {
					//throw new Exception("machine with given name already exists");
                    return false;
				} else {
					int user_id = LoginToId(context, c.Login);
					Machine m1 = Machine.CreateMachine(1, user_id, m.Name);
					m1.machine_description = m.Description;

					context.Machines.AddObject(m1);
					context.SaveChanges();
				}
			}
            return true;
		}

		public bool ChangeMachineDetails(Credentials c, MachineContents newM,
				MachineContents oldM) {
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				oldM.Id = MachineNameToId(context, oldM.Name);
				Machine m1 = (from o in context.Machines
							  where o.machine_id == oldM.Id
							  select o).SingleOrDefault();
                if (m1 == null)
                    return false;
                m1.machine_name = newM.Name;
				m1.machine_description = newM.Description;
				context.SaveChanges();
                return true;
			}
		}

		public List<DirectoryContents> GetDirList(Credentials c, MachineContents m) {
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				int mach_id = MachineNameToId(context, m.Name);
                if (mach_id == -1)
                    return null;
				List<DirectoryContents> dirlist = new List<DirectoryContents>();

				foreach (var x in (from md in context.MachineDirs
								   join d in context.Dirs on md.dir_id equals d.dir_id
								   where md.machine_id == mach_id
								   select new { md.dir_realpath, d })) {
					var dir = new DirectoryContents(x.d.dir_name, x.d.dir_description, x.dir_realpath);
					dir.Id = x.d.dir_id;
					dir.Owner = x.d.user_ownerid;
					dirlist.Add(dir);
				}
				//m.Directories = dirlist;
                return dirlist;
			}
		}
        public MachineContents GetMachineWithDirs(Credentials c, MachineIdentity m)
        {
            MachineContents mc = new MachineContents(m);
            
   
            using (filesyncEntitiesNew context = new filesyncEntitiesNew())
            {
                int mach_id = MachineNameToId(context, m.Name);
                if (mach_id == -1)
                    return null;
                mc.Id = mach_id;
                List<DirectoryContents> dirlist = new List<DirectoryContents>();

                foreach (var x in (from md in context.MachineDirs
                                   join d in context.Dirs on md.dir_id equals d.dir_id
                                   where md.machine_id == mach_id
                                   select new { md.dir_realpath, d }))
                {
                    var dir = new DirectoryContents(x.d.dir_name, x.d.dir_description, x.dir_realpath);
                    dir.Id = x.d.dir_id;
                    dir.Owner = x.d.user_ownerid;
                    dirlist.Add(dir);
                }
                mc.Directories = dirlist;
                return mc;
            }
        }
		private int MachineNameToId(filesyncEntitiesNew context, string name) {
			if (MachineNameExists(context, name)) {
				Machine m1 = (from o in context.Machines
							  where o.machine_name == name
							  select o).Single();

				return m1.machine_id;
			}

			//throw new Exception("no machine with given name found" + name.ToString());
            return -1;
		}

		private static bool MachineNameExists(filesyncEntitiesNew context, string name) {
			Machine m1;
			try {
				m1 = (from o in context.Machines
					  where o.machine_name == name
					  select o).Single();
			} catch {
				return false;
			}

			return true;
		}
        public bool DelMachine(Credentials c, MachineIdentity m) {
            throw new NotImplementedException();
        }
		#endregion

		#region Directory

		public bool AddDirectory(Credentials c, MachineContents m, DirectoryContents d) {
			m.Directories=GetDirList(c, m);
			int NoSuchNameYet = (from o in m.Directories where o.Name == d.Name select o).Count();
			if (NoSuchNameYet != 0) {
				// throw new Exception("directory with given name already exists");
				//no action needed
			} else {
				using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
					d.Owner = LoginToId(context, c.Login);
				}
				AddDir(d);
				using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
					m.Id = MachineNameToId(context, m.Name);
				}
				AddMachDir(m, d);
			}
            return true;
		}

		public List<FileContents> GetFileList(Credentials c, MachineContents m, DirectoryContents d) {
			List<FileContents> filelist = new List<FileContents>();
			m.Directories=GetDirList(c, m);
			d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
           
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
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
			//d.Files = filelist;
            return filelist;
		}
        public DirectoryContents GetDirectoryWithFiles(Credentials c, MachineContents m, DirectoryIdentity d)
        {
            List<FileContents> filelist = new List<FileContents>();
            m.Directories = GetDirList(c, m);
            d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
            var dc = new DirectoryContents(d);
            using (filesyncEntitiesNew context = new filesyncEntitiesNew())
            {
                foreach (var x in (from f in context.Files
                                   join t in context.Types on f.type_id equals t.type_id
                                   where f.dir_id == d.Id
                                   select new { f, t.type_name }))
                {
                    FileIdentity file = new FileIdentity(x.f.file_name, x.f.file_modified,
                        x.f.file_uploaded, FileType.PlainText, x.f.file_size, x.f.file_hash);
                    file.Content = x.f.content_id;
                    file.Id = x.f.file_id;
                    filelist.Add(new FileContents(file));
                }
            }
            dc.Files = filelist;
            return dc;
        }
		private static void AddDir(DirectoryContents d) {
			int AddedDirId;
			Dir d1 = Dir.CreateDir(1, d.Name, d.Owner);
			d1.dir_description = d.Description;

			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				context.Dirs.AddObject(d1);
				context.SaveChanges();
				AddedDirId = (from z in context.Dirs select z).ToList().Last().dir_id;
			}

			d.Id = AddedDirId;

		}

		private static void AddMachDir(MachineContents m, DirectoryContents d) {
			MachineDir md1 = MachineDir.CreateMachineDir(m.Id, d.Id, d.LocalPath);
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				context.MachineDirs.AddObject(md1);
				context.SaveChanges();
			}
		}
        public bool DelDirectory (Credentials c, MachineIdentity m, DirectoryIdentity d)
        {
            throw new NotImplementedException();
        }

		#endregion

		#region File (public)

		public bool AddFile(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			GetDirList(c, m);
			f.Dir = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
        
			if (!CheckFileExistence(c, m, d, f)) {
				AddFileContent(f);
				//TypeManipulator.TypeToId(f);
				File f1 = File.CreateFile(1, f.Dir, 1, f.Content, f.Name, f.Size, f.Hash,
					f.Uploaded, f.Modified);
				using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
					context.Files.AddObject(f1);
					context.SaveChanges();
				}
			} else {
				GetFileId(c, m, d, f);
				GetFileContentId(c, m, d, f);
				UpdateFileContent(f);
				//TypeManipulator.TypeToId(f);

				using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
					File f1 = (from o in context.Files where o.file_id == f.Id select o).Single();
					f1.file_hash = f.Hash;
					f1.file_modified = f.Modified;
					f1.file_size = f.Size;
					f1.file_uploaded = f.Uploaded;

					context.SaveChanges();
				}
			}
            return true;
		}

		public void GetFileContent(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			GetFileContentId(c, m, d, f);
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				Content c1 = (from o in context.Contents
							  where o.content_id == f.Content
							  select o).Single();
				f.Data = c1.content_data;
			}
		}
        public FileContents GetFileWithContent(Credentials c, MachineContents m, DirectoryContents d,
                FileIdentity f)
        {
            var fc = new FileContents(f);
            GetFileContentId(c, m, d, fc);
            using (filesyncEntitiesNew context = new filesyncEntitiesNew())
            {
                Content c1 = (from o in context.Contents
                              where o.content_id == f.Content
                              select o).Single();
                fc.Data = c1.content_data;
            }
            return fc;
        }
        public bool DelFile(Credentials c, MachineIdentity m, DirectoryIdentity d,
                FileIdentity f) {
                    throw new NotImplementedException();
        }
		#endregion

		#region File (private)

		private void AddFileContent(FileContents f) {
			int AddedContentId;
			Content f1 = Content.CreateContent(1, f.Data);
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {

				context.Contents.AddObject(f1);
				context.SaveChanges();
				AddedContentId = (from c in context.Contents select c).ToList().Last().content_id;

			}
			f.Content = AddedContentId;
		}

		private static void UpdateFileContent(FileContents f) {
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				Content c1 = (from o in context.Contents
							  where o.content_id == f.Content
							  select o).Single();
				c1.content_data = f.Data;
				context.SaveChanges();
			}
		}

		private int GetFileContentId(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			m.Directories=GetDirList(c, m);
			d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
				int content_id = (from o in context.Files
								  where (o.file_name == f.Name) && (o.dir_id == d.Id)
								  select o.content_id).Single();
				f.Content = content_id;
			}
            return f.Content;
		}

		private void GetFileId(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			GetDirList(c, m);
			d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
			using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {

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
				using (filesyncEntitiesNew context = new filesyncEntitiesNew()) {
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
