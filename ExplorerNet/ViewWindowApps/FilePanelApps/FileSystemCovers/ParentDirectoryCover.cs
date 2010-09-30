using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers
{
    public class ParentDirectoryCover : CustomFileSystemCover
    {
        private DirectoryInfo _parentDirectory = null;

        public ParentDirectoryCover(DirectoryInfo parentDirectory)
            : base(parentDirectory)
        {
            _parentDirectory = parentDirectory;
        }

        public DirectoryInfo ParentDirectoryElement
        {
            get { return _parentDirectory; }
        }

        public override BitmapImage Ico
        {
            get
            {
                BitmapImage result = new BitmapImage(new Uri("../Icons/UP.ICO", UriKind.Relative));
                return result;
            }
        }

        public override string Name
        {
            get { return ".."; }
        }
    }
}
