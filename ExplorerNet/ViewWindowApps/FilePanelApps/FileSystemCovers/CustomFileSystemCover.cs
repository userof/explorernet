using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.SystemIcons;
using ExplorerNet.Tools;

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

        public string Size
        {
            get
            {
                if (fileSystemElement.GetType() == typeof(DirectoryInfo))
                {
                    return Properties.Resources.dir;
                }
                else if (fileSystemElement.GetType() == typeof(FileInfo))
                {
                    FileInfo fi = (FileInfo)fileSystemElement;
                    return SizeFileInString.GetSizeInStr(fi.Length);
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
