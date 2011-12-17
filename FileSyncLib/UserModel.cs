using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncLib
{
    [DataContract]
    public class UserModel
    {
        int id;
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        string login;
        [DataMember]
        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        string pass;
        [DataMember]
        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }

        string fullname;
        [DataMember]
        public string Fullname
        {
            get { return fullname; }
            set { fullname = value; }
        }
        string email;
        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        DateTime lastlogin;
        [DataMember]
        public DateTime Lastlogin
        {
            get { return lastlogin; }
            set { lastlogin = value; }
        }
        List<MachineModel> machines;
        [DataMember]
        public List<MachineModel> Machines
        {
            get { return machines; }
            set { machines = value; }
        }
        public UserModel( string login, string pass, string fullname, string email)
        {
            
            Login = login;
            Pass = pass;
            Fullname = fullname;
            Email = email;
        }
        public UserModel() { }
    }
}
