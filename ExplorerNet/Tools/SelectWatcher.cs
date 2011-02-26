using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;
using ExplorerNet.ViewWindowApps;

namespace ExplorerNet.Tools
{
    /// <summary>
    /// Данный класс отслеживает выделения в файловых панелях
    /// </summary>
    internal class SelectWatcher
    {
        private static SelectWatcher instance = null;

        public static SelectWatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SelectWatcher();
                }
                return instance;
            }

        }

        public delegate void ChangeSelectedEventHandler(List<CustomFileSystemCover> files, 
            FilePanel activeFilePanel);

        public event ChangeSelectedEventHandler ChangeSelected;

        public void Change(List<CustomFileSystemCover> files, FilePanel activeFilePanel)
        {
            if (ChangeSelected != null)
            {
                ChangeSelected(files, activeFilePanel);
            }
        }
    }
}
