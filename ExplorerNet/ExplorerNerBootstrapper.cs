using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Win32;

namespace ExplorerNet
{
    class ExplorerNerBootstrapper : UnityBootstrapper
    {
        private StartupEventArgs startupEventArgs = null;

        public ExplorerNerBootstrapper(StartupEventArgs e)
            : base()
        {
            startupEventArgs = e;

        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var moduleCatalog = new DirectoryModuleCatalog();
            moduleCatalog.ModulePath = @".\Modules";
            return moduleCatalog;
        }

        /// <summary>
        /// Instantiates the Shell window.
        /// </summary>
        /// <returns>A new ShellWindow window.</returns>
        protected override DependencyObject CreateShell()
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

            //MessageBox.Show(e.Args[0]);

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

        /// <summary>
        /// Displays the Shell window to the user.
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }
    }
}
