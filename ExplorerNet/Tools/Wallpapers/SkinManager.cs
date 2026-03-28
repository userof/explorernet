using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows;
using System.Windows.Baml2006;

namespace ExplorerNet.Tools
{
    /// <summary>
    /// Класс, который управляет скинами приложения
    /// </summary>
    public class SkinManager
    {
        /// <summary>
        /// Путь приложения
        /// </summary>
        private string appPath = "";

        /// <summary>
        /// Директория с файлами скинов
        /// </summary>
        private string skinDirPath = "";

        /// <summary>
        /// Имя папки со скинами
        /// </summary>
        private const string skinDirName = "skins";

        //private const string skinExt = "xaml";

        /// <summary>
        /// Расширения фалов скинов
        /// </summary>
        private const string skinSearchFilter = "*.?aml";


        public delegate void ChangedSkinEventHandler(Object sender, string skin);

        public static event ChangedSkinEventHandler ChangedSkin;

        public SkinManager()
        {
            Init();
        }

        /// <summary>
        /// Метод инициализации основных данных класса
        /// </summary>
        private void Init()
        {
            appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            if (string.IsNullOrEmpty(appPath))
            {
                appPath = AppContext.BaseDirectory;
                skinDirPath = Path.Combine(appPath, skinDirName);
            }
            else
            {
                skinDirPath = Path.GetDirectoryName(appPath) + Path.DirectorySeparatorChar + skinDirName;
            }
        }

        /// <summary>
        /// Устанавливает скин приложения
        /// </summary>
        /// <param name="skinName">Метод инициализации основных данных класса</param>
        public void ApplySkin(string skinName)
        {
            string skinPathXaml = skinDirPath + Path.DirectorySeparatorChar +
                  skinName + ".xaml";

            string skinPathBaml = skinDirPath + Path.DirectorySeparatorChar +
                  skinName + ".baml";

            try
            {
                if (File.Exists(skinPathXaml))
                {
                    ApplySkinXaml(skinPathXaml);
                    Properties.Settings.Default.CurrentSkin = skinName;

                    if (ChangedSkin != null)
                    {
                        ChangedSkin(this, skinName);
                    }
                }
                else if (File.Exists(skinPathBaml))
                {
                    ApplySkinBaml(skinPathBaml);
                    Properties.Settings.Default.CurrentSkin = skinName;

                    if (ChangedSkin != null)
                    {
                        ChangedSkin(this, skinName);
                    }
                }
            }
            catch (Exception)
            {
                // Skin references unavailable assembly (e.g. legacy WPFToolkit BAML).
                // Continue without applying the skin.
            }
        }

        /// <summary>
        /// Устанавливает Xaml скин приложения
        /// </summary>
        /// <param name="skinPath"></param>
        protected void ApplySkinXaml(string skinPath)
        {
            System.Windows.ResourceDictionary rd = new System.Windows.ResourceDictionary();
            rd.Source = new Uri(skinPath);
            App.Current.Resources.MergedDictionaries.Add(rd);
 
        }

        /// <summary>
        /// Устанавливает Baml скин приложения
        /// </summary>
        /// <param name="skinPath"></param>
        protected void ApplySkinBaml(string skinPath)
        {
            // Legacy .NET Framework 4.0 BAML skin files are binary-incompatible
            // with .NET 10 (different BAML format, WPFToolkit references, etc.).
            // These skins need to be recompiled as XAML to work on modern .NET.
        }


        /// <summary>
        /// Возвращает список скинов приложения
        /// </summary>
        /// <returns></returns>
        public string[] GetSkins()
        {
            DirectoryInfo dies = new DirectoryInfo(skinDirPath);

            FileInfo[] files = dies.GetFiles(skinSearchFilter);

            List<string> skins = new List<string>();

            foreach (var file in files)
            {
                skins.Add(Path.GetFileNameWithoutExtension(file.FullName));
            }

            return skins.ToArray();
        }

        /// <summary>
        /// Возвращает текущий скин приложения
        /// </summary>
        /// <returns></returns>
        public string GetCurrentSkin()
        {
            return Properties.Settings.Default.CurrentSkin;
        }

        public string SkinDirPath
        {
            get
            {
                return skinDirPath;
            }
        }
    }
}
