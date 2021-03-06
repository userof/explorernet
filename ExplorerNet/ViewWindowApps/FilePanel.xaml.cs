﻿using System;
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

using System.Collections;

using ExplorerNet.Languages;

using ExplorerNet.Tools;

using Dolinay;

using ExplorerNet.Tools.Sorting;

//using GongSolutions.Wpf.DragDrop;

namespace ExplorerNet.ViewWindowApps
{
    /// <summary>
    /// Interaction logic for FilePanel.xaml
    /// </summary>
    public partial class FilePanel : UserControl
    {
        #region Dependency properties

        public static readonly DependencyProperty IsFirstSelectedProperty =
            DependencyProperty.Register("IsFirstSelected", typeof(bool),
            typeof(FilePanel));

        public static readonly DependencyProperty IsSecondSelectedProperty =
            DependencyProperty.Register("IsSecondSelected", typeof(bool),
            typeof(FilePanel));

        public static readonly DependencyProperty UsedPreviewPanelProperty =
            DependencyProperty.Register("UsedPreviewPanel", typeof(bool),
            typeof(FilePanel));

        /// <summary>
        /// Свойство зависимостей, идентифицирующее панель первого выделение
        /// </summary>
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

        /// <summary>
        /// Свойство зависимостей, идентифицирующее панель второго выделение
        /// </summary>
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

        public bool UsedPreviewPanel
        {
            get
            {
                return (bool)GetValue(UsedPreviewPanelProperty);
            }
            set
            {
                grdPreview.IsEnabled = value;
                
                if (value)
                {
                    GridLength gl = new GridLength(200);
                    //grdPreview.Width = 100; 
                    gsMainSpliter.Visibility = System.Windows.Visibility.Visible;
                    col2.Width = gl;
                    grdPreview.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    //grdPreview.Width = 0;
                    GridLength gl = new GridLength(0);
                    gsMainSpliter.Visibility = System.Windows.Visibility.Hidden;
                    col2.Width = gl;
                    grdPreview.Visibility = System.Windows.Visibility.Hidden;
                }


                SetValue(UsedPreviewPanelProperty, value);
            }
        }

        #endregion //Dependency properties

        //Определяет, сработает ли Drop в lvFileList
        private static bool isListViewDroped = true;

        //объект наблюдения за изминением дисков
        //private DriveDetector driveDetecotr = null;

        #region Constructors
           
        static FilePanel()
        {
            dragList = new List<CustomFileSystemCover>(); 
        }

        public FilePanel()
        {
            InitializeComponent();

            UsedPreviewPanel = false;
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

            //this.lvFileList.DragEnter += 

            //Построения кнопок дисков
            _BuildDrives();

            //Наблюдаем за появлением нового устройства. При появлении обновляем список дисков
            //driveDetecotr = DriveDetector.
            DriveDetectorSing.DriveDetectorProp.DeviceArrived += delegate(object sender, 
                DriveDetectorEventArgs e)
            {
                _BuildDrives();
            };
            DriveDetectorSing.DriveDetectorProp.DeviceRemoved += delegate(object sender, 
                DriveDetectorEventArgs e)
            {
                _BuildDrives();
            };

        }

        #endregion //Constructors

        #region Drag&Drop


        #endregion

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
                //Button btnDrive = new Button();
                DriveButton btnDrive = new DriveButton(d);

                //btnDrive.Content = d.Name[0].ToString();
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
            DriveButton btnDrive = (DriveButton)sender;
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
            DriveButton btnDrive = (DriveButton)sender;
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
            //lvFileList.Items.Clear();
            lvFileList.ItemsSource = null;


            _currentDirectory = directory;

            List<CustomFileSystemCover> list = new List<CustomFileSystemCover>();

            //CustomFileSystemCover cover = null;

            //lvFileList.ItemContainerGenerator.StatusChanged +=
            //        delegate(Object sender, EventArgs e)
            //        {
            //            ListViewItem lvi = (ListViewItem)lvFileList.ItemContainerGenerator.ContainerFromItem(cover);
            //            if (lvi != null)
            //            {
            //                lvi.PreviewMouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
            //                {
            //                    DragDrop.do
            //                }
            //            }

            //        };

            // Если у текущего каталога есть родительский каталог, создаём елемент, дающий возможность
            // перейти в родительский каталог
            if (directory.Parent != null)
            {
                ParentDirectoryCover cover = new ParentDirectoryCover(directory.Parent);
                list.Add(cover);
                //lvFileList.Items.Add(cover);
                
            }

            // Создаём елементы отражающие директории
            try
            {
                foreach (var dir in directory.GetDirectories())
                {
                    DirectoryCover cover = new DirectoryCover(dir);

                    list.Add(cover);

                    #region comments Этот вариант был исправлен на более правельный

                    //Получаем ListViewItem, когда он будет создан и подписываемся на его события
                    //lvFileList.ItemContainerGenerator.StatusChanged +=
                    //    delegate(Object sender, EventArgs e)
                    //    {
                    //        ListViewItem lvi = (ListViewItem)lvFileList.ItemContainerGenerator.ContainerFromItem(cover);
                    //        if (lvi != null)
                    //        {
                    //            lvi.AllowDrop = true;

                    //            //Список событий. Сделан чтобы не подписываться много раз на одно событие
                    //            List<string> eventList = null;
                    //            if (lvi.Tag == null)
                    //            {
                    //                eventList = new List<string>();
                    //                lvi.Tag = eventList;
                    //            }
                    //            else
                    //            {
                    //                eventList = (List<string>)lvi.Tag;
                    //            }

                    //Если события нет в списке, то подписываемся на него
                    //if (!eventList.Contains("DragEnter"))
                    //{
                    //    eventList.Add("DragEnter");
                    //    lvi.DragEnter += delegate(Object sender1, DragEventArgs e1)
                    //    {
                    //        if (e1.Effects == DragDropEffects.Move)
                    //        {
                    //            lvi.Opacity = 0.5;
                    //        }
                    //    };
                    //}

                    //Если события нет в списке, то подписываемся на него
                    //if (!eventList.Contains("DragLeave"))
                    //{
                    //    eventList.Add("DragLeave");
                    //    lvi.DragLeave += delegate(Object sender1, DragEventArgs e1)
                    //    {
                    //        lvi.Opacity = 1;
                    //    };
                    //}

                    //Если события нет в списке, то подписываемся на него
                    //if (!eventList.Contains("Drop"))
                    //{
                    //    eventList.Add("Drop");
                    //    lvi.Drop += delegate(Object sender1, DragEventArgs e1)
                    //    {
                    //        lvi.Opacity = 1;
                    //        DataObject dObj = (DataObject)e1.Data;

                    //        //Делаем не возможным обрабатывать Drop lvFileList
                    //        this.isListViewDroped = false;

                    //        if (dObj.GetDataPresent(typeof(List<CustomFileSystemCover>)))
                    //        {
                    //            // If the desired data format is present, use one of the GetData methods to retrieve the
                    //            // data from the data object.
                    //            List<CustomFileSystemCover> selectedList = dObj.GetData(typeof(List<CustomFileSystemCover>)) 
                    //                as List<CustomFileSystemCover>;

                    //            //MessageBox.Show(selectedList[0].Name);
                    //            List<FileSystemInfo> fsiList = new List<FileSystemInfo>();

                    //            foreach (var sl in selectedList)
                    //            {
                    //                if (sl.FileSystemElement.GetType() == typeof(DirectoryInfo))
                    //                {
                    //                    fsiList.Add(new DirectoryInfo(sl.FileSystemElement.FullName));
                    //                }
                    //                else if (sl.FileSystemElement.GetType() == typeof(FileInfo))
                    //                {
                    //                    fsiList.Add(new FileInfo(sl.FileSystemElement.FullName));
                    //                }
                    //                else
                    //                {
                    //                    new Exception("Type not support!");
                    //                }
                    //            }

                    //            DirectoryCover dc = (DirectoryCover)lvi.Content;


                    //            CopyWindow cw = new CopyWindow(fsiList, dc.FileSystemElement.FullName);
                    //            cw.ShowDialog();

                    //        }

                    //        //DragDrop.RemovePreviewDropHandler(lvFileList, lvFileList_Drop);
                    //    };
                    //}

                    //}
                    //};
                    #endregion //comments
                }
            }
            catch (Exception)
            {
                MessageBox.Show(LanguagesManager.GetCurrLanguage().FPIsNotAccess, "", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Path = System.IO.Path.GetDirectoryName(this.Path);
                return;
            }
            
            // Создаём елементы отражающие файлы
            foreach (var file in directory.GetFiles())
            {
                FileCover cover = new FileCover(file);

                list.Add(cover);
                //lvFileList.Items.Add(cover);

            }

            //list.Sort(new NameUpSorter()); 
            SortingManager.Sort(list, SortingKind.NameUp);

            lvFileList.ItemsSource = list;

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
            Navigate();
        }

        private void Navigate()
        {
            // Если директория - входим в неё
            if (lvFileList.SelectedItem is DirectoryCover)
            {
                //ListViewItem lvi = (ListViewItem)lvFileList.SelectedItem;
                //DirectoryCover cover = (DirectoryCover)lvi.Content;

                DirectoryCover cover = (DirectoryCover)lvFileList.SelectedItem;
                _BuildFileSystemView(cover.DirectoryElement);
            }
            // Если файл - запускаем
            else if (lvFileList.SelectedItem is FileCover)
            {
                FileCover cover = (FileCover)lvFileList.SelectedItem;
                //System.Diagnostics.Process process = new System.Diagnostics.Process();
                //process.StartInfo.WorkingDirectory =
                //    System.IO.Path.GetDirectoryName(cover.FileElement.FullName);
                //process.StartInfo.UseShellExecute = true;
                //process.StartInfo.FileName = cover.FileElement.FullName;
                //process.Start();

                //ExplorerNet.Tools.LastStartedFiles.LastStartedFilesManager lsfm = 
                //    new Tools.LastStartedFiles.LastStartedFilesManager();
                //lsfm.AddlastStartedFile(cover.FileElement.FullName);
                ExplorerNet.Tools.FileStarter.Start(cover.FileElement.FullName);


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

            this.UsedPreviewPanel = settings.UsedPreviewPanel;
            //this.grdPreview.Width = settings.PreviewPanelWidth;
            this.col2.Width = new GridLength(settings.PreviewPanelWidth);

            double starWidth = settings.StarWidth.GetValueOrDefault(Properties.Settings.Default.FilepanelStarWidth);
            (this.lvFileList.View as GridView).Columns[0].Width = starWidth;

            double icoWidth = settings.IcoWidth.GetValueOrDefault(Properties.Settings.Default.FilepanelIcoWidth);
            (this.lvFileList.View as GridView).Columns[1].Width = icoWidth;

            double nameWidth = settings.NameWidth.GetValueOrDefault(Properties.Settings.Default.FilepanelNameWidth);
            (this.lvFileList.View as GridView).Columns[2].Width = nameWidth;

            double sizeWidth = settings.SizeWidth.GetValueOrDefault(Properties.Settings.Default.FilepanelSizeWidth);
            (this.lvFileList.View as GridView).Columns[3].Width = sizeWidth;
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
            fSettings.StarWidth = (this.lvFileList.View as GridView).Columns[0].Width;
            fSettings.IcoWidth = (this.lvFileList.View as GridView).Columns[1].Width;
            fSettings.NameWidth = (this.lvFileList.View as GridView).Columns[2].Width;
            fSettings.SizeWidth = (this.lvFileList.View as GridView).Columns[3].Width;

            fSettings.UsedPreviewPanel = this.UsedPreviewPanel;
            //fSettings.PreviewPanelWidth = this.grdPreview.Width;
            fSettings.PreviewPanelWidth = this.col2.Width.Value;

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

        private void btnCopyDialog_Click(object sender, RoutedEventArgs e)
        {
            CopyWindow cw = new CopyWindow();
            cw.ShowDialog();

            //GongSolutions.Wpf.DragDrop.Utilities.

        }


        private void lvFileList_Drop(object sender, DragEventArgs e)
        {

            if (isListViewDroped)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                    List<FileSystemInfo> fsiList = new List<FileSystemInfo>();
                    foreach (var f in files)
                    {
                        FileSystemInfo fsi = null;

                        if (Directory.Exists(f))
                        {
                            fsi = new DirectoryInfo(f);
                        }
                        else if (File.Exists(f))
                        {
                            fsi = new FileInfo(f);
                        }
                        else
                        {
                            throw new Exception("Its not a directory and not a file!");
                        }

                        fsiList.Add(fsi);
                    }


                    CopyWindow cw = new CopyWindow(fsiList, this.Path);
                    cw.ShowDialog();
                }

                //DataObject dObj = (DataObject)e.Data;

                //if (dObj.GetDataPresent(typeof(List<CustomFileSystemCover>)))
                //{
                //    // If the desired data format is present, use one of the GetData methods to retrieve the
                //    // data from the data object.
                //    List<CustomFileSystemCover> selectedList = dObj.GetData(typeof(List<CustomFileSystemCover>))
                //        as List<CustomFileSystemCover>;

                //    //MessageBox.Show(selectedList[0].Name);
                //    List<FileSystemInfo> fsiList = new List<FileSystemInfo>();

                //    foreach (var sl in selectedList)
                //    {
                //        if (sl.FileSystemElement.GetType() == typeof(DirectoryInfo))
                //        {
                //            fsiList.Add(new DirectoryInfo(sl.FileSystemElement.FullName));
                //        }
                //        else if (sl.FileSystemElement.GetType() == typeof(FileInfo))
                //        {
                //            fsiList.Add(new FileInfo(sl.FileSystemElement.FullName));
                //        }
                //        else
                //        {
                //            new Exception("Type not support!");
                //        }
                //    }

                //    CopyWindow cw = new CopyWindow(fsiList, this.Path);
                //    cw.ShowDialog();

                //}
            }

            //DataObject dObj = (DataObject)e.Data;

            //if (dObj.GetDataPresent(typeof(List<CustomFileSystemCover>)))
            //{
            //    // If the desired data format is present, use one of the GetData methods to retrieve the
            //    // data from the data object.
            //    List<CustomFileSystemCover> selectedList = dObj.GetData(typeof(List<CustomFileSystemCover>))
            //        as List<CustomFileSystemCover>;

            //    MessageBox.Show(selectedList[0].Name + " lv");

            //    selectedList = null;
            //}
        }

        private void lvFileList_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance && 
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                //dropPoint = e.GetPosition(this);
                //if ((Keyboard.IsKeyDown(Key.LeftShift)) || (Keyboard.IsKeyDown(Key.RightShift)))
                {
                    //Делаем возможным lvFileList обрабатывать Drop
                    isListViewDroped = true;

                    List<CustomFileSystemCover> selectedList = new List<CustomFileSystemCover>();

                    foreach (var itm in lvFileList.SelectedItems)
                    {
                        selectedList.Add((CustomFileSystemCover)itm);
                    }

                    if (selectedList.Count > 0)
                    {

                        //DataObject dObj = new DataObject();
                        //dObj.SetData(selectedList.GetType(), selectedList);

                        List<string> list = new List<string>();
                        foreach (var sl in selectedList)
                        {
                            list.Add(sl.FileSystemElement.FullName);
                        }

                        string[] files = list.ToArray();
                        DataObject dObj = new DataObject(DataFormats.FileDrop, files);
                        //dropEnable = true;
                        DragDrop.DoDragDrop(lvFileList, dObj, DragDropEffects.Move);
                        //DragDrop.AddDropHandler
                        //DragDrop.DoDragDrop(lvFileList, selectedList, DragDropEffects.Move);
                    }
                }
            

            }
        }

        private void ListViewItem_DragEnter(Object sender, DragEventArgs e)
        {
            ListViewItem lvi = (ListViewItem)sender;

            if (lvi.Content != null)
            {
                object content = lvi.Content;
                if (content.GetType() == typeof(DirectoryCover))
                {
                    //if (e.Effects == DragDropEffects.Move)
                    //{
                        lvi.Opacity = 0.5;
                    //}
                }
            }
        }

        private void ListViewItem_DragLeave(Object sender, DragEventArgs e)
        {
            ListViewItem lvi = (ListViewItem)sender;

            if (lvi.Content != null)
            {
                object content = lvi.Content;
                if (content.GetType() == typeof(DirectoryCover))
                {
                    //if (e.Effects == DragDropEffects.Move)
                    //{
                        lvi.Opacity = 1;
                    //}
                }
            }
        }

        //private static Point dropPoint;

        private void ListViewItem_Drop(Object sender, DragEventArgs e)
        {
           // var p = e.GetPosition(this);

           // int mov = 55;
           // bool xM = p.X > dropPoint.X - mov;
           // bool xB = p.X < dropPoint.X + mov;
           // bool yM = p.Y > dropPoint.Y - mov;
           // bool yB = p.Y < dropPoint.Y + mov;

           // bool rt = (xM && xB && yM && yB);

           // if (rt)  return;

           //SystemParameters.mi

            ListViewItem lvi = (ListViewItem)sender;
            if (lvi.Content != null)
            {
                object content = lvi.Content;
                lvi.Opacity = 1;
                
                //var fff = e.Data.GetDataPresent(DataFormats.FileDrop);

                if (content.GetType() == typeof(DirectoryCover))
                {
                    if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    {
                        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                        List<FileSystemInfo> fsiList = new List<FileSystemInfo>();
                        foreach (var f in files)
                        {
                            FileSystemInfo fsi = null;

                            if (Directory.Exists(f))
                            {
                                fsi = new DirectoryInfo(f);
                            }
                            else if (File.Exists(f))
                            {
                                fsi = new FileInfo(f);
                            }
                            else
                            {
                                throw new Exception("Its not a directory and not a file!");
                            }

                            fsiList.Add(fsi);
                        }
                        isListViewDroped = false;
                        DirectoryCover dc = (DirectoryCover)lvi.Content;

                        CopyWindow cw = new CopyWindow(fsiList, dc.FileSystemElement.FullName);
                        cw.ShowDialog();
                        //string[] files = (string[])dObj.GetData(DataFormats.FileDrop);
                        // MessageBox.Show(files.ToString());
                    }
                }
                
            }



            //ListViewItem lvi = (ListViewItem)sender;

            //if (lvi.Content != null)
            //{
            //    object content = lvi.Content;
            //    if (content.GetType() == typeof(DirectoryCover))
            //    {
            //        if (e.Effects == DragDropEffects.Move)
            //        {
            //            lvi.Opacity = 1;
            //            DataObject dObj = (DataObject)e.Data;

            //            //Делаем не возможным обрабатывать Drop lvFileList
            //            this.isListViewDroped = false;

            //            if (dObj.GetDataPresent(typeof(List<CustomFileSystemCover>)))
            //            {
            //                // If the desired data format is present, use one of the GetData methods to retrieve the
            //                // data from the data object.
            //                List<CustomFileSystemCover> selectedList = dObj.GetData(typeof(List<CustomFileSystemCover>))
            //                    as List<CustomFileSystemCover>;

            //                //MessageBox.Show(selectedList[0].Name);
            //                List<FileSystemInfo> fsiList = new List<FileSystemInfo>();

            //                foreach (var sl in selectedList)
            //                {
            //                    if (sl.FileSystemElement.GetType() == typeof(DirectoryInfo))
            //                    {
            //                        fsiList.Add(new DirectoryInfo(sl.FileSystemElement.FullName));
            //                    }
            //                    else if (sl.FileSystemElement.GetType() == typeof(FileInfo))
            //                    {
            //                        fsiList.Add(new FileInfo(sl.FileSystemElement.FullName));
            //                    }
            //                    else
            //                    {
            //                        new Exception("Type not support!");
            //                    }
            //                }
            //                DirectoryCover dc = (DirectoryCover)lvi.Content;


            //                CopyWindow cw = new CopyWindow(fsiList, dc.FileSystemElement.FullName);
            //                cw.ShowDialog();
            //            }
            //        }
            //    }
            //}
        }

        private void lvFileList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Navigate();
            }
        }

        private void StarView_Click(Object sender, EventArgs e)
        {
            StarView starView = sender as StarView;
            CustomFileSystemCover cover = starView.Tag as CustomFileSystemCover;
            cover.Star = starView.StarLevel;
        }

        private void NoteView_Click(object sender, EventArgs e)
        {
            NoteView noteView = sender as NoteView;
            CustomFileSystemCover cover = noteView.Tag as CustomFileSystemCover;
            ExplorerNet.MVVM.View.NoteWindow nw = new MVVM.View.NoteWindow();
            nw.txtDescription.Text = noteView.Description;

            Point p = noteView.PointToScreen(new Point());
            nw.Top = p.Y;
            nw.Left = p.X;
            // отобжараем окно заметок
            //nw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            nw.ShowDialog();

            if (nw.DialogAnswer == true)
            {
                if (string.IsNullOrEmpty(nw.txtDescription.Text))
                {
                    cover.Description = null;
                }
                else
                {
                    cover.Description = nw.txtDescription.Text;

                }
                noteView.Description = cover.Description;
            }

            
            //noteView.so
 
        }

        private void lvFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.UsedPreviewPanel)
            {
                List<CustomFileSystemCover> list = new List<CustomFileSystemCover>();

                foreach (var it in lvFileList.SelectedItems)
                {
                    list.Add(it as CustomFileSystemCover);
                }

                ppPreview.PreviewElementStart(list);
            }



            
            //ppPreview.ShowPreview(list);
            //SelectWatcher.Instance.Change(list, this);

        }

        private void btnPreviewPanel_Click(object sender, RoutedEventArgs e)
        {
            UsedPreviewPanel = !UsedPreviewPanel;
        }

        private Point startPoint;

        private void lvFileList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void txtPath_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtPath.SelectAll();
        }

        //private static bool dropEnable = false;

        //private void lvFileList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    DragDrop.rem
        //}

        //public void ChangeSelected(List<CustomFileSystemCover> files,
        //    FilePanel activeFilePanel)
        //{

        //}


        //private void StarView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var i = lvFileList.ItemContainerGenerator.ContainerFromItem((DependencyObject)sender);
        //}



    }

}
