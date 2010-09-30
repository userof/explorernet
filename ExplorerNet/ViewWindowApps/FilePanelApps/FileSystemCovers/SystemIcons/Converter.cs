using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.SystemIcons
{
    class Converter
    {
        public static BitmapImage IconToBitmapImage(System.Drawing.Icon ico)
        {
            MemoryStream ms = new MemoryStream();
            ico.Save(ms);
            BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();

            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(ms.ToArray());
            bImg.CreateOptions = BitmapCreateOptions.None;
            bImg.CacheOption = BitmapCacheOption.Default;
            bImg.EndInit();

            ms.Close();

            return bImg;
        }
    }
}
