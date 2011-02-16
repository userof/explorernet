using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using Microsoft.Win32;

namespace ExplorerNet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Определяет, находимся ли мы в режиме дизайнера
        /// </summary>
        public static bool IsDesignTime { get; private set; }

        static App()
        {
            IsDesignTime = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IsDesignTime = false;
            
            //System.Threading.Thread.CurrentThread.CurrentUICulture = 
            //    new Languages.LanguageManager().GetCurrentCulture();

            ViewWindow vw = new ViewWindow();

            //this.Resources.Source = new Uri(@"skins\BureauBlack.xaml", UriKind.Relative);

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
 
            Registry.SetValue(
                Registry.CurrentUser.ToString() + @"\Software\Classes\enet_auto\shell\open\command", "",
                string.Format("\"{0}\"\"%1\"", path), RegistryValueKind.String);

            Registry.SetValue(Registry.CurrentUser.ToString() + @"\Software\Classes\enet_auto\DefaultIcon\",
                "", System.IO.Path.GetDirectoryName(path) + System.IO.Path.DirectorySeparatorChar + 
                "FileAssotiation.ico", RegistryValueKind.String);

            Registry.SetValue(Registry.CurrentUser.ToString() + @"\Software\Classes\.enet", "", "enet_auto",
                 RegistryValueKind.String);

            //MessageBox.Show(e.Args[0]);

            ExplorerNet.Languages.LanguagesManager.Init();

            if (e.Args.Count() > 0)
            {
                if (System.IO.File.Exists(e.Args[0]))
                {
                    vw._LoadTemplate(e.Args[0]);
                }
            }
            else
            {
                vw.LoadLastTemplate();
            }


            vw.Show();

            //ExplorerNet.Tools.Wallpapers.WallpaperManager.InitWallpaper();

        }
    }
}
