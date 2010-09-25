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

using ExplorerNet.ViewWindowApps;
using ExplorerNet.ViewWindowApps.Templates;

namespace ExplorerNet
{
    /// <summary>
    /// Основное рабочее окно. 
    /// Оно даёт возможность просматривать структуру файловой системы 
    /// и начать операции с файлами
    /// </summary>
    public partial class ViewWindow : Window
    {
        /// <summary>
        /// Текущий шаблон окна
        /// </summary>
        private ViewWindowTemplate currentTemplate = null;

        public static RoutedCommand DeleteFilesCommand = new RoutedCommand();

        public ViewWindow()
        {
            InitializeComponent();

            CommandBinding cb = new CommandBinding(DeleteFilesCommand, ExecutedDeleteFilesCommand);
            this.CommandBindings.Add(cb);

            KeyGesture kg = new KeyGesture(Key.Delete);
            KeyBinding kb = new KeyBinding(DeleteFilesCommand, kg);
            this.InputBindings.Add(kb);
            LoadWindowPos();
        }

        private void ExecutedDeleteFilesCommand(object sender,
            ExecutedRoutedEventArgs e)
        {
            if (FilePanel.SelectedFilePanel != null)
            {
                FilePanel.SelectedFilePanel.DeleteFiles();
            }
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
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        /// <summary>
        /// Сохранение шаблона с окном диалога
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTemplate_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Template"; // Default file name
            dlg.DefaultExt = ".vtmpl"; // Default file extension
            dlg.Filter = "Template (.vtmpl)|*.vtmpl|All Files|*.*"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                _SaveTemplate(dlg.FileName);
            }
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
            currentTemplate.Save(fileName);

        }
        
        /// <summary>
        /// Сохранение визуальных уровней и панелей в шаблон currentTemplate
        /// </summary>
        private void SaveTemplateView()
        {
            this.currentTemplate = new ViewWindowTemplate();
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
        private void _LoadTemplate(string fileName)
        {
            currentTemplate = ViewWindowTemplate.Load(fileName);
            BuildTemplateView();
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
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Template"; // Default file name
            dlg.DefaultExt = ".vtmpl"; // Default file extension
            dlg.Filter = "Template (.vtmpl)|*.vtmpl|All Files|*.*"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                _LoadTemplate(dlg.FileName);
            }
        }

        private void btnClearTemplate_Click(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
        }

        /// <summary>
        /// загружаем расположение уровней и файловых панелей, 
        /// которое было сохранено при прежнем закрытии программы
        /// </summary>
        private void LoadLastTemplate()
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
            BuildTemplateView();
        }

        /// <summary>
        /// сохраняем отображения уровней и файловых панелей для загрузки 
        /// при следующем запуске программы
        /// </summary>
        private void SaveLastTemplate()
        {
            SaveTemplateView();
            Properties.Settings.Default.LastTemplate = currentTemplate;
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
            //Properties.Settings.Default.Reset();
            LoadLastTemplate();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveLastTemplate();
            SaveWindowPos();
        }



    }
}
