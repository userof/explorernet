using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerNet.ViewWindowApps.FilePanelApps
{
    public static class FilePanelSelector
    {
        private static FilePanel firstSelected = null;

        private static FilePanel secondSelected = null;

        private static List<FilePanel> filePanels = null;

        public delegate void FilePanelChangeSelectEventHandler();

        public static event FilePanelChangeSelectEventHandler FilePanelChangeSelect;

        static FilePanelSelector()
        {
            filePanels = new List<FilePanel>();
        }

        private static void CreateSelected()
        {
            if (filePanels.Count > 0)
            {
                firstSelected = filePanels[0];

            }
            else
            {
                firstSelected = null;
            }

            if (filePanels.Count > 1)
            {
                secondSelected = filePanels[1];
            }
            else
            {
                secondSelected = null;
            }

            if (FilePanelChangeSelect != null)
            {
                FilePanelChangeSelect();
            }
        }

        public static void Add(FilePanel filePanel)
        {
            if (filePanels.Contains(filePanel))
            {
                filePanels.Remove(filePanel);
            }

            filePanels.Insert(0, filePanel);
            CheckFilePanels();
            CreateSelected();
        }

        public static void Remove(FilePanel filePanel)
        {
            filePanels.Remove(filePanel);
            CheckFilePanels();

            CreateSelected();
        }

        private static void CheckFilePanels()
        {
            List<FilePanel> removeList = new List<FilePanel>();

            foreach (var fp in filePanels)
            {
                if (fp == null)
                {
                    removeList.Add(fp);
                }

                //if (!fp.IsVisible)
                //{
                //    removeList.Add(fp);
                //}
            }

            foreach (var rfp in removeList)
            {
                filePanels.Remove(rfp);
            }
        }

        public static FilePanel FirstSelected
        {
            get { return firstSelected; }
        }

        public static FilePanel SecondSelected
        {
            get { return secondSelected; }
        }

        public static List<FilePanel> FilePanels
        {
            get { return filePanels; }
        }
    }

    public enum FilePanelSelectedKind
    {
        First,
        Second,
        Another
    }
}
