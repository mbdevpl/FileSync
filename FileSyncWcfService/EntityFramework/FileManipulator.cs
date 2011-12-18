using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncEntityFramework
{
    /// <summary>
    /// Class that handles operation on tables: Files, Content
    /// Allows:
    /// - adding file metadatas
    /// -adding file content
    /// -updating file metadata
    /// - updating file ccontent
    /// -checking wheater file with given name exists in a given directory
    /// </summary>
    public class FileManipulator
    {
        public static void AddFile(CredentialsLib c, MachineModel m, DirModel d, FileModel f)
        {

            DirManipulator.GetDirList(m);
            f.Dir = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
            if (!CheckFileExistence(m, d, f))
            {
                AddFileContent(f);
                TypeManipulator.TypeToId(f);
                File f1 = File.CreateFile(1, f.Dir, f.Type, f.Content, f.Name, f.Size, f.Hash, f.Uploaded, f.Modified);

                using (filesyncEntities context = new filesyncEntities())
                {
                    context.Files.AddObject(f1);
                    context.SaveChanges();
                }
            }
            else 
            {
                GetFileId(m, d, f);
                GetFileContentId(m, d, f);
                UpdateFileContent(f);
                TypeManipulator.TypeToId(f);
                              

                using (filesyncEntities context = new filesyncEntities())
                {
                    File f1 =  (from o in context.Files where o.file_id == f.Id select o).Single();
                    f1.file_hash = f.Hash;
                    f1.file_modified = f.Modified;
                    f1.file_size = f.Size;
                    f1.file_uploaded = f.Uploaded;
                    
                    context.SaveChanges();
                }
            }
        }
        private static void AddFileContent(FileModel f)
        {
            int AddedContentId;
            Content f1 = Content.CreateContent(1, f.Data);
            using (filesyncEntities context = new filesyncEntities())
            {
                
                context.Contents.AddObject(f1);
                context.SaveChanges();
                AddedContentId=(from c in context.Contents select c).ToList().Last().content_id;
                
            }
            f.Content= AddedContentId;
        }
        private static void UpdateFileContent(FileModel f)
        {
           
           
            using (filesyncEntities context = new filesyncEntities())
            {
                Content c1 = (from o in context.Contents
                              where o.content_id == f.Content
                              select o).Single();
                c1.content_data=f.Data;
                context.SaveChanges();

            }
            
        }
        public static void GetFileList(CredentialsLib c, MachineModel m, DirModel d)
        { 
            List<FileModel> filelist = new List<FileModel>();
            DirManipulator.GetDirList(m);
            d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
            using (filesyncEntities context = new filesyncEntities())
            {

                foreach (var x in (from f in context.Files
                                   join t in context.Types on f.type_id equals t.type_id
                                   where f.dir_id == d.Id
                                   select new { f, t.type_name}))
                {
                    FileModel file = new FileModel(x.f.file_name, x.f.file_size,
                        x.f.file_hash, x.type_name, x.f.file_uploaded, x.f.file_modified);
                    file.Content = x.f.content_id;
                    file.Id = x.f.file_id;
                    filelist.Add(file);
                }
            }
                d.Files= filelist;
        }
        public static void GetFileContent(CredentialsLib c, MachineModel m, DirModel d, FileModel f)
        {
            GetFileContentId(m,d,f);
            using (filesyncEntities context = new filesyncEntities())
            {
               
                Content c1 = (from o in context.Contents
                           where o.content_id == f.Content
                           select o).Single();
                f.Data = c1.content_data;


            }
        }
        private static void GetFileContentId(MachineModel m, DirModel d,FileModel f)
        {
            DirManipulator.GetDirList(m);
            d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
            using (filesyncEntities context = new filesyncEntities())
            {

               int content_id = (from o in context.Files
                              where (o.file_name == f.Name) &&  (o.dir_id==d.Id)
                              select o.content_id).Single();
               f.Content = content_id;
               


            }
        }
        private static void GetFileId(MachineModel m, DirModel d, FileModel f)
        {
            DirManipulator.GetDirList(m);
            d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
            using (filesyncEntities context = new filesyncEntities())
            {
                
                int file_id = (from o in context.Files where (o.file_name == f.Name) && (o.dir_id == d.Id)
                                  select o.file_id).Single();
                
                f.Id = file_id;


            }
        }
        private static bool CheckFileExistence(MachineModel m, DirModel d, FileModel f)
        {
            DirManipulator.GetDirList(m);
            d.Id = (from o in m.Directories where o.Name == d.Name select o.Id).Single();
            try
            {
                using (filesyncEntities context = new filesyncEntities())
                {

                     (from o in context.Files
                                   where (o.file_name == f.Name) && (o.dir_id == d.Id)
                                   select o.file_id).Single();
                    


                }
            }
            catch 
            {
                return false;
            }
            return true;
        }
    }
}
