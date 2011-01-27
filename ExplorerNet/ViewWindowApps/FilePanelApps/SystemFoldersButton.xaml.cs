using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ExplorerNet.Tools.RecentTemplateNS;

namespace ExplorerNet.ViewWindowApps.FilePanelApps
{
    /// <summary>
    /// Логика взаимодействия для SystemFoldersButton.xaml
    /// </summary>
    public partial class SystemFoldersButton : Button
    {

        public delegate void GoToFolderHandler(object sender, string path);

        public event GoToFolderHandler GoToFolder;

        public SystemFoldersButton()
        {
            InitializeComponent();

            ExplorerNet.Tools.SkinManager.ChangedSkin += delegate(Object sender, string skin)
            {
                Style stl = (Style)App.Current.Resources[typeof(Button)];

                this.Style = stl;

                //Style stl = new Style(this.GetType());
                //stl.BasedOn = new Style(typeof(Button));
                //stl.TargetType = this.GetType();

                //this.Style = stl;

                //this.UpdateLayout();
            };

            BuildRecentTemplates();
        }

        private void BuildRecentTemplates()
        {
            RecentTemplates rts = RecentTemplateManager.GetRecentTemplates(5);

            foreach(var rt in rts)
            {
                MenuItem mi = new MenuItem();
                mi.Header = rt.Name;
                mi.ToolTip = rt.Path;

                mi.Click += delegate(Object sender, RoutedEventArgs e)
                {
                    foreach (var wnd in App.Current.Windows)
                    {
                        if (wnd.GetType() == typeof(ViewWindow))
                        {
                            ViewWindow vw = (ViewWindow)wnd;

                            MenuItem m = (MenuItem)sender;
                            string path = (string)m.ToolTip;

                            vw._LoadTemplate(path);
                        }
                    }

                };

                cmMain.Items.Add(mi);
            }


        }

        private void miDesctop_Click(object sender, RoutedEventArgs e)
        {
            GoToFolder(miDesctop, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            //MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            //MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            //Environment.SpecialFolder.
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cmMain.IsOpen = true;
            //LeftClickMenu.PlacementTarget = this;
            //LeftClickMenu.IsOpen = true;

            //cmMain.Arrange(new Rect());
        }

        private void miDocuments_Click(object sender, RoutedEventArgs e)
        {
            GoToFolder(miDesctop, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

            //MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

            //Environment.SpecialFolder.
        }

        private void miMusic_Click(object sender, RoutedEventArgs e)
        {
            GoToFolder(miDesctop, Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        }

        private void miPicture_Click(object sender, RoutedEventArgs e)
        {
            GoToFolder(miDesctop, Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        }

        private void miVideo_Click(object sender, RoutedEventArgs e)
        {
            GoToFolder(miDesctop, Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
        }

        private void miNetwork_Click(object sender, RoutedEventArgs e)
        {
            GoToFolder(miDesctop, Environment.GetFolderPath(Environment.SpecialFolder.NetworkShortcuts));
        }

        private void miProfile_Click(object sender, RoutedEventArgs e)
        {
            GoToFolder(miDesctop, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }
    }
}
