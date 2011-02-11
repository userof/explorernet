using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.IO;

namespace ExplorerNet.Tools.Wallpapers
{
    public class WallpaperManager
    {

        private const string PictureFile = "Fon.img";

        private string userPicturePath = "";

        private WallpaperSetting wallpaperSetting = null;

        private ViewWindow viewWindow = null;

        //private static BitmapImage lastBitmapImage = null;

        private string PictureFilePath
        {
            get
            {
                return ExplorerNet.Sys.AppInfo.AppDirectory +
                    System.IO.Path.DirectorySeparatorChar +
                    PictureFile;
            }
        }
        //public void ApplyWallpaper(Image image, Window window)
        //{
        //    image.Visibility = System.Windows.Visibility.Hidden;
        //    window.Background = Brushes.Blue;

        //    //image.Source = new BitmapImage(new Uri(@"h:\pic\otBuh\л\WinampTech3D.png", UriKind.Absolute));
        //}

        public WallpaperManager()
        {
            wallpaperSetting = WallpaperSetting.Load();

            foreach (var w in App.Current.Windows)
            {
                if (w.GetType() == typeof(ViewWindow))
                {
                    viewWindow = (ViewWindow)w;
                }

            }

            //if (lastBitmapImage == null)
            //{
            //    viewWindow.imgFon.Source = lastBitmapImage;
            //    lastBitmapImage = new BitmapImage();
            //}
        }

        public static void InitWallpaper()
        {
            WallpaperManager wm = new WallpaperManager();

            if (wm.wallpaperSetting.Kind == WallpaperKind.Fon)
            {
                Color color = Color.FromArgb(wm.wallpaperSetting.ColorA,
                    wm.wallpaperSetting.ColorR, wm.wallpaperSetting.ColorG,
                    wm.wallpaperSetting.ColorB);

                wm.ChangeWindowFon(color);
            }
            else if (wm.wallpaperSetting.Kind == WallpaperKind.Picture)
            {
                wm.userPicturePath = wm.wallpaperSetting.Path;
                wm.ShowPicture();
            }
        }

        public void ChangeStretch(System.Windows.Media.Stretch streach)
        {
            viewWindow.imgFon.Stretch = streach;
            wallpaperSetting.Stretch = streach;
            wallpaperSetting.Save();
        }

 

        public void ShowPicture()
        {
            string path = "";

            if (File.Exists(userPicturePath))
            {
                //if (lastBitmapImage != null)
                //{
                //    lastBitmapImage.UriSource = new Uri("");
                //}
                path = userPicturePath;
            }
            else if (File.Exists(PictureFilePath))
            {
                path = PictureFilePath;
            }

            viewWindow.imgFon.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            viewWindow.imgFon.Visibility = Visibility.Visible;
            wallpaperSetting.Kind = WallpaperKind.Picture;
            wallpaperSetting.Path = path;
            wallpaperSetting.Save();     
        }

        public void ShowFon()
        {
            viewWindow.imgFon.Visibility = Visibility.Hidden;
            wallpaperSetting.Kind = WallpaperKind.Fon;
            wallpaperSetting.Save();
        }

        public void ChangePicture(string picturePath)
        {
            userPicturePath = picturePath;

            //if (File.Exists(PictureFilePath))
            //{

            //    File.Delete(PictureFilePath);
            //}
            //File.Copy(picturePath, PictureFilePath, true);

            ShowPicture();
        }

        public void ChangeWindowFon(Color color)
        {

            viewWindow.Background = new SolidColorBrush(color);
            ShowFon();
            
        }

        public WallpaperSetting WallpaperSetting
        {
            get
            {
                return wallpaperSetting;
            }
        }

        public Color GetColor()
        {
            Color color = Color.FromArgb(wallpaperSetting.ColorA,
                    wallpaperSetting.ColorR, wallpaperSetting.ColorG,
                    wallpaperSetting.ColorB);
            return color;
        }



    }
}
