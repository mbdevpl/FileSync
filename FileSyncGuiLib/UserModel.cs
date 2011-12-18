using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncLib
{
    public class UserModel
    {
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        string login;

        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        string pass;

        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }
        string fullname;

        public string Fullname
        {
            get { return fullname; }
            set { fullname = value; }
        }
        string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        DateTime lastlogin;

        public DateTime Lastlogin
        {
            get { return lastlogin; }
            set { lastlogin = value; }
        }
        List<MachineModel> machines;

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
    }
}
