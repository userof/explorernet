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

using System.IO;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;

namespace ExplorerNet.ViewWindowApps
{
    /// <summary>
    /// Interaction logic for FilePanel.xaml
    /// </summary>
    public partial class FilePanel : UserControl
    {

        private DirectoryInfo _currentDirectory = null;

        public FilePanel()
        {
            InitializeComponent();
            //Загружаем сохранённую ширину
            this.Width = Properties.Settings.Default.WidthFilepanel;

            _BuildDrives();

        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.Width.ToString());
        }

        /// <summary>
        /// Строем список дисков (создаём кнопки)
        /// </summary>
        private void _BuildDrives()
        {

            DriveInfo[] drives = DriveInfo.GetDrives();

            ugDrives.Children.Clear();

            foreach (var d in drives)
            {
                Button btnDrive = new Button();
                btnDrive.Content = d.Name;

                btnDrive.Click += new RoutedEventHandler(btnDrive_Click);
                btnDrive.MouseRightButtonDown += new MouseButtonEventHandler(btnDrive_MouseRightButtonDown);

                btnDrive.Tag = d;
                ugDrives.Children.Add(btnDrive);
            }
        }

        /// <summary>
        /// Происходит при событии Click на кнопку диска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDrive_Click(object sender, RoutedEventArgs e)
        {
            Button btnDrive = (Button)sender;
            DriveInfo di = (DriveInfo)btnDrive.Tag;

            if (di.IsReady)
            {
                _BuildFileSystemView(di.RootDirectory);
            }
            else
            {
                MessageBox.Show("The drive is not ready!", di.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            //dirTree.SelectedDirectory = new DirectoryInfoEx(di.RootDirectory.FullName);
        }

        /// <summary>
        /// Для вызова контекстного меню проводника для кнопки диска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDrive_MouseRightButtonDown(Object sender, MouseButtonEventArgs e)
        {
            Button btnDrive = (Button)sender;
            DriveInfo drive = (DriveInfo)btnDrive.Tag;

            DirectoryInfo dir = drive.RootDirectory;
        }

        /// <summary>
        /// Для изминения ширины файловой панели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmbMove_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            this.Width += e.HorizontalChange;
        }

        /// <summary>
        /// Самоуничтожение панели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteFilePanel_Click(object sender, RoutedEventArgs e)
        {
            Panel panel = (Panel)this.Parent;
            panel.Children.Remove(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void dirTree_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            //if (dirTree.SelectedDirectory != null)
            //{
            //    txtPath.Text = dirTree.SelectedDirectory.ToString();
            //}
        }

        /// <summary>
        /// Создаёт отражение участка файловой системы
        /// </summary>
        /// <param name="directory"></param>
        private void _BuildFileSystemView(DirectoryInfo directory)
        {
            //Очищаем список
            lvFileList.Items.Clear();

            _currentDirectory = directory;

            // Если у текущего каталога есть родительский каталог, создаём елемент, дающий возможность
            // перейти в родительский каталог
            if (directory.Parent != null)
            {
                ParentDirectoryCover cover = new ParentDirectoryCover(directory.Parent);
                lvFileList.Items.Add(cover);
            }

            // Создаём елементы отражающие директории
            foreach (var dir in directory.GetDirectories())
            {
                DirectoryCover cover = new DirectoryCover(dir);
                lvFileList.Items.Add(cover);

            }

            // Создаём елементы отражающие файлы
            foreach (var file in directory.GetFiles())
            {
                FileCover cover = new FileCover(file);
                lvFileList.Items.Add(cover);

            }

            // Задаём в текстовом поле текущий путь
            txtPath.Text = directory.FullName;
        }

        /// <summary>
        /// Срабатывает при MouseDoubleClick на елементе отражения файловой системы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvFileList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Если директория - входим в неё
            if (lvFileList.SelectedItem is DirectoryCover)
            {
                DirectoryCover cover = (DirectoryCover)lvFileList.SelectedItem;
                _BuildFileSystemView(cover.DirectoryElement);
            }
            // Если файл - запускаем
            else if (lvFileList.SelectedItem is FileCover)
            {
                FileCover cover = (FileCover)lvFileList.SelectedItem;
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.WorkingDirectory = 
                    System.IO.Path.GetDirectoryName(cover.FileElement.FullName);
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = cover.FileElement.FullName;
                process.Start();
            }
            // Если елемент родительской директории - создаём его
            else if (lvFileList.SelectedItem is ParentDirectoryCover)
            {
                ParentDirectoryCover cover = (ParentDirectoryCover)lvFileList.SelectedItem;
                _BuildFileSystemView(cover.ParentDirectoryElement);
            }


        }

        public string Path
        {
            get { return txtPath.Text; }
            set
            {
                if (Directory.Exists(value))
                {
                    _BuildFileSystemView(new DirectoryInfo(value));
                }
                else
                {
                    //Get system drive
                    DriveInfo sDrive = new DriveInfo(Environment.SystemDirectory); 
                    _BuildFileSystemView(sDrive.RootDirectory);

                }

                
            }
        }

        /// <summary>
        /// Поднимаемся на шаг вверх в иерархии файловой системы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (_currentDirectory.Parent != null)
            {
                _BuildFileSystemView(_currentDirectory.Parent);
            } 
        }




    }
}
