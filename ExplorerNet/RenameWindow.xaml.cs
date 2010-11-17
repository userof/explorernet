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

using System.IO;
using ExplorerNet.ViewWindowApps;
using ExplorerNet.ViewWindowApps.FilePanelApps;

namespace ExplorerNet
{
    /// <summary>
    /// Логика взаимодействия для RenameWindow.xaml
    /// </summary>
    public partial class RenameWindow : Window
    {
        private FileSystemInfo renamingFile = null;

        public RenameWindow()
        {
            InitializeComponent();

            var list = FilePanelSelector.FirstSelected.SelectedFiles;
            this.renamingFile = list[0];
            txtName.Text = this.renamingFile.Name;
            txtName.SelectAll();
            txtName.Focus();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.renamingFile.GetType() == typeof(FileInfo))
            {
                File.Move(this.renamingFile.FullName,
                    System.IO.Path.GetDirectoryName(this.renamingFile.FullName) +
                    System.IO.Path.DirectorySeparatorChar + txtName.Text);
                this.Close();
            }
            else if (this.renamingFile.GetType() == typeof(DirectoryInfo))
            {
                Directory.Move(this.renamingFile.FullName,
                    System.IO.Path.GetDirectoryName(this.renamingFile.FullName) +
                    System.IO.Path.DirectorySeparatorChar + txtName.Text);
                this.Close();
            }
            else
            {
                throw new Exception("Uncnoun type!");
            }
        }
    }
}
