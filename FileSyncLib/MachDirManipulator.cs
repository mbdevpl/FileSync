using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncLib
{
    /// <summary>
    /// Class adding entry to MachDir table 
    /// used to add association between a directory with a machine
    /// </summary>
    public class MachDirManipulator
    {
        public static void Add(MachdirModel md)
        {
            MachineDir md1=MachineDir.CreateMachineDir(1,md.Dir,md.Path);
            
            using (filesyncEntities context = new filesyncEntities())
            {

                context.MachineDirs.AddObject(md1);
                context.SaveChanges();

            }
        }
        

    }
}
