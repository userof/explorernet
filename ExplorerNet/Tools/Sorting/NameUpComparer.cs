using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;
using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.CoverExts;

namespace ExplorerNet.Tools.Sorting
{
    internal class NameUpComparer : BaseComparer
    {
        public override int Compare(CustomFileSystemCover x, CustomFileSystemCover y)
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

        
    }
}
