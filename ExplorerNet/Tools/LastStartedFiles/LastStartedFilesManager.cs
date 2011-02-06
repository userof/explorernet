using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.Tools.LastStartedFiles
{
    public class LastStartedFilesManager
    {
        public void AddlastStartedFile(string path)
        {
            LastStartedFiles lsfs = LastStartedFiles.Load();
            LastStartedFile lsf = new LastStartedFile(path);


            List<LastStartedFile> rem = new List<LastStartedFile>();
            foreach (var l in lsfs)
            {
                if (l.Path == lsf.Path)
                {
                    rem.Add(l);
                }
            }

            foreach (var l in rem)
            {
                lsfs.Remove(l);
            }

            lsfs.Insert(0, lsf);
            lsfs.Save();
        }

        public void Clear()
        {
            LastStartedFiles lsfs = LastStartedFiles.Load();
            lsfs.Clear();
            lsfs.Save();

        }


    }

    //public class LastStartedFileComparer : IEqualityComparer<LastStartedFile>
    //{
    //    public bool Equals(LastStartedFile lf1, LastStartedFile lf2)
    //    {
    //        if (lf1.Path == lf2.Path)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }


    //    public int GetHashCode(LastStartedFile lsf)
    //    {
    //        return lsf.Path.GetHashCode();
    //    }

    //}
}
