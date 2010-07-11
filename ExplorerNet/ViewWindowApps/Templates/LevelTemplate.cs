using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.ViewWindowApps.Templates
{
    /// <summary>
    /// Шаблон для элемента Level. Обеспечивает отражения его настроек в случае хранения.
    /// </summary>
    [Serializable]
    public class LevelTemplate
    {
        /// <summary>
        /// Коллекция файловых панелей
        /// </summary>
        private List<FilePanelTemplate> _filePanels = null;

        /// <summary>
        /// Высота уровня
        /// </summary>
        private double _height = 0;

        public LevelTemplate()
        {
            _filePanels = new List<FilePanelTemplate>();
        }

        /// <summary>
        /// Коллекция файловых панелей
        /// </summary>
        public List<FilePanelTemplate> FilePanels
        {
            get { return _filePanels; }
        }

        /// <summary>
        /// Высота уровня
        /// </summary>
        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }
        
    }
}
