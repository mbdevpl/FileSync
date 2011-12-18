using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncLib
{
    [DataContract]
    public class DirUserModel
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
        int right;
        [DataMember]
        public int Right
        {
            get { return right; }
            set { right = value; }
        }
        public DirUserModel(int id, int dir, int right)
        {
            Id = id;
            Dir = dir;
            Right = right;
        }
    }
}
