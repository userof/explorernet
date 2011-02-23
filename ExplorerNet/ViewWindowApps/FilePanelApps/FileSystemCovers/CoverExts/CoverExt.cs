using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.CoverExts
{
    [Serializable]
    public class CoverExt
    {
        private string filePath = null;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private string description = null;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private StarKind? star = null;

        public StarKind? Star
        {
            get { return star; }
            set { star = value; }
        }

        private int? hash = null;

        public int? Hash
        {
            get { return hash; }
            set { hash = value; }
        }

        /// <summary>
        /// No use! One for serialization
        /// </summary>
        public CoverExt()
        {

        }

        public void Save()
        {
            CoverExtManager.Save();
        }

        public static CoverExt CreateOrLoad(string filePath)
        {
            return CoverExtManager.CreateOrLoadCoverExt(filePath);
        }

        public static CoverExt Load(string filePath)
        {
            return CoverExtManager.LoadCoverExt(filePath);
        }

    }

    [Serializable]
    public enum StarKind
    {
        One,
        Two,
        Three
        //Foor,
        //Five
    }
}
