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
using System.Threading;
using System.Windows.Threading;

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

        public static readonly DependencyProperty IsFirstSelectedProperty =
            DependencyProperty.Register("IsFirstSelected", typeof(bool),
            typeof(FilePanel));

        public static readonly DependencyProperty IsSecondSelectedProperty =
            DependencyProperty.Register("IsSecondSelected", typeof(bool),
            typeof(FilePanel));

        public bool IsFirstSelected
        {
            get
            {
                return (bool)GetValue(IsFirstSelectedProperty);
            }
            set
            {
                SetValue(IsFirstSelectedProperty, value);
            }
        }

        public bool IsSecondSelected
        {
            get
            {
                return (bool)GetValue(IsSecondSelectedProperty);
            }
            set
            {
                SetValue(IsSecondSelectedProperty, value);
            }
        }

        private delegate Point GetPositionDelegate(IInputElement element);

        /// <summary>
        /// Текущая директория файловой панели
        /// </summary>
        private DirectoryInfo _currentDirectory = null;

        /// <summary>
        /// Объект для наблюдения в изменении файловой системы
        /// </summary>
        private FileSystemWatcher watcher = null;

        private static List<CustomFileSystemCover> dragList = null;

        /// <summary>
        /// Является ли данная файловая панель первой выделенной?
        /// </summary>
        //private bool isFirstSelected = false;

        /// <summary>
        /// Является ли данная файловая панель второй  выделенной?
        /// </summary>
        //private bool isSecondSelected = false;

        //public static FilePanel SelectedFilePanel = null;

        //public static FilePanel SecondSelectedFilePanel = null;

        static FilePanel()
        {
            dragList = new List<CustomFileSystemCover>();
        }

        public FilePanel()
        {
            InitializeComponent();
            //Загружаем сохранённую ширину
            this.Width = Properties.Settings.Default.WidthFilepanel;

            //каждаю новая файловая панель стаёт выделенной
            //SelectedFilePanel = this;

            //btnMakeDirectory.ToolTip = "Make a directory (Ctrl + D)";

            //Создаём элемент наблюдающий за изменениями файловой системы
            this.watcher = new FileSystemWatcher();
            //События, на которые будет происходить обновления отображения файлов
            this.watcher.Created += new FileSystemEventHandler(watcher_Changed);
            this.watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            this.watcher.Deleted += new FileSystemEventHandler(watcher_Changed);
            //this.watcher.EnableRaisingEvents = true;

            //Построения кнопок дисков
            _BuildDrives();

        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            //Происходить обновления отображения файлов
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
            {
                _BuildFileSystemView(this._currentDirectory);
            });

        }

        /// <summary>
        /// Обработчик события. Происходит, когда в классе FilePanelSelector 
        /// изменяются выбранные панели
        /// </summary>
        private void FilePanelSelector_FilePanelChangeSelect()
        {
            //Изменяем цвет элементы, сигнализирую о выделении
            //this.rctSelected.Fill = Brushes.Black;
            // Если панель имеет степень выделения, это отражается на полях
            // isFirstSelected или isSecondSelected
            if (FilePanelSelector.FirstSelected == this)
            {
                this.IsFirstSelected = true;

                this.IsSecondSelected = false;
                //this.isFirstSelected = true;
                //Изменяем цвет элементы, сигнализирую о выделении
                //this.rctSelected.Fill = Brushes.YellowGreen;
            }
            else if (FilePanelSelector.SecondSelected == this)
            {
                this.IsFirstSelected = false;

                this.IsSecondSelected = true;
                //Изменяем цвет элементы, сигнализирую о выделении
                //this.rctSelected.Fill = Brushes.Green;
            }

            else
            {
                this.IsSecondSelected = false;
                this.IsFirstSelected = false;
            }
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

            //Добавляем кнопку доступа к системным папкам
            SystemFoldersButton sfb = new SystemFoldersButton();
            sfb.GoToFolder += delegate(object sender, string path)
            {
                this.Path = path;
            };
            ugDrives.Children.Add(sfb);

            foreach (var d in drives)
            {
                Button btnDrive = new Button();
                btnDrive.Content = d.Name[0].ToString();
                //Создаём подсказку для диска
                btnDrive.ToolTip = new DriveHint(d);
                ToolTipService.SetBetweenShowDelay(btnDrive, 20000);

                btnDrive.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(btnDrive_PreviewMouseLeftButtonDown);

                btnDrive.MouseRightButtonDown += new MouseButtonEventHandler(btnDrive_MouseRightButtonDown);
                //Сам экземпляр диска держим в теге
                btnDrive.Tag = d;
                ugDrives.Children.Add(btnDrive);
            }
        }

        /// <summary>
        /// Обработчик щелчка мыши по кнопке диска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDrive_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button btnDrive = (Button)sender;
            DriveInfo di = (DriveInfo)btnDrive.Tag;

            if (di.IsReady)
            {
                //Если диск готов, отображаем список файлов
                _BuildFileSystemView(di.RootDirectory);
            }
            else
            {
                //Если нет, предупреждающее сообщение
                MessageBox.Show("The drive is not ready!", di.Name, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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


            System.Drawing.Point dPoint = new System.Drawing.Point();

            Point point = btnDrive.PointToScreen(new Point());

            dPoint.X = Convert.ToInt32(point.X);
            dPoint.Y = Convert.ToInt32(point.Y);

            ExplorerNet.Tools.ShellContextMenu scm = new Tools.ShellContextMenu();
            scm.ShowContextMenu(drive, dPoint);

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

            this.watcher.Path = directory.FullName;
            //this.watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            this.watcher.EnableRaisingEvents = true;
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

            foreach (var itm in dragList)
            {
                if (!(itm is ParentDirectoryCover))
                {
                    files.Add(itm.FileSystemElement);
                }

            }

            //CopyOrMoveWindow cmw = new CopyOrMoveWindow(files, targetDir);
            //cmw.ShowDialog();
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
            //SecondSelectedFilePanel = SelectedFilePanel;
            //SelectedFilePanel = (FilePanel)sender;

            //Добавляем нашу панель в коллекцию класса FilePanelSelector
            FilePanelSelector.Add(this);
        }

        //Метод, вызывает диалог удаления файла
        public void DeleteFilesDialog()
        {
            List<FileSystemInfo> list = new List<FileSystemInfo>();

            foreach (var itm in lvFileList.SelectedItems)
            {
                if (itm.GetType() != typeof(ParentDirectoryCover))
                {
                    CustomFileSystemCover fsi = (CustomFileSystemCover)itm;
                    list.Add(fsi.FileSystemElement);
                }
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
            fp.FilePanelSettings = (FilePanelSettings)this.FilePanelSettings.Clone();
            panel.Children.Add(fp);
        }

        /// <summary>
        /// Обработчик события, клонирует панель
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClone_Click(object sender, RoutedEventArgs e)
        {
            CloneFilePanel();
        }

        /// <summary>
        /// Обработчик события, запускает проводник и открывает в нём текущий путь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWindowsExplorer_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(this.Path);
        }

        /// <summary>
        /// Обработчик события, запускает TotalCommander и открывает в нём текущий путь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTotalCommander_Click(object sender, RoutedEventArgs e)
        {
            const string TotalCommanderPath = @"C:\Program Files\Total Commander\Totalcmd.exe";

            if (File.Exists(TotalCommanderPath))
            {
                System.Diagnostics.Process.Start(TotalCommanderPath, this.Path);
            }
            else
            {
                MessageBox.Show("File not found " + TotalCommanderPath);
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

        /// <summary>
        /// Задаёт настройки текущей файловой панели
        /// </summary>
        /// <param name="settings"></param>
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

        /// <summary>
        /// получает настройки текущей файловой панели
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Задаёт и получает настройки текущей файловой панели
        /// </summary>
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

        /// <summary>
        /// Обработчик события, создаёт контекстное меню проводника для выделенных элементов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvFileList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListView lstFiles = (ListView)sender;
            var selItems = lstFiles.SelectedItems;

            //List<System.IO.FileSystemInfoEx> list = new List<FileSystemInfoEx>(); 
            List<FileSystemInfo> list = new List<FileSystemInfo>();


           // Point mousePoint = e.GetPosition(lstFiles);
           // lstFiles.
 

            foreach (var itm in selItems)
            {
                CustomFileSystemCover cc = (CustomFileSystemCover)itm;

                list.Add(cc.FileSystemElement);

            }

            System.Drawing.Point dPoint = new System.Drawing.Point();
            Point point = lstFiles.PointToScreen(new Point());


            dPoint.X = Convert.ToInt32(point.X);
            dPoint.Y = Convert.ToInt32(point.Y);
            //System.IO.Tools.ContextMenuWrapper cmw = new System.IO.Tools.ContextMenuWrapper();
            //cmw.Popup(list.ToArray(), dPoint);

            ExplorerNet.Tools.ShellContextMenu scm = new Tools.ShellContextMenu();
            scm.ShowContextMenu(list.ToArray(), dPoint);
        }

        /// <summary>
        /// Создаёт директорию 
        /// </summary>
        /// <param name="directoryName"></param>
        private void MakeNewDirectory(string directoryName)
        {
            Directory.CreateDirectory(this.Path + System.IO.Path.DirectorySeparatorChar + directoryName);
        }

        /// <summary>
        /// Вызывает диалог создания директории
        /// </summary>
        public void MakeNewDirectoryDialog()
        {
            CreateFolderWindow cfw = new CreateFolderWindow(this.FilePanelSettings.Path);
            cfw.ShowDialog();
            MakeNewDirectory(cfw.DirectoryName);
            cfw.Close();
        }

        /// <summary>
        /// Обработчик события, вызывает диалог создания директории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMakeDirectory_Click(object sender, RoutedEventArgs e)
        {
            this.MakeNewDirectoryDialog();
        }

        /// <summary>
        /// Обработчик события, удаляет файловую панель со списка класса FilePanelSelector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filePanel_Unloaded(object sender, RoutedEventArgs e)
        {
            FilePanelSelector.Remove(this);
        }

        /// <summary>
        /// Обработчик события, регистрирует события изменения выделения и 
        /// добавляет текущую файловую панель в списoк класса FilePanelSelector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filePanel_Loaded(object sender, RoutedEventArgs e)
        {
            FilePanelSelector.FilePanelChangeSelect += new FilePanelSelector.FilePanelChangeSelectEventHandler(FilePanelSelector_FilePanelChangeSelect);
            FilePanelSelector.Add(this);
        }

        /// <summary>
        /// Является ли данная файловая панель первой выделенной?
        /// </summary>
        //public bool IsFirstSelected
        //{
        //    get { return this.isFirstSelected; }
        //}

        /// <summary>
        /// Является ли данная файловая панель второй  выделенной?
        /// </summary>
        //public bool IsSecondSelected
        //{
        //    get { return this.isSecondSelected; }
        //}

        public List<FileSystemInfo> SelectedFiles
        {
            get 
            {
                List<FileSystemInfo> result = new List<FileSystemInfo>();
                foreach (var si in lvFileList.SelectedItems)
                {
                    if (si.GetType() != typeof(ParentDirectoryCover))
                    {
                        CustomFileSystemCover cfsc = (CustomFileSystemCover)si;
                        result.Add(cfsc.FileSystemElement);
                    }
                }
                return result;
                
            }
        }

        private void btnCMD_Click(object sender, RoutedEventArgs e)
        {

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.WorkingDirectory = this.Path;
            proc.StartInfo = psi;
            proc.Start();
            
            
        }

        private void filePanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //this.IsChecked = true;
        }

        private void filePanel_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(IsChecked.ToString());
        }
    }

}
