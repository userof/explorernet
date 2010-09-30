using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ExplorerNet.CopyOrMoveWindowApps
{
    public class FromToFileSystemInfo
    {
        private FileSystemInfo fromPath = null;
        private FileSystemInfo newPath = null;
        private DirectoryInfo toCopy = null;


        public FromToFileSystemInfo(FileSystemInfo fromPath, DirectoryInfo toCopy)
        {
            this.fromPath = fromPath;
            this.toCopy = toCopy;

            if (fromPath.GetType() == typeof(DirectoryInfo))
            {
                this.newPath = new DirectoryInfo(toCopy.FullName + 
                Path.DirectorySeparatorChar.ToString() + fromPath.Name);
            }
            else if (fromPath.GetType() == typeof(FileInfo))
            {
                this.newPath = new FileInfo(toCopy.FullName + 
                Path.DirectorySeparatorChar.ToString() + fromPath.Name);
            }
        }

        public FileSystemInfo FromPath
        {
            get { return this.fromPath; }
        }

        public string FromPathStr
        {
            get { return this.fromPath.FullName; }
        }

        public FileSystemInfo NewPath
        {
            get { return this.newPath; }
        }

        public string NewPathStr
        {
            get { return this.newPath.FullName; }
        }

        public DirectoryInfo ToCopy
        {
            get { return this.toCopy; }
        }
    }
}
