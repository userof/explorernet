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

namespace ExplorerNet
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            _LoadSettings();
        }

        /// <summary>
        /// Загружаем настройки
        /// </summary>
        private void _LoadSettings()
        {
            txtHeightLevel.Text = Properties.Settings.Default.HeightLevel.ToString();
            txtWidthFilepanel.Text = Properties.Settings.Default.WidthFilepanel.ToString();

            //switch(Properties.Settings.Default.FileOverwriteOption)
            switch (Properties.Settings.Default.FileOverwriteOption)
            {
                case ExplorerNet.CopyOrMoveWindowApps.FileOverwriteOptionKind.ShowDialog:
                    rbShowDialog.IsChecked = true;
                    break;
                case ExplorerNet.CopyOrMoveWindowApps.FileOverwriteOptionKind.Skip:
                    rbSkipFile.IsChecked = true;
                    break;
                case ExplorerNet.CopyOrMoveWindowApps.FileOverwriteOptionKind.Rewrite:
                    rbRewriteFile.IsChecked = true;
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Сохраняем настройки
        /// </summary>
        private void _SaveSettings()
        {
            Properties.Settings.Default.HeightLevel = double.Parse(txtHeightLevel.Text);
            Properties.Settings.Default.WidthFilepanel = double.Parse(txtWidthFilepanel.Text);

            #region Сохраняем настройки перезаписи файла 
            if (rbShowDialog.IsChecked == true)
            {
                Properties.Settings.Default.FileOverwriteOption = 
                    CopyOrMoveWindowApps.FileOverwriteOptionKind.ShowDialog;
            }

            if (rbRewriteFile.IsChecked == true)
            {
                Properties.Settings.Default.FileOverwriteOption =
                    CopyOrMoveWindowApps.FileOverwriteOptionKind.Rewrite;
            }

            if (rbSkipFile.IsChecked == true)
            {
                Properties.Settings.Default.FileOverwriteOption =
                    CopyOrMoveWindowApps.FileOverwriteOptionKind.Skip;
            }
            #endregion //Сохраняем настройки перезаписи файла


            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Закрываем окно без сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Восстанавливаем настройки по умолчанию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
            _LoadSettings();
        }

        /// <summary>
        /// Сохраняем настройки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            _SaveSettings();
        }

        /// <summary>
        /// Закрываем окно с сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _SaveSettings();
            this.Close();
        }
    }
}
