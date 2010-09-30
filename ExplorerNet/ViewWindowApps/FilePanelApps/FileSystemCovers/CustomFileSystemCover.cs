using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.SystemIcons;

namespace ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers
{
    public abstract class CustomFileSystemCover
    {
        private FileSystemInfo fileSystemElement = null;

        public CustomFileSystemCover(FileSystemInfo fileSystemElement)
        {
            this.fileSystemElement = fileSystemElement;
        }

        public FileSystemInfo FileSystemElement
        {
            get { return fileSystemElement; }
        }

        private string GetSizeInStr(long bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);


            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";

        }

        public string Size
        {
            get
            {
                if (fileSystemElement.GetType() == typeof(DirectoryInfo))
                {
                    return "<dir>";
                }
                else if (fileSystemElement.GetType() == typeof(FileInfo))
                {
                    FileInfo fi = (FileInfo)fileSystemElement;
                    return GetSizeInStr(fi.Length);
                }
                else
                {
                    throw new Exception("Uncnown type");
                }
            }
        }

        public abstract BitmapImage Ico
        {
            get;
        }

        public abstract string Name
        {
            get;
        }

    }
}
