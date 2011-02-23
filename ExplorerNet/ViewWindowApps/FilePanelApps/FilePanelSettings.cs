using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.ViewWindowApps.FilePanelApps
{
    [Serializable]
    public class FilePanelSettings : ICloneable
    {
        public object Clone()
        {
            FilePanelSettings result = new FilePanelSettings();
            result.Width = this.Width;
            result.IcoWidth = this.IcoWidth;
            result.Path = this.Path;
            result.NameWidth = this.NameWidth;
            result.SizeWidth = this.SizeWidth;

            return result;
        }

        public double? Width
        { 
            get; 
            set; 
        }

        public double? StarWidth
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
