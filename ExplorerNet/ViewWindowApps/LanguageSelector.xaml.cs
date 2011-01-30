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

using ExplorerNet.Languages;

namespace ExplorerNet.ViewWindowApps
{
    /// <summary>
    /// Логика взаимодействия для LanguageSelector.xaml
    /// </summary>
    public partial class LanguageSelector : Button
    {
        private List<OneLanguage> languages = null;

        private int currLangNum = 0;

        private LanguagesManager languagesManager = null;

        public LanguageSelector()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                languagesManager = new LanguagesManager();
                languagesManager.ChangeLanguage += new LanguagesManager.ChangeLanguagesEventHandler(languagesManager_ChangeLanguage);
                
                //this.Content = languagesManager.CurrLanguage.Name;
                this.tbData.Text = languagesManager.CurrLanguage.Name;

                ExplorerNet.Tools.SkinManager.ChangedSkin += delegate(Object sender, string skin)
                {
                    Style stl = (Style)App.Current.Resources[typeof(Button)];

                    this.Style = stl;

                    //Style stl = new Style(this.GetType());
                    //stl.BasedOn = new Style(typeof(Button));
                    //stl.TargetType = this.GetType();

                    //this.Style = stl;

                    //this.UpdateLayout();
                };

            }
        }

        private void languagesManager_ChangeLanguage(OneLanguage newLang)
        {
            //this.Content = newLang.Name;
            this.tbData.Text = newLang.Name; 
        }

        private void Button_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            cm.Items.Clear();

            List<OneLanguage> langs = languagesManager.GetAllOneLanguages();

            StackPanel sp = null;
            TextBlock tbName = null;
            TextBlock tbCultureName = null;
            CheckBox cb = null;
            foreach (var lang in langs)
            {
                sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.Tag = lang;
                sp.PreviewMouseDown += new MouseButtonEventHandler(sp_PreviewMouseDown);
                tbName = new TextBlock();
                tbName.Text = lang.Name;
                cb = new CheckBox();
                cb.IsEnabled = false;

                if (lang.Name == languagesManager.CurrLanguage.Name)
                {
                    cb.IsChecked = true; //this.t
                }
                else
                {
                    cb.IsChecked = false;
                }
                sp.Children.Add(cb);
                sp.Children.Add(tbName);

                if (lang.Culture != null)
                {
                    tbCultureName = new TextBlock();
                    tbCultureName.Text = " - " + lang.Culture.DisplayName;
                    sp.Children.Add(tbCultureName);
                }

                cm.Items.Add(sp);
            }

            sp = new StackPanel();
            sp.PreviewMouseDown += new MouseButtonEventHandler(sp_PreviewMouseDown);
            tbName = new TextBlock();
            tbName.Text = "Hide";
            sp.Children.Add(tbName);
            cm.Items.Add(sp);

            //this.Content = Properties.Settings.Default.CurrLang;
        }

        private void sp_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel sp = (StackPanel)sender;
            if (sp.Tag != null)
            {
                OneLanguage lang = (OneLanguage)sp.Tag;

                //LanguagesManager lm = new LanguagesManager();
                languagesManager.ChangeOneLanguage(lang);
            }
            else
            {
                Properties.Settings.Default.LanguageSelectorVisible = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //LanguagesManager lm = new LanguagesManager();
            if (languages == null)
            {
                languages = languagesManager.GetAllOneLanguages();
            }

            OneLanguage lang = languagesManager.CurrLanguage;

            if (lang.Name == languages[currLangNum].Name)
            {
                currLangNum++;
            }

            languagesManager.ChangeOneLanguage(languages[currLangNum]);
            currLangNum++;

            if (currLangNum >= languages.Count)
            {
                currLangNum = 0;
            }

            //this.Content = Properties.Settings.Default.CurrLang;

        }

        private void Button_LayoutUpdated(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.LanguageSelectorVisible)
            {
                this.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                this.Visibility = System.Windows.Visibility.Visible;

            }
        }

    }
}
