using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

using System.Windows;
using System.Windows.Controls;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.SystemIcons;
using ExplorerNet.Tools;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.CoverExts;

namespace ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers
{
    public abstract class CustomFileSystemCover
    {
        private FileSystemInfo fileSystemElement = null;

        private CoverExt coverExt = null;

        public CustomFileSystemCover(FileSystemInfo fileSystemElement)
        {
            this.fileSystemElement = fileSystemElement;

            coverExt = CoverExt.Load(fileSystemElement.FullName);
        }

        public FileSystemInfo FileSystemElement
        {
            get { return fileSystemElement; }
        }

        //public string Size
        //{
        //    get
        //    {
        //        if (fileSystemElement.GetType() == typeof(DirectoryInfo))
        //        {
        //            return Properties.Resources.dir;
        //        }
        //        else if (fileSystemElement.GetType() == typeof(FileInfo))
        //        {
        //            FileInfo fi = (FileInfo)fileSystemElement;
        //            return SizeFileInString.GetSizeInStr(fi.Length);
        //        }
        //        else
        //        {
        //            throw new Exception("Uncnown type");
        //        }
        //    }
        //}

        public SizePanel Size
        {
            get
            {
                SizePanel sp = new SizePanel();
                sp.Data = this;

                return sp;
            //    TextBlock txt = new TextBlock();

            //    if (fileSystemElement.GetType() == typeof(DirectoryInfo))
            //    {
            //        txt.Text = Properties.Resources.dir;;
            //        return txt;
            //    }
            //    else if (fileSystemElement.GetType() == typeof(FileInfo))
            //    {
            //        FileInfo fi = (FileInfo)fileSystemElement;
            //        txt.Text = SizeFileInString.GetSizeInStr(fi.Length);
            //        return txt;
            //    }
            //    else
            //    {
            //        throw new Exception("Uncnown type");
            //    }
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

        public virtual Visibility StarVisible
        {
            get { return Visibility.Visible;}
        }

        public StarKind? Star
        {
            get
            {
                if (coverExt != null)
                {
                    return coverExt.Star;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (coverExt == null)
                {
                    coverExt = CoverExt.CreateOrLoad(this.fileSystemElement.FullName);
                }

                coverExt.Star = value;
                coverExt.Save();
            }
        }

        public virtual Visibility DescriptionVisible
        {
            get
            {
                return Visibility.Visible;
            }
        }

        public virtual string Description
        {
            get
            {
                if (coverExt != null)
                {
                    //coverExt.Description = "desc";
                    return coverExt.Description;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (coverExt == null)
                {
                    coverExt = CoverExt.CreateOrLoad(this.fileSystemElement.FullName);
                }

                coverExt.Description = value;
                coverExt.Save();
            }
        }

    }
}
