using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;
using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.CoverExts;

namespace ExplorerNet.Tools.Sorting
{
    /// <summary>
    /// It's abstract class provide method StarCompar that sorting on base star
    /// </summary>
    internal abstract class BaseComparer : IComparer<CustomFileSystemCover>
    {
        /// <summary>
        /// Support the method Compare in IComparer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public abstract int Compare(CustomFileSystemCover x, CustomFileSystemCover y);

        /// <summary>
        /// Compare on base opacity of stars
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected int StarCompare(CustomFileSystemCover x, CustomFileSystemCover y)
        {
            StarKind? skX = x.Star;
            StarKind? skY = y.Star;

            if (skX == null)
            {
                if (skY == null)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (skY == null)
                {
                    return -1;
                }
                else
                {
                    StarKind skXF = skX.GetValueOrDefault(StarKind.One);
                    StarKind skYF = skY.GetValueOrDefault(StarKind.One);
                    int c = skXF.CompareTo(skYF);
                    int result = c;


                    switch (c)
                    {
                        case -1:
                            result = 1;
                            break;
                        case 1:
                            result = -1;
                            break;
                        case 0:
                            result = 0;
                            break;
                    }

                    return result;
                }
            }
        }

    }
}
