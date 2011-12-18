using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

using FileSyncObjects;

namespace FileSyncWcfService.EntityFramework
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
    public class UserManipulator
    {
        public static void DelUser(CredentialsLib c)
        {
            UserModel u = GetUser(c);

            using (filesyncEntities context = new filesyncEntities())
            {
                try
                {
                    int id = LoginToId(u.Login);
                    var u1 = (from o in context.Users
                              where o.user_id == id
                              select o).Single();
                    context.Users.DeleteObject(u1);
                    context.SaveChanges();
                }
                catch
                {
                    throw new Exception("no such user");
                }


            }
        }
        private static void UpdateLastLogin(int id)
        {


            using (filesyncEntities context = new filesyncEntities())
            {
                try
                {
                    var u1 = (from o in context.Users
                              where o.user_id == id
                              select o).Single();
                    u1.user_lastlogin = DateTime.Now;
                    context.SaveChanges();
                }
                catch
                {
                    throw new Exception("no such user");
                }


            }

        }

        public static bool LoginIn(CredentialsLib c)
        {


            using (filesyncEntities context = new filesyncEntities())
            {
                User u1;
                try
                {


                    u1 = (from u in context.Users
                          where u.user_login == c.Login && u.user_pass == c.Pass
                          select u).Single();
                    UpdateLastLogin(LoginToId(c.Login));


                }
                catch
                {
                    return false;
                }

                return true;
            }

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
        public static bool LoginExists(string login)
        {


            using (filesyncEntities context = new filesyncEntities())
            {
                User u1;
                try
                {


                    u1 = (from o in context.Users
                          where o.user_login == login
                          select o).Single();


                }
                catch
                {
                    return false;
                }

                return true;
            }

        }
        internal static int LoginToId(string login)
        {
            if (LoginExists(login))
            {

                using (filesyncEntities context = new filesyncEntities())
                {

                    User u1 = (from o in context.Users
                               where o.user_login == login
                               select o).Single();

                    return u1.user_id;
                }
            }
            else
            {

                throw new Exception("no such user");

            }

        }

    }
}
