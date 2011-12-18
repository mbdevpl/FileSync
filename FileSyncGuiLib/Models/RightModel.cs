using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncLib
{
    public class RightModel
    {
        int id;

        public int Id
        {
            get { return id; }
            private set { id = value; }
        }
        string right;

        public string Right
        {
            get { return right; }
            set { right = value; }
        }
        public RightModel(int id, string right)
        {
            Id = id;
            Right = right;
        }
    }
}
