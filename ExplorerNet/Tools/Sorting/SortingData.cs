using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;

namespace ExplorerNet.Tools.Sorting
{

    public class SortingData
    {
        public SortingKind Kind { get; set; }

        public List<CustomFileSystemCover> SortingList { get; set; }

        private SortingData() { }

        public SortingData(List<CustomFileSystemCover> sortingList, SortingKind sortingKind)
        {
            Kind = sortingKind;
            SortingList = sortingList;
        }
    }
}
