using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.CoverExts
{
    internal static class CoverExtManager
    {
        private static CoverExtCollection coverExtList = null;

        public static CoverExtCollection CoverExtList
        {
            get
            {
                if (coverExtList == null)
                {
                    coverExtList = Properties.Settings.Default.CoverExtList;

                    if (coverExtList == null)
                    {
                        coverExtList = new CoverExtCollection();
                        Properties.Settings.Default.CoverExtList = coverExtList;
                        Properties.Settings.Default.Save();
                    }
                }

                return coverExtList;
            }
        }

        public static CoverExt LoadCoverExt(string filePath)
        {
            int filePathHex = filePath.GetHashCode();
            CoverExt coverExt = null;

            foreach (var ce in CoverExtList)
            {
                if (ce.Hash == filePathHex)
                {
                    coverExt = ce;
                }
            }
            return coverExt;
        }

        public static void Save()
        {
            Properties.Settings.Default.CoverExtList = CoverExtList;
            Properties.Settings.Default.Save();
        }

        public static CoverExt CreateOrLoadCoverExt(string filePath, 
            string description = null, StarKind? star = null)
        {

            CoverExt coverExt = LoadCoverExt(filePath);
            //foreach (var ce in CoverExtList)
            //{
            //    if (ce.Hash == filePathHex)
            //    {
            //        coverExt = ce;
            //    }
            //}

            if (coverExt == null)
            {
                coverExt = CreateCoverExt(filePath, description, star);
            }

            return coverExt;
        }

        private static CoverExt CreateCoverExt(string filePath, 
            string description = null, StarKind? star = null)
        {
            CoverExt coverExt = new CoverExt();
            coverExt.FilePath = filePath;
            coverExt.Description = description;
            coverExt.Star = star;
            coverExt.Hash = filePath.GetHashCode();
            CoverExtList.Add(coverExt);
            return coverExt;
        }
    }
}
