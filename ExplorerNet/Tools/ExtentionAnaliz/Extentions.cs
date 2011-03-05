using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.Tools.ExtentionAnaliz
{
    [Serializable] //ExplorerNet.Tools.ExtentionAnaliz.Extentions
    public class Extentions : Dictionary<string, PreviewKind>
    {
        public static Extentions Load()
        {
            if (Properties.Settings.Default.ExtentionDictionar == null)
            {
                var exts = Create();
                Properties.Settings.Default.ExtentionDictionar = exts;
                return exts;
            }
            else
            {
                return Properties.Settings.Default.ExtentionDictionar;
            }
        }

        private static Extentions Create()
        {
            Extentions exts = new Extentions();

            exts.Add(".mp3", PreviewKind.Media);
            exts.Add(".wav", PreviewKind.Media);
            exts.Add(".avi", PreviewKind.Media);
            exts.Add(".mpg", PreviewKind.Media);
            exts.Add(".mpeg", PreviewKind.Media);
            exts.Add(".bmp", PreviewKind.Media);
            exts.Add(".jpg", PreviewKind.Media);
            exts.Add(".jpeg", PreviewKind.Media);
            exts.Add(".gif", PreviewKind.Media);
            exts.Add(".png", PreviewKind.Media);
            exts.Add(".ico", PreviewKind.Media);

            exts.Add(".html", PreviewKind.Web);
            exts.Add(".htm", PreviewKind.Web);
            exts.Add(".pdf", PreviewKind.Web);

            return exts;
        }

        public void Save()
        {
            //Properties.Settings.Default.ExtentionDictionar = Create();
            Properties.Settings.Default.ExtentionDictionar = this;
            Properties.Settings.Default.Save();
        }

    }
}
