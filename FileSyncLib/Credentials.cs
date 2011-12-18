using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncLib
{
    [DataContract]
    public class CredentialsLib
    {
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
