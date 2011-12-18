using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncEntityFramework
{
    /// <summary>
    /// Class for handling Types table in the database
    /// Allows: 
    /// - adding a type
    /// - getting list of existing types
    /// - checking if a given type already exists
    /// </summary>
    public class TypeManipulator
    {
        
		//public static void AddType(FileModel f)
		//{
		//    if (TypeNameExists(f.Typename))
		//    {
		//        throw new Exception("type with given name already exists");
		//    }
		//    else
		//    {
		//        Type t = Type.CreateType(1, f.Typename);

		//        using (filesyncEntities context = new filesyncEntities())
		//        {

		//            context.Types.AddObject(t);
		//            context.SaveChanges();

		//        }
		//    }
		//}
		//internal static void TypeToId(FileModel f)
		//{
		//    if (!TypeNameExists(f.Typename))
		//    {
		//        throw new Exception("no such type exists");
		//    }
		//    else
		//    {
		//        using (filesyncEntities context = new filesyncEntities())
		//        {


		//            int type_id = (from o in context.Types where o.type_name == f.Typename select o.type_id).Single();
		//            f.Type = type_id;

		//        }
		//    }
		//}
		//public static List<string> GetTypeList()
		//{
		//    List<string> typelist;
		//    using (filesyncEntities context = new filesyncEntities())
		//    {

		//        typelist=(from o in context.Types select o.type_name).ToList();

		//    }
		//    return typelist;
		//}
		//private static bool TypeNameExists(string name)
		//{


		//    using (filesyncEntities context = new filesyncEntities())
		//    {
		//        Type t1;
		//        try
		//        {


		//            t1 = (from o in context.Types
		//                  where o.type_name == name
		//                  select o).Single();


		//        }
		//        catch
		//        {
		//            return false;
		//        }

		//        return true;
		//    }

		//}
    }
}
