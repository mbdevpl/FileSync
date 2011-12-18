using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using FileSyncLib.ServiceRef;



namespace FileSyncLib
{
    /// <summary>
    /// Class used to managing Users table
    /// Allows to:
    /// - add a new user
    /// - log in
    /// - check if a given username is available or occupied
    /// - delete a user
    /// - update information about last login
    /// - authenticate (check wheather a passwd maches login)
    /// </summary>
    [DataContract]
    public class UserManipulator
    {
        public static UserModel GetUser(CredentialsLib c)
        {
			//var c2 = new RemoteCredentialsLib();
			//c2.Login = c.Login;
			//c2.Pass = c.Pass;

            var cl = new FileSyncServiceClient();
            var u = cl.GetUser(c);
            cl.Close();

            var res = new UserModel(u.Login, u.Pass, u.Fullname, u.Email);
            
            return  res;
        }
        public static void Add(UserModel u)
        {
            //var u2 = new RemoteUserModel();
            //u2.Login = u.Login;
            //u2.Pass = u.Pass;
            //u2.Email = u.Email;
            //u2.Fullname = u.Fullname;
            //u2.Id = u.Id;
            //u2.Lastlogin = u.Lastlogin;
            //u2.Machines = u.Machines;
            

            var cl = new FileSyncServiceClient();
            cl.AddUser(u);
            cl.Close();

           

        }
        
       

        public static bool LoginIn(CredentialsLib c)
        {


			//var c2 = new RemoteCredentialsLib();
			//c2.Login = c.Login;
			//c2.Pass = c.Pass;

            var cl = new FileSyncServiceClient();
            var res = cl.LoginIn(c);
            cl.Close();

            return res;


        }
        
     

    }
}
