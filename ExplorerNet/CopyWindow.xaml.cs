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
using ExplorerNet.CopyWindowApps;
using ExplorerNet.ViewWindowApps.FilePanelApps;
using Microsoft.VisualBasic.FileIO;

namespace ExplorerNet
{
    /// <summary>
    /// Логика взаимодействия для CopyWindow.xaml
    /// </summary>
    public partial class CopyWindow : Window
    {
        public static RoutedCommand CopyCommand = new RoutedCommand();
        public static RoutedCommand MoveCommand = new RoutedCommand();

        public CopyWindow()
        {
            InitializeComponent();

            /////////////////////////
            CommandBinding cbCopy = new CommandBinding(CopyCommand, ExecutedCopyCommand);
            this.CommandBindings.Add(cbCopy);

            KeyGesture kgCopyF5 = new KeyGesture(Key.F5);
            KeyBinding kbCopyF5 = new KeyBinding(CopyCommand, kgCopyF5);

            this.InputBindings.Add(kbCopyF5);

            ////////////////////////
            CommandBinding cbMove = new CommandBinding(MoveCommand, ExecutedMoveCommand);
            this.CommandBindings.Add(cbMove);

            KeyGesture kgMoveF6 = new KeyGesture(Key.F6);
            KeyBinding kbMoveF6 = new KeyBinding(MoveCommand, kgMoveF6);

            this.InputBindings.Add(kbMoveF6);

            CreateVisualData();
            //lvFromCopy.ItemsSource = copyFiles;
            //cbToCopy.ItemsSource = FilePanelSelector.FilePanels;
            //cbToCopy.Text = FilePanelSelector.SecondSelected.p;

            //FilePanel fpFirst = null;
            //if (FilePanelSelector.FirstSelected != null)
            //{
            //    fpFirst = FilePanelSelector.FirstSelected;
            //}
            //else
            //{
            //    throw new Exception("Not set FilePanelSelector.FirstSelected");
            //}

            //FilePanel fpSecond = null;
            //if (FilePanelSelector.SecondSelected != null)
            //{
            //    fpSecond = FilePanelSelector.SecondSelected;
            //}
            //else
            //{
            //    throw new Exception("Not set FilePanelSelector.SecondSelected");
            //}
        }

        private void ExecutedCopyCommand(object sender,
            ExecutedRoutedEventArgs e)
        {
            string destinationPath = "";

            foreach (var fsi in lvFromCopy.Items)
            {
                lvFromCopy.SelectedItem = fsi;
                this.Show();

                if (fsi.GetType() == typeof(DirectoryInfo))
                {
                    DirectoryInfo di = (DirectoryInfo)fsi;
                    destinationPath = cbToCopy.Text + System.IO.Path.DirectorySeparatorChar + di.Name;
                    switch (Properties.Settings.Default.FileOverwriteOption)
                    {
                        case FileOverwriteOptionKind.ShowDialog:
                            FileSystem.CopyDirectory(di.FullName, destinationPath,
                                UIOption.AllDialogs, UICancelOption.DoNothing);
                            break;
                        case FileOverwriteOptionKind.Skip:
                            FileSystem.CopyDirectory(di.FullName, destinationPath, false);
                            break;
                        case FileOverwriteOptionKind.Rewrite:
                            FileSystem.CopyDirectory(di.FullName, destinationPath, true);
                            break;
                        default:
                            break;
                    }
                }
                else if (fsi.GetType() == typeof(FileInfo))
                {
                    FileInfo fi = (FileInfo)fsi;
                    destinationPath = cbToCopy.Text + System.IO.Path.DirectorySeparatorChar + fi.Name;
                    switch (Properties.Settings.Default.FileOverwriteOption)
                    {
                        case FileOverwriteOptionKind.ShowDialog:
                            FileSystem.CopyFile(fi.FullName, destinationPath,
                                UIOption.AllDialogs, UICancelOption.DoNothing);
                            break;
                        case FileOverwriteOptionKind.Skip:
                            FileSystem.CopyFile(fi.FullName, destinationPath, false);
                            break;
                        case FileOverwriteOptionKind.Rewrite:
                            FileSystem.CopyFile(fi.FullName, destinationPath, true);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    throw new Exception();
                }

            }

            this.Close();
        }

        private void ExecutedMoveCommand(object sender,
            ExecutedRoutedEventArgs e)
        {

            string destinationPath = "";

            foreach (var fsi in lvFromCopy.Items)
            {
                lvFromCopy.SelectedItem = fsi;
                this.Show();

                if (fsi.GetType() == typeof(DirectoryInfo))
                {
                    DirectoryInfo di = (DirectoryInfo)fsi;
                    destinationPath = cbToCopy.Text + System.IO.Path.DirectorySeparatorChar + di.Name;
                    switch (Properties.Settings.Default.FileOverwriteOption)
                    {
                        case FileOverwriteOptionKind.ShowDialog:
                            FileSystem.MoveDirectory(di.FullName, destinationPath,
                                UIOption.AllDialogs, UICancelOption.DoNothing);
                            break;
                        case FileOverwriteOptionKind.Skip:
                            FileSystem.MoveDirectory(di.FullName, destinationPath, false);
                            break;
                        case FileOverwriteOptionKind.Rewrite:
                            FileSystem.MoveDirectory(di.FullName, destinationPath, true);
                            break;
                        default:
                            break;
                    }
                }
                else if (fsi.GetType() == typeof(FileInfo))
                {
                    FileInfo fi = (FileInfo)fsi;
                    destinationPath = cbToCopy.Text + System.IO.Path.DirectorySeparatorChar + fi.Name;
                    switch (Properties.Settings.Default.FileOverwriteOption)
                    {
                        case FileOverwriteOptionKind.ShowDialog:
                            FileSystem.MoveFile(fi.FullName, destinationPath,
                                UIOption.AllDialogs, UICancelOption.DoNothing);
                            break;
                        case FileOverwriteOptionKind.Skip:
                            FileSystem.MoveFile(fi.FullName, destinationPath, false);
                            break;
                        case FileOverwriteOptionKind.Rewrite:
                            FileSystem.MoveFile(fi.FullName, destinationPath, true);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    throw new Exception();
                }

            }

            this.Close();
        }

        private void CreateVisualData()
        {
            FilePanel fpFirst = null;
            FilePanel fpSecond = null;
            if (FilePanelSelector.FirstSelected != null)
            {
                fpFirst = FilePanelSelector.FirstSelected;
            }
            else
            {
                throw new Exception("Not set FilePanelSelector.FirstSelected");
            }

            
            if (FilePanelSelector.SecondSelected != null)
            {
                fpSecond = FilePanelSelector.SecondSelected;
            }
            else
            {
                throw new Exception("Not set FilePanelSelector.SecondSelected");
            }

            List<FileSystemInfo> lstCopyFiles = fpFirst.SelectedFiles;
            string path = fpSecond.FilePanelSettings.Path;

 

            lvFromCopy.ItemsSource = lstCopyFiles;
            
            
            List<string> lstPathes = new List<string>();
            foreach(FilePanel fp in FilePanelSelector.FilePanels)
            {
                lstPathes.Add(fp.FilePanelSettings.Path);
            }
            cbToCopy.ItemsSource = lstPathes;
            cbToCopy.Text = path;
        }

    }


}
