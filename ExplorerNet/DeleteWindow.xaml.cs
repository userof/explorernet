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

using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace ExplorerNet
{
    /// <summary>
    /// Interaction logic for DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
        private List<FileSystemInfo> deleteFSIList = null;

        private DeleteWindow()
        {
            InitializeComponent();
        }

        private void Delete(bool toRecycle)
        {
            RecycleOption roToRecycle = RecycleOption.SendToRecycleBin;

            if (!toRecycle)
            {
                roToRecycle = RecycleOption.DeletePermanently;
            }

            foreach(var fsi in deleteFSIList)
            {
                lvMain.SelectedItem = fsi;

                if (fsi.GetType() == typeof(DirectoryInfo))
                {
                    FileSystem.DeleteDirectory(fsi.FullName, UIOption.AllDialogs, roToRecycle);
                }
                else if (fsi.GetType() == typeof(FileInfo))
                {
                    FileSystem.DeleteFile(fsi.FullName, UIOption.AllDialogs, roToRecycle);
                }
                else
                {
                    throw new Exception("Uncnoun type");
                }
            }


        }

        public DeleteWindow(List<FileSystemInfo> deleteFSIList)
            : this()
        {
            this.deleteFSIList = deleteFSIList;

            lvMain.ItemsSource = this.deleteFSIList;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Delete(false);
            this.Close();
        }

        private void btnDeleteToRecycle_Click(object sender, RoutedEventArgs e)
        {
            this.Delete(true);
            this.Close();
        }
    }
}
