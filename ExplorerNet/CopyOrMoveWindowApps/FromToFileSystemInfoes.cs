using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ExplorerNet.CopyOrMoveWindowApps
{
    public class FromToFileSystemInfoes : List<FromToFileSystemInfo>
    {
        public FromToFileSystemInfoes(List<FileSystemInfo> sourceList, DirectoryInfo toCopy)
        {
            foreach (var fsi in sourceList)
            {
                FromToFileSystemInfo ffsi = new FromToFileSystemInfo(fsi, toCopy);
                this.Add(ffsi);
            }
        }
    }
}
