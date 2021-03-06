﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using ExplorerNet.Tools;
using ExplorerNet.Tools.RecentTemplateNS;
using ExplorerNet.Tools.Wallpapers;
using ExplorerNet.ViewWindowApps;
using ExplorerNet.ViewWindowApps.Templates;

using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

using ExplorerNet.Tools.LastStartedFiles;

namespace ExplorerNet
{
    /// <summary>
    /// Основное рабочее окно. 
    /// Оно даёт возможность просматривать структуру файловой системы 
    /// и начать операции с файлами
    /// </summary>
    public partial class Shell : Window
    {
        /// <summary>
        /// Текущий шаблон окна
        /// </summary>
        private ViewWindowTemplate currentTemplate = null;

        public static RoutedCommand DeleteFilesCommand = new RoutedCommand();

        public static RoutedCommand MakeDirCommand = new RoutedCommand();

        public static RoutedCommand CopyCommand = new RoutedCommand();

        public static RoutedCommand RenameCommand = new RoutedCommand();

        public Shell()
        {
            InitializeComponent();


            ////////////////////////
            //LastStartedFiles target = new LastStartedFiles(); // TODO: инициализация подходящего значения

            //LastStartedFile lf = new LastStartedFile("C:\\1.txt");
            //LastStartedFile lf2 = new LastStartedFile("C:\\2.txt");
            //LastStartedFile lf3 = new LastStartedFile("C:\\3.txt");
            //target.Add(lf);
            //target.Add(lf2);
            //target.Add(lf3);

            //target.Save();
            //LastStartedFiles target2 = LastStartedFiles.Load();
            ////////////////////////////
 
            //Создание команды и сочетания клавиш для удаления
            CommandBinding cbDelete = new CommandBinding(DeleteFilesCommand, ExecutedDeleteFilesCommand);
            this.CommandBindings.Add(cbDelete);

            KeyGesture kgDel = new KeyGesture(Key.Delete);
            KeyBinding kbDel = new KeyBinding(DeleteFilesCommand, kgDel);
            this.InputBindings.Add(kbDel);

            KeyGesture kgF8 = new KeyGesture(Key.F8);
            KeyBinding kbF8 = new KeyBinding(DeleteFilesCommand, kgF8);
            this.InputBindings.Add(kbF8);

            //Создание команды и сочетания клавиш для создания директории
            CommandBinding cbMakeDir = new CommandBinding(MakeDirCommand, ExecutedMakeDirCommand);
            this.CommandBindings.Add(cbMakeDir);
            KeyGesture kgMakeDir = new KeyGesture(Key.F7);
            KeyBinding kbMakeDir = new KeyBinding(MakeDirCommand, kgMakeDir);

            this.InputBindings.Add(kbMakeDir);
            
            //////////////////////////////////////////////////
            CommandBinding cbCopy = new CommandBinding(CopyCommand, ExecutedCopyCommand);
            this.CommandBindings.Add(cbCopy);

            KeyGesture kgCopyF5 = new KeyGesture(Key.F5);
            KeyBinding kbCopyF5 = new KeyBinding(CopyCommand, kgCopyF5);

            KeyGesture kgCopyF6 = new KeyGesture(Key.F6);
            KeyBinding kbCopyF6 = new KeyBinding(CopyCommand, kgCopyF6);

            this.InputBindings.Add(kbCopyF5);
            this.InputBindings.Add(kbCopyF6);
            
            //////////////////////////////////////////////////
            CommandBinding cbRename = new CommandBinding(RenameCommand, ExecutedRenameCommand);
            this.CommandBindings.Add(cbRename);

            KeyGesture kgRename = new KeyGesture(Key.F2);
            KeyBinding kbRename = new KeyBinding(RenameCommand, kgRename);

            this.InputBindings.Add(kbRename);

            LoadWindowPos();
            //testing
            //ExplorerNet.Languages.LanguagesManager lm = new Languages.LanguagesManager();
            //var lst = lm.GetAllLanguages();

            lbLastStartedFiles.ItemsSource = Properties.Settings.Default.LastStartedFiles;
            //cmLastStartedFiles.Items.Clear();
            //cmLastStartedFiles.ItemsSource = Properties.Settings.Default.LastStartedFiles;
            lbLastStartedFilesContextMenu.ItemsSource = Properties.Settings.Default.LastStartedFiles;


            /////////////////////////
           // ExplorerNet.Tools.Wallpapers.WallpaperManager wm = new Tools.Wallpapers.WallpaperManager();
            //wm.ChangePicture(@"h:\pic\otBuh\CG Artwork Wallpapers Collection-3 02.jpg", imgFon);
            //this.imgFon.Visibility = System.Windows.Visibility.Hidden;
            //wm.ChangeWindowFon(this, Brushes.Black.Color);


           // WallpaperManager wm = new WallpaperManager();
            

            //wm.ApplyWallpaper(imgFon, this);
        }

        private void ExecutedRenameCommand(object sender,
            ExecutedRoutedEventArgs e)
        {
            RenameWindow rw = new RenameWindow();
            rw.ShowDialog(); 
        }

        private void ExecutedDeleteFilesCommand(object sender,
            ExecutedRoutedEventArgs e)
        {
            FilePanel fp1 = ExplorerNet.ViewWindowApps.FilePanelApps.FilePanelSelector.FirstSelected;

            if (fp1 != null)
            {
                fp1.DeleteFilesDialog();
            }

            //if (FilePanel.SelectedFilePanel != null)
            //{
            //    FilePanel.SelectedFilePanel.DeleteFiles();
            //}
        }

        private void ExecutedMakeDirCommand(object sender,
            ExecutedRoutedEventArgs e)
        {
            FilePanel fp1 = ExplorerNet.ViewWindowApps.FilePanelApps.FilePanelSelector.FirstSelected;

            if (fp1 != null)
            {
                fp1.MakeNewDirectoryDialog();
            }

            //if (FilePanel.SelectedFilePanel != null)
            //{
            //    FilePanel.SelectedFilePanel.MakeNewDirectoryDialog
        }


        private void ExecutedCopyCommand(object sender,
            ExecutedRoutedEventArgs e)
        {
            CopyWindow cw = new CopyWindow();
            cw.ShowDialog();

            //if (FilePanel.SelectedFilePanel != null)
            //{
            //    FilePanel.SelectedFilePanel.MakeNewDirectoryDialog();
            //}
        }


        /// <summary>
        /// Добавляем новый уровень
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLevel_Click(object sender, RoutedEventArgs e)
        {
            Level newLevel = new Level();
            spMain.Children.Add(newLevel);
        }

        /// <summary>
        /// Вызываем окно настроек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //SettingsWindow2 settingsWindow = new SettingsWindow2();
            //settingsWindow.ShowDialog();
            ExplorerNet.MVVM.View.SettingWindow sw = new MVVM.View.SettingWindow();
            sw.ShowDialog();
        }

        /// <summary>
        /// Сохранение шаблона с окном диалога
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTemplate_Click(object sender, RoutedEventArgs e)
        {
            string ext = Properties.Settings.Default.FileAppExtention;

            //ColorDialog cd = new ColorDialog();
            //cd.ShowDialog();


            System.Windows.Forms.SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Template"; // Default file name
            dlg.DefaultExt = ext; // Default file extension
            dlg.Filter = "Template (" + ext + ")|*" + ext + "|All Files|*.*"; // Filter files by extension

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _SaveTemplate(dlg.FileName);
            }
            #region комментарий
            //Вариант кода ниже закомментирован, так как в нём не совместимость со сборкой WPF.Themes, причина не понятна
            //Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //dlg.FileName = "Template"; // Default file name
            //dlg.DefaultExt = ext; // Default file extension
            //dlg.Filter = "Template (" + ext + ")|*" + ext + "|All Files|*.*"; // Filter files by extension

            //Nullable<bool> result = dlg.ShowDialog();

            //if (result == true)
            //{
            //    _SaveTemplate(dlg.FileName);
            //}
            #endregion
        }

        /// <summary>
        /// Формируем шаблон на основе интерфейса и сохраняем в файл
        /// </summary>
        /// <param name="fileName">Путь файла для сохранения</param>
        private void _SaveTemplate(string fileName)
        {
            //Перебираем все уровни
            SaveTemplateView();
            currentTemplate.Name = System.IO.Path.GetFileNameWithoutExtension(fileName);
            currentTemplate.Skin = new SkinManager().GetCurrentSkin();
            currentTemplate.Save(fileName);

            //добавляем в список недавних файлов
            RecentTemplateManager.AddNewRecentTemplate(currentTemplate.Name, fileName);
        }
        
        /// <summary>
        /// Сохранение визуальных уровней и панелей в шаблон currentTemplate
        /// </summary>
        private void SaveTemplateView()
        {
            this.currentTemplate = new ViewWindowTemplate();
            this.currentTemplate.Skin = new SkinManager().GetCurrentSkin();
            foreach (var child in spMain.Children)
            {
                if (child.GetType() == typeof(Level)) 
                { 
                    Level level = (Level)child;
                    LevelTemplate lTemplate = new LevelTemplate();

                    lTemplate.Height = level.Height;
                    //Перебираем все файловые панели
                    foreach (var c in level.spMain.Children)
                    {
                        if (c.GetType() == typeof(FilePanel))
                        {
                            FilePanel filePanel = (FilePanel)c;

                            FilePanelTemplate fpTemplate = new FilePanelTemplate();
                            fpTemplate.FilePanelSettings = filePanel.FilePanelSettings;
                            //fpTemplate.Width = filePanel.Width;
                            //fpTemplate.Path = filePanel.Path;
                            
                            lTemplate.FilePanels.Add(fpTemplate);
                        }
                    }
                    currentTemplate.Levels.Add(lTemplate);
                }
            }
        }

        /// <summary>
        /// загружаем шаблон из файла и строем визуальное отображения
        /// </summary>
        /// <param name="fileName"></param>
        public void _LoadTemplate(string fileName)
        {
            currentTemplate = ViewWindowTemplate.Load(fileName);
            new SkinManager().ApplySkin(currentTemplate.Skin);
            BuildTemplateView();
            //добавляем в список недавних файлов
            RecentTemplateManager.AddNewRecentTemplate(currentTemplate.Name, fileName);
        }

        /// <summary>
        /// строем отображения уровней и панелей в окне на основе currentTemplate
        /// </summary>
        private void BuildTemplateView()
        {
            spMain.Children.Clear();
            
            foreach (var levelTemplate in currentTemplate.Levels)
            {
                Level level = new Level();
                level.Height = levelTemplate.Height;

                foreach (var filePanelTemplate in levelTemplate.FilePanels)
                {
                    FilePanel filePanel = new FilePanel();
                    filePanel.filePanel.FilePanelSettings = filePanelTemplate.FilePanelSettings;
                    //filePanel.Width = filePanelTemplate.Width;
                    //filePanel.Path = filePanelTemplate.Path;

                    level.spMain.Children.Add(filePanel);
                }

                spMain.Children.Add(level);
            }
        }

        private void btnLoadTemplate_Click(object sender, RoutedEventArgs e)
        {
            string ext = Properties.Settings.Default.FileAppExtention;
            System.Windows.Forms.OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "Template"; // Default file name
            dlg.DefaultExt = ext; // Default file extension
            dlg.Filter = "Template (" + ext + ")|*" + ext + "|All Files|*.*"; // Filter files by extension

            //Nullable<bool> result = dlg.ShowDialog();

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _LoadTemplate(dlg.FileName);
            }

            #region комментарий
            //Вариант кода ниже закомментирован, так как в нём не совместимость со сборкой WPF.Themes, причина не понятна
            //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.FileName = "Template"; // Default file name
            //dlg.DefaultExt = ext; // Default file extension
            //dlg.Filter = "Template (" + ext + ")|*" + ext + "|All Files|*.*"; // Filter files by extension
            //dlg.ShowDialog();
            //Nullable<bool> result = dlg.ShowDialog();

            //if (result == true)
            //{
            //    _LoadTemplate(dlg.FileName);
            //}
            #endregion
        }

        private void btnClearTemplate_Click(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
        }

        /// <summary>
        /// загружаем расположение уровней и файловых панелей, 
        /// которое было сохранено при прежнем закрытии программы
        /// </summary>
        public void LoadLastTemplate()
        {
            //this.currentTemplate = ViewWindowTemplate.CreateDefoultTemplate();
            

            if (Properties.Settings.Default.LastTemplate == null)
            {
                this.currentTemplate = ViewWindowTemplate.CreateDefoultTemplate();
                
            }
            else
            {
                this.currentTemplate = Properties.Settings.Default.LastTemplate;
            }
            new SkinManager().ApplySkin(currentTemplate.Skin);
            BuildTemplateView();
        }

        /// <summary>
        /// сохраняем отображения уровней и файловых панелей для загрузки 
        /// при следующем запуске программы
        /// </summary>
        public void SaveLastTemplate()
        {
            SaveTemplateView();
            Properties.Settings.Default.LastTemplate = currentTemplate;
            //Properties.Settings.Default.CurrentSkin = currentTemplate.Skin;
            Properties.Settings.Default.Save();
        }

        #region Save and load this window state and size and position

        private void SaveWindowPos()
        {
            Properties.Settings.Default.ViewWindowTop = this.Top;
            Properties.Settings.Default.ViewWindowLeft = this.Left;
            Properties.Settings.Default.ViewWindowHeight = this.Height;
            Properties.Settings.Default.ViewWindowWidth = this.Width;
            Properties.Settings.Default.ViewWindowState = this.WindowState;
            Properties.Settings.Default.Save();
        }

        private void LoadWindowPos()
        {
            this.Top = Properties.Settings.Default.ViewWindowTop;
            this.Left = Properties.Settings.Default.ViewWindowLeft;
            this.Height = Properties.Settings.Default.ViewWindowHeight;
            this.Width = Properties.Settings.Default.ViewWindowWidth;
            this.WindowState = Properties.Settings.Default.ViewWindowState;
        }
        #endregion //Save and load this window state and size and position

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            asm.GetName();
            
            
            

            //Если имя сборки (и версия) изменились, запускаем информационное окно
            if (IsNewNameAssemblie())
            {
                InfoWindow inf = new InfoWindow();
                inf.ShowDialog();
            }
            //Properties.Settings.Default.Reset();
            //LoadLastTemplate();

            WallpaperManager wallpaperManager = new WallpaperManager();
            wallpaperManager.Init();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveLastTemplate();

            //var lng = Properties.Settings.Default.CurrLang;

            SaveWindowPos();

            //testing
            //Properties.Settings.Default.CurrLanguage = new Languages.Language();
            //Properties.Settings.Default.CurrLanguage.Name = "test";
            //Properties.Settings.Default.Save();
        }

        private void btnResetSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
        }

        private void btnRandomSkin_Click(object sender, RoutedEventArgs e)
        {
            SkinManager sm = new SkinManager();
            sm.ApplySkin(sm.GetSkins()[new Random().Next(15)]);
            var ss = sm.GetSkins();
            //var ci = System.Threading.Thread.CurrentThread.CurrentUICulture;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            //MessageBox.Show(Properties.Resources.AddLevel);
        }

        private void btnShowInfoWindow_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow iw = new InfoWindow();
            iw.ShowDialog();
        }

        /// <summary>
        /// Возвращает true если это новая версия приложения (текущей сборки)
        /// </summary>
        /// <returns></returns>
        private bool IsNewNameAssemblie()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            if (asm.FullName != Properties.Settings.Default.LastNameAssemblie)
            {
                Properties.Settings.Default.LastNameAssemblie = asm.FullName;
                Properties.Settings.Default.Save();
                return true;
            }
            else
            {
                return false;
            }
        }


        private void ButtonsContent_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LastStartedFile lsf = (LastStartedFile)lbLastStartedFiles.ItemContainerGenerator.ItemFromContainer((ListBoxItem)sender);

            ExplorerNet.Tools.FileStarter.Start(lsf.Path);
            //System.Diagnostics.Process.Start(lsf.Path);
        }

        private void btnClearLastFiles_Click(object sender, RoutedEventArgs e)
        {
            LastStartedFilesManager lsfm = new LastStartedFilesManager();
            lsfm.Clear();
            cmLastStartedFiles.IsOpen = false;
        }

        private void btnDeleteLastFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = 
                (System.Windows.Controls.Button)sender;

            if (btn.Tag.GetType() == typeof(LastStartedFile))
            {
                LastStartedFile lsf = (LastStartedFile)(btn.Tag);
                LastStartedFiles lsfs = LastStartedFiles.Load();
                lsfs.Remove(lsf);
            }
            
        }

        private void txtMenuPath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.FrameworkElement fe = (System.Windows.FrameworkElement)sender;
            if (fe.Tag.GetType() == typeof(LastStartedFile))
            {
                LastStartedFile lsf = (LastStartedFile)(fe.Tag);
                FileStarter.Start(lsf.Path);
            }

        }



    }
}
 