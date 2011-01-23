using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.Tools.ViewSettings
{
    [Serializable]
    public class ViewLocations : List<ViewLocation>
    {
        public static ViewLocations Load()
        {
            if (Properties.Settings.Default.ViewLocations != null)
            {
                return Properties.Settings.Default.ViewLocations;
            }
            else
            {
                return new ViewLocations();
            }
            
        }

        public void Save()
        {
            Properties.Settings.Default.ViewLocations = this;
            Properties.Settings.Default.Save();
        }
    }
}
