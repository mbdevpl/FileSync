using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncLib
{
    public class MachdirModel
    {
        int id;

        public int Id
        {
            get { return id; }
            private set { id = value; }
        }
        int dir;

        public int Dir
        {
            get { return dir; }
            set { dir = value; }
        }

        string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        public MachdirModel(int id, int dir, string path) 
        {
            Id = id;
            Dir = dir;
            Path = path;
        }
    }
}
