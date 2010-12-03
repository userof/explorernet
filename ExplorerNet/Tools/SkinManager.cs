using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ExplorerNet.Tools
{
    public class SkinManager
    {
        private string appPath = "";

        private string skinDirPath = "";

        private const string skinDirName = "skins";

        private const string skinExt = "xaml";

        public SkinManager()
        {
            Init();
        }


        private void Init()
        {
            appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            skinDirPath = Path.GetDirectoryName(appPath) + Path.DirectorySeparatorChar + skinDirName;
        }

        public void ApplySkin(string skinName)
        {
            string skinPath = skinDirPath + Path.DirectorySeparatorChar +
                              skinName + "." + skinExt;

            if (string.IsNullOrEmpty(skinName))
            {
                Properties.Settings.Default.CurrentSkin = "";
                App.Current.Resources = null;
            }
            else if (File.Exists(skinPath))
            {
                Properties.Settings.Default.CurrentSkin = skinName;
                App.Current.Resources.Source = new Uri(skinPath);
            }
            else 
            {
                throw new Exception("The skin not found");
            }
            
        }

        public string[] GetSkins()
        {
            DirectoryInfo dies = new DirectoryInfo(skinDirPath);

            FileInfo[] files = dies.GetFiles("*." + skinExt);

            List<string> skins = new List<string>();

            foreach (var file in files)
            {
                skins.Add(Path.GetFileNameWithoutExtension(file.FullName));
            }

            return skins.ToArray();
        }

        public string GetCurrentSkin()
        {
            return Properties.Settings.Default.CurrentSkin;
        }
    }
}
