using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncLib
{
    
    public class CredentialsLib
    {
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
        
        public CredentialsLib(string login, string pass)
        {
            Login = login;
            Pass = pass;
        }
        public CredentialsLib() { }
        public CredentialsLib(CredentialsLib c)
        {
            c.Login = Login;
            c.Pass = Pass;
        }
    }
}
