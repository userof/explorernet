using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows;
using System.Windows.Baml2006;

namespace ExplorerNet.Tools
{
    public class SkinManager
    {
        private string appPath = "";

        private string skinDirPath = "";

        private const string skinDirName = "skins";

        //private const string skinExt = "xaml";

        private const string skinSearchFilter = "*.?aml";

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
            string skinPathXaml = skinDirPath + Path.DirectorySeparatorChar +
                  skinName + ".xaml";

            string skinPathBaml = skinDirPath + Path.DirectorySeparatorChar +
                  skinName + ".baml";

            if (File.Exists(skinPathXaml))
            {
                //Application.Current.Resources.Clear();
                ApplySkinXaml(skinPathXaml);
                Properties.Settings.Default.CurrentSkin = skinName;
            }
            else if (File.Exists(skinPathBaml))
            {
                //Application.Current.Resources.Clear();
                ApplySkinBaml(skinPathBaml);
                Properties.Settings.Default.CurrentSkin = skinName;
            }
            else
            {
                //throw new Exception("The skin not found");
            }
            
        }

        protected void ApplySkinXaml(string skinPath)
        {
            System.Windows.ResourceDictionary rd = new System.Windows.ResourceDictionary();
            rd.Source = new Uri(skinPath);
            App.Current.Resources.MergedDictionaries.Add(rd);
 
        }

        protected void ApplySkinBaml(string skinPath)
        {

            FileStream fstream = new FileStream(skinPath, FileMode.Open);
            Baml2006Reader reader = new Baml2006Reader(fstream);

            ResourceDictionary rd = (ResourceDictionary)System.Windows.Markup.XamlReader.Load(reader);
            App.Current.Resources.MergedDictionaries.Add(rd);

            reader.Close();
            fstream.Close();
        }

        public string[] GetSkins()
        {
            DirectoryInfo dies = new DirectoryInfo(skinDirPath);

            FileInfo[] files = dies.GetFiles(skinSearchFilter);

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
