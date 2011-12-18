using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncLib
{
    [DataContract]
    public class MachineModel
    {
        int id;
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        int user;
        [DataMember]
        public int User
        {
            get { return user; }
            set { user = value; }
        }
        string name;
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string description;
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        List<DirModel> directories;
        [DataMember]
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
