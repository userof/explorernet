using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.ViewWindowApps.FilePanelApps
{
    [Serializable]
    public class FilePanelSettings
    {
        public double? Width
        { 
            get; 
            set; 
        }

        public string Path
        {
            get;
            set;
        }

        public double? IcoWidth
        {
            get;
            set;
        }

        public double? NameWidth
        {
            get;
            set;
        }

        public double? SizeWidth
        {
            get;
            set;
        }
    }
}
