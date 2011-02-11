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

using ExplorerNet.Languages;
using ExplorerNet.Tools;
using ExplorerNet.Tools.Wallpapers;


namespace ExplorerNet
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow2.xaml
    /// </summary>
    public partial class SettingsWindow2 : Window
    {
        public SettingsWindow2()
        {
            InitializeComponent();

            RefreshLanguages();

            RefreshSkins();

            WallpaperBuildVisual();
        }

        private void RefreshSkins()
        {
            SkinManager sm = new SkinManager();
            lvSkins.ItemsSource = sm.GetSkins().ToList();
            lvSkins.SelectedItem = sm.GetCurrentSkin();
        }


        private void RefreshLanguages()
        {

            LanguagesManager lm = new LanguagesManager();
            lvLanguages.ItemsSource = lm.GetAllOneLanguages();
            lvLanguages.SelectedItem = lm.CurrLanguage;

            cbLangSelectorChange.IsChecked = Properties.Settings.Default.LanguageSelectorVisible;
        }

        private void lvLanguages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LanguagesManager lm = new LanguagesManager();
            lm.ChangeOneLanguage((OneLanguage)lvLanguages.SelectedItem);
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

        private void lvSkins_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SkinManager sm = new SkinManager();
            sm.ApplySkin((string)lvSkins.SelectedItem);
        }

        private void btnOpenSkinDirectory_Click(object sender, RoutedEventArgs e)
        {
            SkinManager sm = new SkinManager();
            System.Diagnostics.Process.Start(sm.SkinDirPath);
        }

        private void btnResetApp_Click(object sender, RoutedEventArgs e)
        {
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            App.Current.Shutdown();

            FileStarter.Start(appPath);
        }

        private void btnOpenLangDir_Click(object sender, RoutedEventArgs e)
        {
            Languages.LanguagesManager lm = new LanguagesManager();
            System.Diagnostics.Process.Start(lm.LanguagesDirPath);
        }

        private void btnChangeWallpaperColor_Click(object sender, RoutedEventArgs e)
        {
            //Colo
        }

        private void btnOpenWallpaper_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = 
                new System.Windows.Forms.OpenFileDialog();
            //dlg.FileName = "*"; // Default file name
            //dlg.DefaultExt = "jpg|bmp|png|gif"; // Default file extension
            dlg.Filter = "Image Files|*.jpg;*.gif;*.bmp;*.png;*.jpeg|All Files|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtWallpaperPath.Text = dlg.FileName;

                WallpaperStreachChange();

                imgWallpaperPreview.Source = new BitmapImage(new Uri(dlg.FileName, 
                    UriKind.Absolute));
                //WallpaperManager wm = new WallpaperManager();
                //wm.ChangePicture(dlg.FileName);
            }
        }

        private void WallpaperStreachChange()
        {
            WallpaperManager wm = new WallpaperManager();
            if (rbWPFill.IsChecked.GetValueOrDefault(false))
            {
                imgWallpaperPreview.Stretch = Stretch.Fill;
                wm.ChangeStretch(Stretch.Fill);
            }
            else if (rbWPNone.IsChecked.GetValueOrDefault(false))
            {
                imgWallpaperPreview.Stretch = Stretch.None;
                wm.ChangeStretch(Stretch.None);
            }
            else if (rbWPUniform.IsChecked.GetValueOrDefault(false))
            {
                imgWallpaperPreview.Stretch = Stretch.Uniform;
                wm.ChangeStretch(Stretch.Uniform);
            }
            else if (rbWPUniformToFill.IsChecked.GetValueOrDefault(false))
            {
                imgWallpaperPreview.Stretch = Stretch.UniformToFill;
                wm.ChangeStretch(Stretch.UniformToFill);
            }
        }

        private void rbWPNone_Checked(object sender, RoutedEventArgs e)
        {
            WallpaperStreachChange();
        }

        private void WallpaperBuildVisual()
        {
            WallpaperManager wm = new WallpaperManager();

            if (wm.WallpaperSetting.Kind == WallpaperKind.Picture)
            {
                rbWPPicture.IsChecked = true;
            }
            else if (wm.WallpaperSetting.Kind == WallpaperKind.Fon)
            {
                rbWPFon.IsChecked = true;
            }
            switch (wm.WallpaperSetting.Stretch)
            {
                case Stretch.Fill:
                    rbWPFill.IsChecked = true;
                    break;
                case Stretch.None:
                    rbWPNone.IsChecked = true;
                    break;
                case Stretch.Uniform:
                    rbWPUniform.IsChecked = true;
                    break;
                case Stretch.UniformToFill:
                    rbWPUniformToFill.IsChecked = true;
                    break;
                default:
                    break;
            }

        }

        private void rbWPPicture_Checked(object sender, RoutedEventArgs e)
        {
            WallpaperManager wm = new WallpaperManager();
            //wm.ChangeStretch
        }

        private void rbWPFon_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnChangeWallpaper_Click(object sender, RoutedEventArgs e)
        {
            WallpaperManager wm = new WallpaperManager();
            wm.ChangePicture(txtWallpaperPath.Text);
        }
    }
}
