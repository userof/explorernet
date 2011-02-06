using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using ExplorerNet.Tools.LastStartedFiles;

namespace ExplorerNet.Tools
{
    public class FileStarter
    {
        public void StartFile(string fullName)
        {
            Process process = new System.Diagnostics.Process();
            process.StartInfo.WorkingDirectory =
                System.IO.Path.GetDirectoryName(fullName);
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = fullName;
            process.Start();

            LastStartedFilesManager lsfm =
                new LastStartedFilesManager();
            lsfm.AddlastStartedFile(fullName);
        }

        public static void Start(string fullName)
        {
            FileStarter fs = new FileStarter();
            fs.StartFile(fullName);
        }
    }
}
