using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncLib
{
    [DataContract]
    public class TypeModel
    {
        int id;
        [DataMember]
        public int Id
        {
            get { return id; }
            private set { id = value; }
        }
        string type;
        [DataMember]
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
        public TypeModel() { }
    }
}
