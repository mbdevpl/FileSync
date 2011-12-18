using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FileSyncEntityFramework
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
            if (MachineNameExists(m.Name))
            {
                throw new Exception("machine with given name already exists");
            }
            else
            {
                int user_id = UserManipulator.LoginToId(c.Login);
                Machine m1 = Machine.CreateMachine(1, user_id, m.Name);
                m1.machine_description = m.Description;

                using (filesyncEntities context = new filesyncEntities())
                {
                    context.Machines.AddObject(m1);
                    context.SaveChanges();
                }
            }
        }
        public static void GetMachineList(UserModel user)
        {
            List<MachineModel> machinelist = new List<MachineModel>();
            user.Id = UserManipulator.LoginToId(user.Login);
            using (filesyncEntities context = new filesyncEntities())
            {

                List<Machine> ml = (from o in context.Machines
                                    where o.user_id == user.Id
                                    select o).ToList();
                foreach (Machine m in ml)
                {
                    MachineModel m1 = new MachineModel(m.machine_name, m.machine_description);
                    m1.Id = m.machine_id;
                    m1.User = m.user_id;
                    machinelist.Add(m1);
                }
                user.Machines = machinelist;

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
    }
}
