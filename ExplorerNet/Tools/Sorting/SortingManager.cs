using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;

namespace ExplorerNet.Tools.Sorting
{
    /// <summary>
    /// Use this class for sorting the files (List<CustomFileSystemCover>)
    /// </summary>
    internal static class SortingManager
    {
        public delegate void SortingEventHandler(SortingData data);

        public static event SortingEventHandler BeforeSorting;

        public static event SortingEventHandler AfterSorting;

        /// <summary>
        /// Sorting files
        /// </summary>
        /// <param name="list">list of files</param>
        /// <param name="kind">Kind sorting</param>
        /// <example>
        /// 
        /// List<CustomFileSystemCover> list = new List<CustomFileSystemCover>();
        /// SortingManager.AfterSorting += new SortingEventHandler(SortingManager_AfterSorting);
        /// SortingManager.BeforeSorting += new SortingEventHandler(SortingManager_BeforeSorting);
        /// SortingManager.Sort(list, SortingKind.NameDown);
        /// 
        /// </example>
        public static void Sort(List<CustomFileSystemCover> list, SortingKind kind)
        {
            SortingData sd = new SortingData(list, kind);

            //sending that sorting to be start
            if (BeforeSorting != null)
            {
                BeforeSorting(sd);
            }

            //start sorting differenc kind
            switch (kind)
            {
                case SortingKind.NameUp:
                    list.Sort(new NameUpComparer());
                    break;
                case SortingKind.NameDown:
                    //not implemented yet
                    new NotImplementedException();
                    break;
                case SortingKind.SizeUp:
                    //not implemented yet
                    new NotImplementedException();
                    break;
                case SortingKind.SizeDown:
                    //not implemented yet
                    new NotImplementedException();
                    break;
                default:
                    break;
            }

            //sending that sorting compleated
            if (AfterSorting != null)
            {
                AfterSorting(sd);
            }

        }
    }
}
