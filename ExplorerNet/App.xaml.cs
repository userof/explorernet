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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ViewWindow vw = new ViewWindow();

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Registry.SetValue(
                Registry.CurrentUser.ToString() + @"\Software\Classes\enet_auto\shell\open\command", "",
                string.Format("\"{0}\"\"%1\"", path), RegistryValueKind.String);
            Registry.SetValue(Registry.CurrentUser.ToString() + @"\Software\Classes\.enet", "", "enet_auto",
                 RegistryValueKind.String);

            MessageBox.Show(e.Args[0]);

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
        }
    }
}
