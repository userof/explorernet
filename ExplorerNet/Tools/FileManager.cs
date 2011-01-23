using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using ShellBasics;
using ShellLib;

namespace ExplorerNet.Tools
{
    public class FileManager
    {
        private string[] FilesToStrings(List<FileSystemInfo> files)
        {
            List<string> result = new List<string>();

            foreach (var fsi in files)
            {
                result.Add(fsi.FullName);
            }

            return result.ToArray();
        }

        private string[] GetTargetPathes(List<FileSystemInfo> from, DirectoryInfo to)
        {
            List<string> result = new List<string>();

            foreach (var fsi in from)
            {
                result.Add(to.FullName + Path.DirectorySeparatorChar + fsi.Name);
            }

            return result.ToArray();


        }

        public void Copy(List<FileSystemInfo> from, DirectoryInfo to)
        {
            string[] sFrom = FilesToStrings(from);
            string[] sTo = GetTargetPathes(from, to);

            ShellLib.ShellFileOperation fo = new ShellLib.ShellFileOperation();

            fo.Operation = ShellLib.ShellFileOperation.FileOperations.FO_COPY;
            fo.SourceFiles = sFrom;
            fo.DestFiles = sTo;
            fo.DoOperation();
        }

        public void Move(List<FileSystemInfo> from, DirectoryInfo to)
        {
            string[] sFrom = FilesToStrings(from);
            string[] sTo = GetTargetPathes(from, to);

            ShellLib.ShellFileOperation fo = new ShellLib.ShellFileOperation();

            fo.Operation = ShellLib.ShellFileOperation.FileOperations.FO_MOVE;
            fo.SourceFiles = sFrom;
            fo.DestFiles = sTo;
            fo.DoOperation();
        }

        public void Delete(List<FileSystemInfo> from)
        {
            string[] sFrom = FilesToStrings(from);

            ShellFileOperation.ShellFileOperationFlags flags = ShellFileOperation.ShellFileOperationFlags.FOF_NOCONFIRMATION |              
                ShellFileOperation.ShellFileOperationFlags.FOF_SILENT;

            ShellLib.ShellFileOperation fo = new ShellLib.ShellFileOperation();

            fo.OperationFlags = flags;
            
            fo.Operation = ShellLib.ShellFileOperation.FileOperations.FO_DELETE;
            fo.SourceFiles = sFrom;
            fo.DoOperation();
        }

        public void DeleteToRecycler(List<FileSystemInfo> from)
        {
            string[] sFrom = FilesToStrings(from);

            ShellFileOperation.ShellFileOperationFlags flags = ShellFileOperation.ShellFileOperationFlags.FOF_NOCONFIRMATION |
                ShellFileOperation.ShellFileOperationFlags.FOF_SILENT | ShellFileOperation.ShellFileOperationFlags.FOF_ALLOWUNDO;

            ShellLib.ShellFileOperation fo = new ShellLib.ShellFileOperation();

            fo.OperationFlags = flags;
            fo.Operation = ShellLib.ShellFileOperation.FileOperations.FO_DELETE;
            fo.SourceFiles = sFrom;
            fo.DoOperation();
        }


    }
}
