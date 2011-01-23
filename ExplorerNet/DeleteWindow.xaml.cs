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
using ExplorerNet.Tools;

namespace ExplorerNet
{
    /// <summary>
    /// Interaction logic for DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
        private List<FileSystemInfo> deleteFSIList = null;

        public static RoutedCommand DeleteFilesCommand = new RoutedCommand();

        public static RoutedCommand DeleteFilesToRecCommand = new RoutedCommand();

        private DeleteWindow()
        {
            InitializeComponent();

            CommandBinding cbDelete = new CommandBinding(DeleteFilesCommand, ExecutedDeleteFilesCommand);
            this.CommandBindings.Add(cbDelete);

            KeyGesture kgDel = new KeyGesture(Key.F8);
            KeyBinding kbDel = new KeyBinding(DeleteFilesCommand, kgDel);
            this.InputBindings.Add(kbDel);

            //////////////////////////
            CommandBinding cbDeleteToRec = new CommandBinding(DeleteFilesToRecCommand, 
                ExecutedDeleteFilesToRecCommand);
            this.CommandBindings.Add(cbDeleteToRec);

            KeyGesture kgDelToRec = new KeyGesture(Key.Delete);
            KeyBinding kbDelToRec = new KeyBinding(DeleteFilesToRecCommand, kgDelToRec);
            this.InputBindings.Add(kbDelToRec);
        }

        private void ExecutedDeleteFilesCommand(object sender, ExecutedRoutedEventArgs e)
        {
            this.Delete(false);
            this.Close();
        }

        private void ExecutedDeleteFilesToRecCommand(object sender, ExecutedRoutedEventArgs e)
        {
            this.Delete(true);
            this.Close();
        }
            

        private void Delete(bool toRecycle)
        {

            FileManager fm = new FileManager();

            if (toRecycle)
            {
                fm.DeleteToRecycler(deleteFSIList);
            }
            else
            {
                fm.Delete(deleteFSIList);
            }

            //RecycleOption roToRecycle = RecycleOption.SendToRecycleBin;

            //if (!toRecycle)
            //{
            //    roToRecycle = RecycleOption.DeletePermanently;
            //}

            //foreach(var fsi in deleteFSIList)
            //{
            //    lvMain.SelectedItem = fsi;

            //    if (fsi.GetType() == typeof(DirectoryInfo))
            //    {
            //        FileSystem.DeleteDirectory(fsi.FullName, UIOption.OnlyErrorDialogs, roToRecycle);
            //    }
            //    else if (fsi.GetType() == typeof(FileInfo))
            //    {
            //        FileSystem.DeleteFile(fsi.FullName, UIOption.OnlyErrorDialogs, roToRecycle);
            //    }
            //    else
            //    {
            //        throw new Exception("Uncnoun type");
            //    }
            //}


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
