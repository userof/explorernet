using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace ExplorerNet.Tools
{
    public class FileManager
    {
        // SHFileOperation flags
        private const int FO_MOVE = 0x0001;
        private const int FO_COPY = 0x0002;
        private const int FO_DELETE = 0x0003;

        private const int FOF_MULTIDESTFILES = 0x0001;
        private const int FOF_SILENT = 0x0004;
        private const int FOF_NOCONFIRMATION = 0x0010;
        private const int FOF_ALLOWUNDO = 0x0040;
        private const int FOF_NOERRORUI = 0x0400;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public int wFunc;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pTo;
            public ushort fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszProgressTitle;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

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

        private static string JoinPaths(string[] paths)
        {
            return string.Join("\0", paths) + "\0\0";
        }

        public void Copy(List<FileSystemInfo> from, DirectoryInfo to)
        {
            string[] sFrom = FilesToStrings(from);
            string[] sTo = GetTargetPathes(from, to);

            SHFILEOPSTRUCT fileOp = new SHFILEOPSTRUCT();
            fileOp.wFunc = FO_COPY;
            fileOp.pFrom = JoinPaths(sFrom);
            fileOp.pTo = JoinPaths(sTo);
            fileOp.fFlags = (ushort)FOF_MULTIDESTFILES;
            SHFileOperation(ref fileOp);
        }

        public void Move(List<FileSystemInfo> from, DirectoryInfo to)
        {
            string[] sFrom = FilesToStrings(from);
            string[] sTo = GetTargetPathes(from, to);

            SHFILEOPSTRUCT fileOp = new SHFILEOPSTRUCT();
            fileOp.wFunc = FO_MOVE;
            fileOp.pFrom = JoinPaths(sFrom);
            fileOp.pTo = JoinPaths(sTo);
            fileOp.fFlags = (ushort)FOF_MULTIDESTFILES;
            SHFileOperation(ref fileOp);
        }

        public void Delete(List<FileSystemInfo> from)
        {
            string[] sFrom = FilesToStrings(from);

            SHFILEOPSTRUCT fileOp = new SHFILEOPSTRUCT();
            fileOp.wFunc = FO_DELETE;
            fileOp.pFrom = JoinPaths(sFrom);
            fileOp.fFlags = (ushort)(FOF_NOCONFIRMATION | FOF_SILENT);
            SHFileOperation(ref fileOp);
        }

        public void DeleteToRecycler(List<FileSystemInfo> from)
        {
            string[] sFrom = FilesToStrings(from);

            SHFILEOPSTRUCT fileOp = new SHFILEOPSTRUCT();
            fileOp.wFunc = FO_DELETE;
            fileOp.pFrom = JoinPaths(sFrom);
            fileOp.fFlags = (ushort)(FOF_NOCONFIRMATION | FOF_SILENT | FOF_ALLOWUNDO);
            SHFileOperation(ref fileOp);
        }
    }
}
