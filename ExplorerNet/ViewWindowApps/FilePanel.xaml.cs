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

using System.Windows.Controls.Primitives;

using System.IO;

using ExplorerNet.ViewWindowApps.FilePanelApps;
using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;

namespace ExplorerNet.ViewWindowApps
{
    /// <summary>
    /// Interaction logic for FilePanel.xaml
    /// </summary>
    public partial class FilePanel : UserControl
    {
        //private FilePanelSettings filePanelSettings = null;

        private delegate Point GetPositionDelegate(IInputElement element);

        private DirectoryInfo _currentDirectory = null;

        private static List<CustomFileSystemCover> dragList = null;

        public static FilePanel SelectedFilePanel = null;

        static FilePanel()
        {
            dragList = new List<CustomFileSystemCover>();
        }

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

        /// <summary>
        /// Возвращает или устанавлевает текущий путь
        /// </summary>
        protected string Path
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
            if (_currentDirectory != null)
            {
                if (_currentDirectory.Parent != null)
                {
                    _BuildFileSystemView(_currentDirectory.Parent);
                }
            }
        }


        #region Drag&Drop

        private void lvFileList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

                
            //if (lvFileList.SelectedItems.Count < 1) return;

            //dragList.Clear();

            //foreach(var itm in lvFileList.SelectedItems)
            //{
            //    if (itm is CustomFileSystemCover)
            //    {
            //        dragList.Add((CustomFileSystemCover)itm);
            //    }
            //}

            //DragDropEffects allowedEffects = DragDropEffects.Move;

            //if (DragDrop.DoDragDrop(this.lvFileList, dragList, allowedEffects) != DragDropEffects.None)
            //{

            //}



        }

        private void lvFileList_Drop(object sender, DragEventArgs e)
        {

            if (dragList.Count < 1) return;

            DirectoryInfo targetDir = null;

            int index = this.GetCurrentIndex(e.GetPosition);

            object target = lvFileList.Items[index];

            if (dragList[0] == target) return;

            if (target.GetType() == typeof(FileCover))
            {
                targetDir = ((FileCover)target).FileElement.Directory;
            }
            else
            {
                targetDir = ((CustomFileSystemCover)target).FileSystemElement as DirectoryInfo;
            }

            List<FileSystemInfo> files = new List<FileSystemInfo>();

            foreach(var itm in dragList)
            {
                if (!(itm is ParentDirectoryCover))
                {
                files.Add(itm.FileSystemElement);
                }
                
            }

            CopyOrMoveWindow cmw = new CopyOrMoveWindow(files, targetDir);
            cmw.ShowDialog();
        }


        /////////////////////////////////////////////////////////


        private ListViewItem GetListViewItem(int index)
        {
            if (lvFileList.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return lvFileList.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }

        private int GetCurrentIndex(GetPositionDelegate getPosition)
        {
            int index = -1;
            for (int i = 0; i < this.lvFileList.Items.Count; ++i)
            {
                ListViewItem item = GetListViewItem(i);
                if (this.IsMouseOverTarget(item, getPosition))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private bool IsMouseOverTarget(Visual target, GetPositionDelegate getPosition)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            Point mousePos = getPosition((IInputElement)target);
            return bounds.Contains(mousePos);
        }

        private void lvFileList_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                lvFileList.AllowDrop = true;
            }
            else
            {
                lvFileList.AllowDrop = false;
            }
        }

        #endregion //Drag&Drop

        private void filePanel_GotFocus(object sender, RoutedEventArgs e)
        {
            SelectedFilePanel = (FilePanel)sender;
        }

        public void DeleteFiles()
        {
            List<FileSystemInfo> list = new List<FileSystemInfo>();

            foreach (var itm in lvFileList.SelectedItems)
            {
                CustomFileSystemCover fsi = (CustomFileSystemCover)itm;
                list.Add(fsi.FileSystemElement);
            }

            DeleteWindow dw = new DeleteWindow(list);
            dw.ShowDialog();
        }


        /// <summary>
        /// Панель клонируется
        /// </summary>
        private void CloneFilePanel()
        {

            Panel panel = (Panel)this.Parent;

            FilePanel fp = new FilePanel();

            fp.Width = this.Width;
            fp.Path = this.Path;

            panel.Children.Add(fp);
        }

        private void btnClone_Click(object sender, RoutedEventArgs e)
        {
            CloneFilePanel();
        }

        private void btnWindowsExplorer_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(this.Path);
        }

        private void btnTotalCommander_Click(object sender, RoutedEventArgs e)
        {
            const string TotalCommanderPath = @"C:\Program Files\Total Commander\Totalcmd.exe";

            if (File.Exists(TotalCommanderPath))
            {
                System.Diagnostics.Process.Start(TotalCommanderPath, this.Path);
            }
            else
            {
                MessageBox.Show("File not found "+ TotalCommanderPath);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(txtPath.Text))
            {
                this.Path = txtPath.Text;
            }
            else
            {
                MessageBox.Show("Not found path " + txtPath.Text);
            }
        }

        private void SetFilePanelSettings(FilePanelSettings settings)
        {
            this.Width = settings.Width.GetValueOrDefault(Properties.Settings.Default.WidthFilepanel);

            if (settings.Path != null)
            {
                this.Path = settings.Path;
            }

            double icoWidth = settings.IcoWidth.GetValueOrDefault(Properties.Settings.Default.FilepanelIcoWidth);
            (this.lvFileList.View as GridView).Columns[0].Width = icoWidth;

            double nameWidth = settings.NameWidth.GetValueOrDefault(Properties.Settings.Default.FilepanelNameWidth);
            (this.lvFileList.View as GridView).Columns[1].Width = nameWidth;

            double sizeWidth = settings.SizeWidth.GetValueOrDefault(Properties.Settings.Default.FilepanelSizeWidth);
            (this.lvFileList.View as GridView).Columns[2].Width = sizeWidth;
        }

        private FilePanelSettings GetFilePanelSettings()
        {
            FilePanelSettings fSettings = new FilePanelSettings();
            fSettings.Width = this.Width;
            fSettings.Path = this.Path;
            fSettings.IcoWidth = (this.lvFileList.View as GridView).Columns[0].Width;
            fSettings.NameWidth = (this.lvFileList.View as GridView).Columns[1].Width;
            fSettings.SizeWidth = (this.lvFileList.View as GridView).Columns[2].Width;

            return fSettings;
        }

        public FilePanelSettings FilePanelSettings
        {
            get 
            { 
                return this.GetFilePanelSettings(); 
            }
            set 
            {
                SetFilePanelSettings(value);
                //this.filePanelSettings = value;
            }
        }

    }
}
