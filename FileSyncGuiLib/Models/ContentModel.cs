using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileSyncLib
{
    public class ContentModel
    {
        int id;

        public int Id
        {
            get { return id; }
            private set { id = value; }
        }
        byte[] content;

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
    }
}
