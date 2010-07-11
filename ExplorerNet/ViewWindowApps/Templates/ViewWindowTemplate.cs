using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.ViewWindowApps.Templates
{
    [Serializable]
    public class ViewWindowTemplate
    {
        private List<LevelTemplate> levels = null;

        public ViewWindowTemplate()
        {
            levels = new List<LevelTemplate>();
        }
    }
}
