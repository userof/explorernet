using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExplorerNet.Tools.Wallpapers
{
    internal class WallpaperManager
    {
        private ViewWindow viewWindow = null;

        public void Init()
        {
            //var wm = new WallpaperManager();
            WallpaperSetting ws = WallpaperSetting.Load();


            if (ws.Kind == WallpaperKind.Fon)
            {
                Color color = Color.FromArgb(ws.ColorA,
                    ws.ColorR, ws.ColorG,
                    ws.ColorB);

                ShowWindowFon();
            }
            else if (ws.Kind == WallpaperKind.Picture)
            {
                ShowPicture();
            }

        }

        public WallpaperManager()
        {

            foreach (var w in App.Current.Windows)
            {
                if (w.GetType() == typeof(ViewWindow))
                {
                    this.viewWindow = (ViewWindow)w;
                }
            }

            if (viewWindow == null)
            {
                throw new Exception("Not created ViewWindow!");
            }

        }

        public void RefreshPicture()
        {
            WallpaperSetting ws = WallpaperSetting.Load();
            if (File.Exists(ws.Path))
            {
                File.Copy(ws.Path, ExplorerNet.Sys.AppInfo.PictureFilePath, true);
            }
        }

        private void ShowPicture()
        {
            WallpaperSetting ws = WallpaperSetting.Load();
            string pictPath = ExplorerNet.Sys.AppInfo.PictureFilePath;
            byte[] buffer = System.IO.File.ReadAllBytes(pictPath);
            var ms = new MemoryStream(buffer);


            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            this.viewWindow.imgFon.Stretch = ws.Stretch;
            this.viewWindow.imgFon.Source = bitmapImage;
            this.viewWindow.imgFon.Visibility = Visibility.Visible;
            //ms.Close();
        }

        public void ShowWindowFon()
        {
            WallpaperSetting ws = WallpaperSetting.Load();
            Color color = Color.FromArgb(ws.ColorA, ws.ColorR,
                                         ws.ColorG, ws.ColorB);
            this.viewWindow.Background = new SolidColorBrush(color);
            this.viewWindow.imgFon.Visibility = Visibility.Hidden;

        }
    }
}
