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
    public class FileCover : CustomFileSystemCover
    {
        private FileInfo _file = null;

        public FileCover(FileInfo file)
            : base(file)
        {
            this._file = file;
        }

        public FileInfo FileElement
        {
            get { return _file; }
        }

        public override BitmapImage Ico
        {
            get
            {
                //Icon ico = Icon.ExtractAssociatedIcon(_file.FullName);
                //Icon ico = IconExtractor.GetFileIcon(_file.FullName);

                //return SystemIcons.Converter.IconToBitmapImage(ico);
                return IconExtractor.GetFileIconToBitmapImage(_file.FullName);
            }
        }

        public override string Name
        {
            get { return _file.Name; }
        }
    }
}
