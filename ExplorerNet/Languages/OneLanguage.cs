﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Globalization;

namespace ExplorerNet.Languages
{
    [Serializable]//
    public class OneLanguage
    {
        private string name = "";

        private CultureInfo culture = null;

        private ResourceDictionary resourceDictionary = null;

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


        public ResourceDictionary ResourceDictionary
        {
            get { return resourceDictionary; }
            set { resourceDictionary = value; }
        }

        public CultureInfo Culture
        {
            get { return culture; }
            set { culture = value; }
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

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
    }
}