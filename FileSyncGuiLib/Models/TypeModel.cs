using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncLib
{
    public class TypeModel
    {
        int id;

        public int Id
        {
            get { return id; }
            private set { id = value; }
        }
        string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public TypeModel(int id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}
