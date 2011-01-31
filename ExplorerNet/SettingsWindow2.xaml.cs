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
    }
}
