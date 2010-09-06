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
using Microsoft.VisualBasic.FileIO;

using ExplorerNet.CopyOrMoveWindowApps;


namespace ExplorerNet
{
    /// <summary>
    /// Interaction logic for CopyOrMoveWindow.xaml
    /// </summary>
    public partial class CopyOrMoveWindow : Window
    {
        //private List<FileSystemInfo> sourceList = null;

        //private DirectoryInfo targetDir = null;

        private FromToFileSystemInfoes sourceAndTargetFSIList = null;

        protected CopyOrMoveWindow()
        {
            InitializeComponent();
        }

        private void Copy()
        {
            foreach (var ffsi in sourceAndTargetFSIList)
            {
                lvMain.SelectedItem = ffsi;

                
                if (ffsi.FromPath.GetType() == typeof(DirectoryInfo))
                {
                    switch (Properties.Settings.Default.FileOverwriteOption)
                    {
                        case FileOverwriteOptionKind.ShowDialog:
                            FileSystem.CopyDirectory(ffsi.FromPathStr, ffsi.NewPathStr, 
                                UIOption.AllDialogs, UICancelOption.DoNothing);
                            break;
                        case FileOverwriteOptionKind.Skip:
                            FileSystem.CopyDirectory(ffsi.FromPathStr, ffsi.NewPathStr, false);
                            break;
                        case FileOverwriteOptionKind.Rewrite:
                            FileSystem.CopyDirectory(ffsi.FromPathStr, ffsi.NewPathStr, true);
                            break;
                        default:
                            break;
                    }
                }
                else if (ffsi.FromPath.GetType() == typeof(FileInfo))
                {
                    switch (Properties.Settings.Default.FileOverwriteOption)
                    {
                        case FileOverwriteOptionKind.ShowDialog:
                            FileSystem.CopyFile(ffsi.FromPathStr, ffsi.NewPathStr,
                                UIOption.AllDialogs, UICancelOption.DoNothing);
                            break;
                        case FileOverwriteOptionKind.Skip:
                            FileSystem.CopyFile(ffsi.FromPathStr, ffsi.NewPathStr, false);
                            break;
                        case FileOverwriteOptionKind.Rewrite:
                            FileSystem.CopyFile(ffsi.FromPathStr, ffsi.NewPathStr, true);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    throw new Exception("Uncnoun type!");
                }
            }
        }

        private void Move()
        {
            foreach (var ffsi in sourceAndTargetFSIList)
            {
                lvMain.SelectedItem = ffsi;


                if (ffsi.FromPath.GetType() == typeof(DirectoryInfo))
                {
                    switch (Properties.Settings.Default.FileOverwriteOption)
                    {
                        case FileOverwriteOptionKind.ShowDialog:
                            FileSystem.MoveDirectory(ffsi.FromPathStr, ffsi.NewPathStr,
                                UIOption.AllDialogs, UICancelOption.DoNothing);
                            break;
                        case FileOverwriteOptionKind.Skip:
                            FileSystem.MoveDirectory(ffsi.FromPathStr, ffsi.NewPathStr, false);
                            break;
                        case FileOverwriteOptionKind.Rewrite:
                            FileSystem.MoveDirectory(ffsi.FromPathStr, ffsi.NewPathStr, true);
                            break;
                        default:
                            break;
                    }
                }
                else if (ffsi.FromPath.GetType() == typeof(FileInfo))
                {
                    switch (Properties.Settings.Default.FileOverwriteOption)
                    {
                        case FileOverwriteOptionKind.ShowDialog:
                            FileSystem.MoveFile(ffsi.FromPathStr, ffsi.NewPathStr,
                                UIOption.AllDialogs, UICancelOption.DoNothing);
                            break;
                        case FileOverwriteOptionKind.Skip:
                            FileSystem.MoveFile(ffsi.FromPathStr, ffsi.NewPathStr, false);
                            break;
                        case FileOverwriteOptionKind.Rewrite:
                            FileSystem.MoveFile(ffsi.FromPathStr, ffsi.NewPathStr, true);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    throw new Exception("Uncnoun type!");
                }

            }

        }

        public CopyOrMoveWindow(List<FileSystemInfo> sourceList, DirectoryInfo toCopy)
            : this()
        {
            sourceAndTargetFSIList = new FromToFileSystemInfoes(sourceList, toCopy);

            //this.sourceList = sourceList;
            //this.targetDir = targetDir;

            lvMain.ItemsSource = sourceAndTargetFSIList;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            this.Copy();
            this.Close();
        }

        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            this.Move();
            this.Close();
        }
    }
}
