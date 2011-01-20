using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows;
using System.Globalization;

namespace ExplorerNet.Languages
{
    


    public class LanguagesManager
    {
        private string appPath = "";

        private string languagesDirPath = "";

        private const string LanguagesDirectory = "Languages";

        public delegate void ChangeLanguagesEventHandler(OneLanguage newLanguag);

        public event ChangeLanguagesEventHandler ChangeLanguage;

        public LanguagesManager()
        {
            //ChangeLanguage = new ChangeLanguagesEventHandler(ChangeLanguage);

            appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            languagesDirPath = Path.GetDirectoryName(appPath) + Path.DirectorySeparatorChar + LanguagesDirectory;
        }

        public OneLanguage CreateOneLanguage(ResourceDictionary rdOneLanguage)
        {
            OneLanguage lang = new OneLanguage();

            lang.ResourceDictionary = rdOneLanguage;

            if (rdOneLanguage.Contains("LanguageDisplayName"))
            {
                lang.Name = (string)rdOneLanguage["LanguageDisplayName"];
            }

            if (rdOneLanguage.Contains("LanguageCulture"))
            {
                string cult = (string)rdOneLanguage["LanguageCulture"];
                foreach (var c in CultureInfo.GetCultures(CultureTypes.AllCultures))
                {
                    if (c.Name == cult)
                    {
                       lang.Culture = new CultureInfo(c.Name);
                    }
                }
            }

            return lang;
        }

        public OneLanguage GetEnLenguage()
        {
            ResourceDictionary rdEn = new ResourceDictionary();
            rdEn.Source = new Uri("\\Languages\\en.xaml", UriKind.Relative);
            //return new Language(rdEn);
            //Language lang = new Language();

            return CreateOneLanguage(rdEn);


            //return null;
        }

        public List<OneLanguage> GetAllOneLanguages()
        {
            List<OneLanguage> OneLanguages = new List<OneLanguage>();
            OneLanguages.Add(GetEnLenguage());


            OneLanguage lang = null;

            DirectoryInfo diLanguagesDir = new DirectoryInfo(languagesDirPath);
            foreach (var f in diLanguagesDir.GetFiles("*.xaml"))
            {
                System.Windows.ResourceDictionary rd = new System.Windows.ResourceDictionary();
                rd.Source = new Uri(f.FullName);
                lang = CreateOneLanguage(rd);
                OneLanguages.Add(lang);

            }

            return OneLanguages;
        }

        public void ChangeOneLanguage(OneLanguage lang)
        {
            Properties.Settings.Default.CurrLang = lang.Name;
            Properties.Settings.Default.Save();

            App.Current.Resources.MergedDictionaries.Add(lang.ResourceDictionary);

            if (ChangeLanguage != null)
            {
                ChangeLanguage(lang);
            }
        }

        public static void Init()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.CurrLang))
            {
                CultureInfo currCulture = CultureInfo.CurrentUICulture;

                LanguagesManager lm = new LanguagesManager();

                List<OneLanguage> langs = lm.GetAllOneLanguages();

                foreach (var lang in langs)
                {
                    if (lang.Culture != null)
                    {
                        if (currCulture.Name == lang.Culture.Name)
                        {
                            lm.ChangeOneLanguage(lang);
                        }
                    }
                }

            }
            else
            {
                LanguagesManager lm = new LanguagesManager();
                lm.ChangeOneLanguage(OneLanguage.FromName(Properties.Settings.Default.CurrLang));
            }
        }

        public OneLanguage CurrLanguage
        {
            get
            {
                if (Properties.Settings.Default.CurrLang == null)
                {
                    return GetEnLenguage();
                }
                else
                {
                    return OneLanguage.FromName(Properties.Settings.Default.CurrLang);
                    //return null;
                }
            }
            set
            {
                ChangeOneLanguage(value);
            }
        }

        public static OneLanguage GetCurrLanguage()
        {
            LanguagesManager lm = new LanguagesManager();
            return lm.CurrLanguage;
        }
    }
}
