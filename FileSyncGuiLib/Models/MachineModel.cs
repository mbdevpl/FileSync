using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncLib
{
    public class MachineModel
    {
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        int user;

        public int User
        {
            get { return user; }
            set { user = value; }
        }
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        List<DirModel> directories;

        public List<DirModel> Directories
        {
            get { return directories; }
            set { directories = value; }
        }

       

        public MachineModel( string name, string description)
        {
           
            
            Name=name;
            Description= description;
            
        }

    }
}
