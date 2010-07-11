using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.ViewWindowApps.Templates
{
    [Serializable]
    public class LevelTemplate
    {

        private List<FilePanelTemplate> filePanels = null;

        public LevelTemplate()
        {
            filePanels = new List<FilePanelTemplate>();
        }
    }
}
