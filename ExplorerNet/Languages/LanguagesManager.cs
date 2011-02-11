using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows;
using System.Globalization;

namespace ExplorerNet.Languages
{
    /// <summary>
    /// Позволяет управлять языками
    /// </summary>
    public class LanguagesManager
    {
        /// <summary>
        /// Путь приложения
        /// </summary>
        private string appPath = "";

        /// <summary>
        /// Путь директории, в которой расположены файлы языков
        /// </summary>
        private string languagesDirPath = "";

        /// <summary>
        /// Имя папки, в которой расположены файлы языков
        /// </summary>
        private const string LanguagesDirectory = "Languages";

        public delegate void ChangeLanguagesEventHandler(OneLanguage newLanguag);

        /// <summary>
        /// Происходит при смене языка
        /// </summary>
        public event ChangeLanguagesEventHandler ChangeLanguage;

        public LanguagesManager()
        {
            //ChangeLanguage = new ChangeLanguagesEventHandler(ChangeLanguage);

            appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            languagesDirPath = Path.GetDirectoryName(appPath) + Path.DirectorySeparatorChar + LanguagesDirectory;
        }

        #region Methods

        /// <summary>
        /// Создаёт объект языка на основе словаря ресурсов
        /// </summary>
        /// <param name="rdOneLanguage">Словарь ресурсов на основании которого будет создан новый объект языка</param>
        /// <returns></returns>
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

        /// <summary>
        /// Создаёт объект языка на основе языка по умолчанию (английского)
        /// </summary>
        /// <returns></returns>
        public OneLanguage GetEnLenguage()
        {
            ResourceDictionary rdEn = new ResourceDictionary();
            rdEn.Source = new Uri("\\Languages\\en.xaml", UriKind.Relative);
            //return new Language(rdEn);
            //Language lang = new Language();

            return CreateOneLanguage(rdEn);


            //return null;
        }

        /// <summary>
        /// Получить список всех доступных языков, которые может использовать программа
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Изминения языка приложения
        /// </summary>
        /// <param name="lang">новый язык приложения</param>
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
        #endregion //Methods

        #region Static methods

        /// <summary>
        /// Устанавливает первоначальные значения языка в приложении. Используется при старте приложения
        /// </summary>
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

        /// <summary>
        /// Получаем объект текущего языка приложения
        /// </summary>
        /// <returns></returns>
        public static OneLanguage GetCurrLanguage()
        {
            LanguagesManager lm = new LanguagesManager();
            return lm.CurrLanguage;
        }

        #endregion //Static methods

        /// <summary>
        /// Узнаём текущий язык приложения
        /// </summary>
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

        public string LanguagesDirPath
        {
            get
            {
                return languagesDirPath;
            }
        }


    }
}
