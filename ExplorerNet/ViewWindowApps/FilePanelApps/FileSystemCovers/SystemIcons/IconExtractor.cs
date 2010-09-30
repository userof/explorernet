using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.SystemIcons
{
    public class IconExtractor
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        class Win32
        {
            public const uint SHGFI_ICON = 0x100;
            public const uint SHGFI_LARGEICON = 0x0;    // 'Large icon
            public const uint SHGFI_SMALLICON = 0x1;    // 'Small icon
            public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;

            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath,
                                        uint dwFileAttributes,
                                        ref SHFILEINFO psfi,
                                        uint cbSizeFileInfo,
                                        uint uFlags);
        }

        /// <summary>
        /// Gets the icon asotiated with the filename.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Icon GetFileIcon(string fileName)
        {
            System.Drawing.Icon myIcon = null;
            try
            {
                IntPtr hImgSmall;    //the handle to the system image list
                SHFILEINFO shinfo = new SHFILEINFO();

                //Use this to get the small Icon
                hImgSmall = Win32.SHGetFileInfo(fileName, 0, ref shinfo,
                                                (uint)Marshal.SizeOf(shinfo),
                                                Win32.SHGFI_ICON |
                                               (Win32.SHGFI_LARGEICON | Win32.SHGFI_USEFILEATTRIBUTES));

                //The icon is returned in the hIcon member of the shinfo
                //struct
                myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);

            }
            catch
            {
                return null;
            }
            return myIcon;
        }

        public static BitmapImage GetFileIconToBitmapImage(string fileName)
        {
            Icon ico = GetFileIcon(fileName);

            if (ico == null)
            {
                return null;
            }
            else
            {
                return IconToBitmapImage(ico);
            }
        }

        private static BitmapImage IconToBitmapImage(System.Drawing.Icon ico)
        {
            MemoryStream ms = new MemoryStream();
            ico.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();

            bImg.BeginInit();


            bImg.StreamSource = new MemoryStream(ms.ToArray());
            bImg.EndInit();

            ms.Close();

            return bImg;
        }
    }
}
