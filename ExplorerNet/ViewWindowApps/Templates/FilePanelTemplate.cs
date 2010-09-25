using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExplorerNet.ViewWindowApps.FilePanelApps;

namespace ExplorerNet.ViewWindowApps.Templates
{
    /// <summary>
    /// /// Шаблон для элемента FilePanel. Обеспечивает отражения его настроек в случае хранения.
    /// </summary>
    [Serializable]
    public class FilePanelTemplate
    {
        public FilePanelSettings FilePanelSettings
        {
            get;
            set;
        }

        //private double _width = 0;

        //private string _path = "";

        //public double Width
        //{
        //    get { return _width; }
        //    set { _width = value; }
        //}

        //public string Path
        //{
        //    get { return _path; }
        //    set { _path = value; }
        //}
    }
}
