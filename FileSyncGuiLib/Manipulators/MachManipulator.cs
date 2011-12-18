using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FileSyncLib
{
    /// <summary>
    /// Class handling operations in table Machines and MachDirs
    /// Allows to:
    /// - add machine
    /// - change machine details
    /// - obtain list of machines for a given user
    /// - check weather a machine with a given name exists in the database
    /// </summary>
    public class MachManipulator
    {
        public static void AddMachine(CredentialsLib c, MachineModel m)
        {
            //
        }
        public static void GetMachineList(UserModel user)
        {
            //

        }
        internal static int MachineNameToId(string name)
        {

            if (MachineNameExists(name))
            {
                using (filesyncEntities context = new filesyncEntities())
                {

                    Machine m1 = (from o in context.Machines
                                  where o.machine_name == name
                                  select o).Single();

                    return m1.machine_id;
                }
            }
            else
            {
                throw new Exception("no machine with given name found"+name.ToString());
            }


        }
        public static void ChangeMachineDetails(CredentialsLib c, MachineModel newMachine, MachineModel oldMachine)
        {

            oldMachine.Id = MachineNameToId(oldMachine.Name);
            
            using (filesyncEntities context = new filesyncEntities())
            {

                Machine m1 = (from o in context.Machines
                              where o.machine_id == oldMachine.Id
                              select o).Single();
                m1.machine_name = newMachine.Name;
                m1.machine_description = newMachine.Description;
                context.SaveChanges();



            }


        }
        private static bool MachineNameExists(string name)
        {


            using (filesyncEntities context = new filesyncEntities())
            {
                Machine m1;
                try
                {


                    m1 = (from o in context.Machines
                          where o.machine_name == name
                          select o).Single();


                }
                catch
                {
                    return false;
                }

                return true;
            }

        }
    }
}
