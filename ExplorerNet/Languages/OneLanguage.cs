using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Globalization;

namespace ExplorerNet.Languages
{
    /// <summary>
    /// Представляет объект языка
    /// </summary>
    [Serializable]
    public class OneLanguage
    {
        /// <summary>
        /// Отображающееся имя языка
        /// </summary>
        private string name = "";

        /// <summary>
        /// Культура языка. (Может быть не присвоена)
        /// </summary>
        private CultureInfo culture = null;

        /// <summary>
        /// Словарь ресурсов в котором хранится информация данного языка
        /// </summary>
        private ResourceDictionary resourceDictionary = null;

        /// <summary>
        /// Позволяет создать язык на основе имени. (Язык с таким именем 
        /// должен быть в списке языков LanguagesManager.GetAllOneLanguages)
        /// </summary>
        /// <param name="langName"></param>
        /// <returns></returns>
        public static OneLanguage FromName(string langName)
        {
            LanguagesManager lm = new LanguagesManager();
            var langs = lm.GetAllOneLanguages();
            OneLanguage oneLanguage = null;
            foreach (var lang in langs)
            {
                if (lang.Name == langName)
                {
                    oneLanguage = lang;
                }
            }

            if (oneLanguage == null)
            {
                return lm.GetEnLenguage();
            }
            else
            {
                return oneLanguage;
            }

        }

        /// <summary>
        /// Словарь ресурсов в котором хранится информация данного языка
        /// </summary>
        public ResourceDictionary ResourceDictionary
        {
            get { return resourceDictionary; }
            set { resourceDictionary = value; }
        }

        /// <summary>
        /// Культура языка. (Может быть не присвоена)
        /// </summary>
        public CultureInfo Culture
        {
            get { return culture; }
            set { culture = value; }
        }

        /// <summary>
        /// Отображающееся имя языка
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //Свойства, для передачи языковой информации в коде
        #region LocalizationProperty

        #region SizeFileInString

        /// <summary>
        /// Size in GB
        /// </summary>
        public string SFISGB
        {
            get 
            {
                //return "GB";
                return (string)resourceDictionary["SFISGB"];
            }
        }

        /// <summary>
        /// Size in MB
        /// </summary>
        public string SFISMB
        {
            get
            {
                //return "MB";
                return (string)resourceDictionary["SFISMB"];
            }
        }

        /// <summary>
        /// Size in KB
        /// </summary>
        public string SFISKB
        {
            get
            {
                //return "KB";
                return (string)resourceDictionary["SFISKB"];
            }
        }

        /// <summary>
        /// Size in Bytes
        /// </summary>
        public string SFISBytes
        {
            get
            {
                //return "b-a-a";
                return (string)resourceDictionary["SFISBytes"];
            }
        }


        #endregion //SizeFileInString

        #region SizePanel

        public string SPDir
        {
            get
            {
                return (string)resourceDictionary["SPDir"];
            }
        }

        #endregion //SizePanel

        #region DriveData

        public string DDDriveIsNotReady
        {
            get
            {
                return (string)resourceDictionary["DDDriveIsNotReady"];
            }
        }

        public string DDLongSizeString
        {
            get
            {
                string str = (string)resourceDictionary["DDLongSizeString"];
                if (string.IsNullOrEmpty(str))
                {
                    LanguagesManager lm = new LanguagesManager();
                    str = lm.GetEnLenguage().DDLongSizeString;
                }
                return str;
            }
        }

        #endregion

        #region FilePanel

        public string FPIsNotAccess
        {
            get
            {
                return (string)resourceDictionary["FPIsNotAccess"];
            }
        }

        #endregion //FilePanel

        #endregion

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(OneLanguage))
            {
                OneLanguage lang = (OneLanguage)obj;

                if (this.ResourceDictionary.Source == lang.ResourceDictionary.Source)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            //return base.Equals(obj);

        }

        public override int GetHashCode()
        {
            return ResourceDictionary.GetHashCode();
        }
    }
}
