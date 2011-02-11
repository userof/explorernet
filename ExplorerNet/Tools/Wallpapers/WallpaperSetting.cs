using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.Tools.Wallpapers
{
    [Serializable]
    public class WallpaperSetting
    {
        public WallpaperKind Kind
        { get; set; }

        public byte  ColorA
        { get; set; }

        public byte ColorR
        { get; set; }

        public byte ColorG
        { get; set; }

        public byte ColorB
        { get; set; }

        public string Path
        { get; set; }

        public System.Windows.Media.Stretch Stretch
        { get; set; }

        public static WallpaperSetting Load()
        {
            WallpaperSetting wallpaperSetting = null;
            if (Properties.Settings.Default.WallpaperSetting == null)
            {
                wallpaperSetting = new WallpaperSetting();
            }
            else
            {
                wallpaperSetting = Properties.Settings.Default.WallpaperSetting;
            }
            return wallpaperSetting;
        }

        public void Save()
        {
            Properties.Settings.Default.WallpaperSetting = this;
            Properties.Settings.Default.Save();
        }
    }
}
