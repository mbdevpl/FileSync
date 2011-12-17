using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncLib
{
    /// <summary>
    /// Class for handling operation on Dirs table in the database.
    /// This class allows: 
    /// -adding new directory
    /// -getting list of directories for a given machine
    /// </summary>
    public class DirManipulator
    {
        
         public static void AddDirectory(CredentialsLib c, MachineModel m, DirModel d) 
         {
             GetDirList(m);
             int NoSuchNameYet=(from o in m.Directories where o.Name == d.Name select o).Count();
             if (NoSuchNameYet != 0)
             {
                // throw new Exception("directory with given name already exists");
                 //no action needed
             }
             else
             {
                 d.Owner = UserManipulator.LoginToId(c.Login);
                 AddDir(d);
                 m.Id = MachManipulator.MachineNameToId(m.Name);
                 AddMachDir(m, d);
             }
             
         }
        public static void GetDirList(MachineModel m)
        {
            int mach_id = MachManipulator.MachineNameToId(m.Name);
            List<DirModel> dirlist = new List<DirModel>();
            using (filesyncEntities context = new filesyncEntities())
            {

                foreach (var x in (from md in context.MachineDirs
                                   join d in context.Dirs on md.dir_id equals d.dir_id
                                   where md.machine_id == mach_id
                                   select new { md.dir_realpath, d }))
                {
                    DirModel dir = new DirModel( x.d.dir_name, x.d.dir_description, x.dir_realpath);
                    dir.Id=x.d.dir_id;
                    dir.Owner = x.d.user_ownerid;
                    dirlist.Add(dir);
                    
                }

                m.Directories= dirlist;
            }
        }
        private static void AddDir(DirModel d)
        {
            int AddedDirId;
            Dir d1 = Dir.CreateDir(1, d.Name, d.Owner);
            d1.dir_description = d.Description;

            using (filesyncEntities context = new filesyncEntities())
            {
                context.Dirs.AddObject(d1);
                context.SaveChanges();
                AddedDirId = (from z in context.Dirs select z).ToList().Last().dir_id;
            }
            
            d.Id = AddedDirId;

        }
        private static void AddMachDir(MachineModel m, DirModel d)
        {

            MachineDir md1 = MachineDir.CreateMachineDir(m.Id, d.Id, d.Path);
            using (filesyncEntities context = new filesyncEntities())
            {
                context.MachineDirs.AddObject(md1);
                context.SaveChanges();

            }

        }
        
    }

}
