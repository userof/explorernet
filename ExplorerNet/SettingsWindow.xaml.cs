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
using ExplorerNet.Tools;

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
                case ExplorerNet.CopyWindowApps.FileOverwriteOptionKind.ShowDialog:
                    rbShowDialog.IsChecked = true;
                    break;
                case ExplorerNet.CopyWindowApps.FileOverwriteOptionKind.Skip:
                    rbSkipFile.IsChecked = true;
                    break;
                case ExplorerNet.CopyWindowApps.FileOverwriteOptionKind.Rewrite:
                    rbRewriteFile.IsChecked = true;
                    break;
                default:
                    break;
            }

            cbLangSelectorChange.IsChecked = Properties.Settings.Default.LanguageSelectorVisible;

            //ExplorerNet.Tools.ViewSettings.ViewLocation.SaveWindowLocation(this);
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
                    CopyWindowApps.FileOverwriteOptionKind.ShowDialog;
            }

            if (rbRewriteFile.IsChecked == true)
            {
                Properties.Settings.Default.FileOverwriteOption =
                    CopyWindowApps.FileOverwriteOptionKind.Rewrite;
            }

            if (rbSkipFile.IsChecked == true)
            {
                Properties.Settings.Default.FileOverwriteOption =
                    CopyWindowApps.FileOverwriteOptionKind.Skip;
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

        private void RefreshSkins()
        {
            SkinManager sm = new SkinManager();
            lstSkins.ItemsSource = sm.GetSkins();
            lstSkins.SelectedItem = sm.GetCurrentSkin();
            //lstSkins.Items.Clear();

            //foreach (var s in sm.GetSkins())
            //{
            //    ListViewItem lvi = new ListViewItem();
            //    lvi.Content = s;
            //    lstSkins.Items.Add(lvi);
            //}
        }



        private void lstSkins_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SkinManager sm = new SkinManager();
            sm.ApplySkin((string)lstSkins.SelectedItem);
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RefreshSkins();
        }

        private void cbLangSelectorChange_Click(object sender, RoutedEventArgs e)
        {
            if (cbLangSelectorChange.IsChecked == true)
            {
                Properties.Settings.Default.LanguageSelectorVisible = true;
            }
            else
            {
                Properties.Settings.Default.LanguageSelectorVisible = false;
            }
        }
    }
}
