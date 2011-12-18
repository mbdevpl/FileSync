using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncLib
{   
    [DataContract]
    public class RightModel
    {
        int id;
        [DataMember]
        public int Id
        {
            get { return id; }
            private set { id = value; }
        }
        string right;
        [DataMember]
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
