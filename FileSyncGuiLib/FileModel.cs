using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FileSyncLib
{
    [DataContract]
    public class FileModel
    {
        int id;
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        int dir;
        [DataMember]
        public int Dir
        {
            get { return dir; }
            set { dir = value; }
        }
        int type;
        [DataMember]
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        int content;
        [DataMember]
        public int Content
        {
            get { return content; }
            set { content = value; }
        }
        string name;
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        long size;
        [DataMember]
        public long Size
        {
            get { return size; }
            set { size = value; }
        }
        string hash;
        [DataMember]
        public string Hash
        {
            get { return hash; }
            set { hash = value; }
        }
        DateTime modified;
        [DataMember]
        public DateTime Modified
        {
            get { return modified; }
            set { modified = value; }
        }
        DateTime uploaded;
        [DataMember]
        public DateTime Uploaded
        {
            get { return uploaded; }
            set { uploaded = value; }
        }
        byte[] data;
        [DataMember]
        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }
        string typename;
        [DataMember]
        public string Typename
        {
            get { return typename; }
            set { typename = value; }
        }
        public FileModel(string name, long size, string hash, string typename, DateTime uploaded, DateTime modified)
        {
           
            Name = name;
            Size = size;
            Hash = hash;
            Typename = typename;
            Uploaded = uploaded;
            Modified = modified;
        }
    }
}
