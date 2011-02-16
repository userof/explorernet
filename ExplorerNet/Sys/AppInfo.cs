using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.Sys
{
    public static class AppInfo
    {
        private static string appPath = "";

        private static string appDirPath = "";

        private const string PictureFile = "Fon.img";

        public static string AppPath
        {
            get
            {
                if (string.IsNullOrEmpty(appPath))
                {
                    appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                }
                return appPath;
            }
        }

        public static string AppDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(appDirPath))
                {
                    appDirPath = System.IO.Path.GetDirectoryName(AppPath);
                }
                return appDirPath;
            }
        }

        public static string PictureFilePath
        {
            get
            {
                return ExplorerNet.Sys.AppInfo.AppDirectory +
                    System.IO.Path.DirectorySeparatorChar +
                    PictureFile;
            }
        }
    }
}
