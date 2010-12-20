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
