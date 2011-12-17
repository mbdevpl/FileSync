using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncLib
{
    public class DirModel
    {
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        //int rootdir;

        //public int Rootdir
        //{
        //    get { return rootdir; }
        //    set { rootdir = value; }
        //}
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        int owner;

        public int Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        //List<lib_diruser> dirusers;

        //public List<lib_diruser> Dirusers
        //{
        //    get { return dirusers; }
        //    set { dirusers = value; }
        //}
        List<FileModel> files;

        public List<FileModel> Files
        {
            get { return files; }
            set { files = value; }
        }
        //List<lib_machdir> machdirs;

        //public List<lib_machdir> Machdirs
        //{
        //    get { return machdirs; }
        //    set { machdirs = value; }
        //}
        //List<lib_dir> subdirs;

        //public List<lib_dir> Subdirs
        //{
        //    get { return subdirs; }
        //    set { subdirs = value; }
        //}
        string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        public DirModel( string name, string description, string path)
        {

          
            //Rootdir = rootdir;
            Name = name;
            //Owner = owner;
            Description = description;
            //Dirusers = dirusers;
            //Files = files;
            //Machdirs = machdirs;
            //Subdirs = subdirs;
            Path = path;
        }
    }
}
