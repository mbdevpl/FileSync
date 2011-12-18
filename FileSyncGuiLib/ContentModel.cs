using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace FileSyncLib
{
    [DataContract]
    public class ContentModel
    {
        int id;
        [DataMember]
        public int Id
        {
            get { return id; }
            private set { id = value; }
        }
        byte[] content;
        [DataMember]
        public byte[] Content
        {
            get { return content; }
            set { content = value; }
        }
        public ContentModel(int id, byte[] content)
        {
            Id = id;
            Content = content;
        }
        public ContentModel() { }
    }
}
