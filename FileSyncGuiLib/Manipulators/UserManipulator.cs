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
            var c2 = new RemoteCredentialsLib();
            c2.Login = c.Login;
            c2.Pass = c.Pass;

            var cl = new FileSyncServiceClient();
            var res = cl.GetUser(c2);
            cl.Close();

            return res;
        }
        public static void Add(UserModel u)
        {
            //var c2 = new RemoteCredentialsLib();
            //c2.Login = c.Login;
            //c2.Pass = c.Pass;

            //var cl = new FileSyncServiceClient();
            //var res = cl.LoginIn(c2);
            //cl.Close();

           

        }
        
       

        public static bool LoginIn(CredentialsLib c)
        {


            var c2 = new RemoteCredentialsLib();
            c2.Login = c.Login;
            c2.Pass = c.Pass;

            var cl = new FileSyncServiceClient();
            var res = cl.LoginIn(c2);
            cl.Close();

            return res;


        }
        public static bool Authenticate(CredentialsLib c)
        {


            using (filesyncEntities context = new filesyncEntities())
            {
                User u1;
                try
                {


                    u1 = (from u in context.Users
                          where u.user_login == c.Login && u.user_pass == c.Pass
                          select u).Single();



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
