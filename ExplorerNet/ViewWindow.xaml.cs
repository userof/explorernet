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

        public ViewWindow()
        {
            InitializeComponent();
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
            currentTemplate = new ViewWindowTemplate();
            currentTemplate.Name = System.IO.Path.GetFileNameWithoutExtension(fileName);
            //Перебираем все уровни
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
                            fpTemplate.Width = filePanel.Width;

                            lTemplate.FilePanels.Add(fpTemplate);
                        }
                    }
                    currentTemplate.Levels.Add(lTemplate);
                }
            }

            currentTemplate.Save(fileName);

        }
    }
}
