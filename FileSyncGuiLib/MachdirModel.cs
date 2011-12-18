using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncLib
{
    [DataContract]
    public class MachdirModel
    {
        int id;
        [DataMember]
        public int Id
        {
            get { return id; }
            private set { id = value; }
        }
        int dir;
        [DataMember]
        public int Dir
        {
            get { return dir; }
            set { dir = value; }
        }

        string path;
        [DataMember]
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
        public MachdirModel() { }
    }
}
