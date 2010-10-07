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
using System.Windows.Shapes;

namespace ExplorerNet.ViewWindowApps.FilePanelApps
{
    /// <summary>
    /// Логика взаимодействия для CreateFolderWindow.xaml
    /// </summary>
    public partial class CreateFolderWindow : Window
    {

        public CreateFolderWindow()
        {
            InitializeComponent();
            txtDirectoryName.Focus();
        }

        public CreateFolderWindow(string directoryPath)
            : this()
        {
            txtInfo.Text += directoryPath;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        public string DirectoryName
        {
            get { return txtDirectoryName.Text; }
        }
    }
}
