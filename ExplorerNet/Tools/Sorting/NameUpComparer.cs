using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;
using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.CoverExts;

namespace ExplorerNet.Tools.Sorting
{
    public class NameUpComparer : IComparer<CustomFileSystemCover>
    {
        public int Compare(CustomFileSystemCover x, CustomFileSystemCover y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {

                    if (x is ParentDirectoryCover)
                    {
                        if (y is ParentDirectoryCover)
                        {
                            return 0;
                        }
                        else if ((y is DirectoryCover) || (y is FileCover))
                        {
                            return -1;
                        }
                    }
                    else if (x is DirectoryCover)
                    {
                        if (y is ParentDirectoryCover)
                        {
                            return 1;
                        }
                        else if (y is DirectoryCover)
                        { 
                            int sc = StarCompare(x, y);
                            if (sc == 0)
                            {
                                return x.Name.CompareTo(y.Name);
                            }
                            else
                            {
                                return sc;
                            }
                        }
                        else if (y is FileCover)
                        {
                            int sc = StarCompare(x, y);
                            if (sc == 0)
                            {
                                return -1;
                            }
                            else
                            {
                                return sc;
                            }
                        }
                    }
                    else if (x is FileCover)
                    {
                        if ((y is ParentDirectoryCover))
                        {
                            return 1;
                        }
                        else if (y is DirectoryCover)
                        {
                            int sc = StarCompare(x, y);
                            if (sc == 0)
                            {
                                return 1;
                            }
                            else
                            {
                                return sc;
                            }
                        }
                        else if (y is FileCover)
                        {
                            int sc = StarCompare(x, y);
                            if (sc == 0)
                            {
                                return x.Name.CompareTo(y.Name);
                            }
                            else
                            {
                                return sc;
                            }
                        }
                    }
                }
            }

            return -1;
        }

        private int StarCompare(CustomFileSystemCover x, CustomFileSystemCover y)
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
                            result = - 1;
                            break;
                        case 0:
                            result = 0;
                            break;
                    }

                    return result;
                }
            }

            //return -1;
        }
    }
}
