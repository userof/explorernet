using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace ExplorerNet
{
    /// <summary>
    /// Application bootstrapper - initializes the shell window.
    /// Prism dependency removed; modules can be loaded directly if needed.
    /// </summary>
    class ExplorerNerBootstrapper
    {
        private StartupEventArgs startupEventArgs = null;

        public ExplorerNerBootstrapper(StartupEventArgs e)
        {
            startupEventArgs = e;
        }

        public Window Run()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;

            Registry.SetValue(
                Registry.CurrentUser.ToString() + @"\Software\Classes\enet_auto\shell\open\command", "",
                string.Format("\"{0}\"\"%1\"", path), RegistryValueKind.String);

            Registry.SetValue(Registry.CurrentUser.ToString() + @"\Software\Classes\enet_auto\DefaultIcon\",
                "", System.IO.Path.GetDirectoryName(path) + System.IO.Path.DirectorySeparatorChar +
                "FileAssotiation.ico", RegistryValueKind.String);

            Registry.SetValue(Registry.CurrentUser.ToString() + @"\Software\Classes\.enet", "", "enet_auto",
                 RegistryValueKind.String);

            ExplorerNet.Shell shell = new Shell();

            ExplorerNet.Languages.LanguagesManager.Init();

            if (startupEventArgs.Args.Count() > 0)
            {
                if (System.IO.File.Exists(startupEventArgs.Args[0]))
                {
                    shell._LoadTemplate(startupEventArgs.Args[0]);
                }
            }
            else
            {
                shell.LoadLastTemplate();
            }

            return shell;
        }
    }
}
