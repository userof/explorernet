using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using System.IO;

namespace ExplorerNet.Tools.LastStartedFiles
{
    [Serializable]//System.Collections.ObjectModel.ObservableCollection<LastStartedFile>
    public class LastStartedFiles : ObservableCollection<LastStartedFile>
    {
        public LastStartedFiles()
            : base()
        {

        }

        public static LastStartedFiles Load()
        {
            if (Properties.Settings.Default.LastStartedFiles != null)
            {
                return Properties.Settings.Default.LastStartedFiles;
            }
            else
            {
                return new LastStartedFiles();
            }
        }

        public void Save()
        {
            Check();
            Properties.Settings.Default.LastStartedFiles = this;
            Properties.Settings.Default.Save();
        }

        private void Check()
        {
            var rem = new ObservableCollection<LastStartedFile>();

            foreach (var lsf in this)
            {
                if (!File.Exists(lsf.Path))
                {
                    rem.Add(lsf);
                }
            }

            foreach (var lsf in rem)
            {
                this.Remove(lsf);
            }
        }
    }
}
