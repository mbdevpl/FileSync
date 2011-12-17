using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncLib
{
    public class DirUserModel
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
        int right;

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
