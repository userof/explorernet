using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.ViewWindowApps.Templates
{
    /// <summary>
    /// /// Шаблон для элемента FilePanel. Обеспечивает отражения его настроек в случае хранения.
    /// </summary>
    [Serializable]
    public class FilePanelTemplate
    {
        private double _width = 0;

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }
    }
}
