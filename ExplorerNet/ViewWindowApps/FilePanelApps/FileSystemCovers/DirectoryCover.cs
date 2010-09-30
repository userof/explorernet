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
    public class DirectoryCover : CustomFileSystemCover
    {
        private DirectoryInfo _directory = null;

        public DirectoryCover(DirectoryInfo directory)
            : base(directory)
        {
            this._directory = directory;
        }

        public DirectoryInfo DirectoryElement
        {
            get { return _directory; }

        }

        public override BitmapImage Ico
        {
            get
            {
                BitmapImage result = new BitmapImage(new Uri("../Icons/dir.ico", UriKind.Relative));
                return result; 
            }
        }

        public override string Name
        {
            get { return _directory.Name; }
        }
    }
}
