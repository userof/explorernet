using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media.Imaging;
using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.SystemIcons;

namespace ExplorerNet.Tools.LastStartedFiles
{
    [Serializable]
    public class LastStartedFile
    {
        public LastStartedFile()
            : base()
        {

        }

        public LastStartedFile(string path)
        {
            this.Path = path;
        }

        public string Path { get; set; }

        public BitmapImage Ico
        {
            get
            {
                return IconExtractor.GetFileIconToBitmapImage(Path);
            }
        }

        public string Name
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }

        public override string ToString()
        {
            return Path;
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        //public override bool Equals(object obj)
        //{
        //    return string.Equals(this.Path,
        //        ((LastStartedFile)obj).Path);
        //}

        //public override bool Equals(object obj)
        //{
        //    return this.GetHashCode() == 
        //        ((LastStartedFile)obj).GetHashCode();
        //}


    }
}
