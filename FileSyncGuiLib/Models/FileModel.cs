using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSyncLib
{
    public class FileModel
    {
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        int dir;

        public int Dir
        {
            get { return dir; }
            set { dir = value; }
        }
        int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        int content;

        public int Content
        {
            get { return content; }
            set { content = value; }
        }
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        long size;

        public long Size
        {
            get { return size; }
            set { size = value; }
        }
        string hash;

        public string Hash
        {
            get { return hash; }
            set { hash = value; }
        }
        DateTime modified;

        public DateTime Modified
        {
            get { return modified; }
            set { modified = value; }
        }
        DateTime uploaded;

        public DateTime Uploaded
        {
            get { return uploaded; }
            set { uploaded = value; }
        }
        byte[] data;

        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }
        string typename;

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
