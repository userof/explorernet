using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ExplorerNet.Tools.ExtentionAnaliz;
using System.Windows;
using System.Windows.Input;

using ExplorerNet.MVVM.Helper;
using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;

namespace ExplorerNet.MVVM.ViewModel
{
    internal class PreviewPanelViewModel : BaseViewModel
    {

        #region data
        private Visibility mediaVisibility = Visibility.Hidden;

        public Visibility MediaVisibility
        {
            get { return mediaVisibility; }
            set 
            { 
                mediaVisibility = value;
                OnPropertyChanged(() => MediaVisibility);
            }
        }

        private Visibility webVisibility = Visibility.Hidden;

        public Visibility WebVisibility
        {
            get { return webVisibility; }
            set 
            {
                webVisibility = value;
                OnPropertyChanged(() => WebVisibility);
            }
        }

        private string previewMediaFile = "";

        public string PreviewMediaFile
        {
            get { return previewMediaFile; }
            set 
            { 
                previewMediaFile = value;
                OnPropertyChanged(() => PreviewMediaFile);
            }
        }

        private double mediaVolume = 0;

        public double MediaVolume
        {
            get { return mediaVolume; }
            set 
            { 
                mediaVolume = value;
                OnPropertyChanged(() => MediaVolume);
            }
        }

        private TimeSpan mediaPosition;

        public TimeSpan MediaPosition
        {
            get { return TimeSpan.FromSeconds(MediaPositionSecond); }
            set 
            {
                MediaPositionSecond = value.Seconds;
            }
        }

        private long mediaPositionSecond = 0;

        public long MediaPositionSecond
        {
            get { return mediaPositionSecond; }
            set 
            { 
                mediaPositionSecond = value;
                OnPropertyChanged(() => MediaPositionSecond);
            }
        }



        private string previewWebFile = "";

        public string PreviewWebFile
        {
            get { return previewWebFile; }
            set
            {
                previewWebFile = value;
                OnPropertyChanged(() => PreviewWebFile);
            }
        }

        #endregion //data

        #region Commands

        public ICommand PlayMediaCommand { get { return new BaseCommand(PlayMedia); } }

        private void PlayMedia()
        {
            //////////
        }

        #endregion

        public delegate void MediaPositionChangedEventHandler(TimeSpan newPosition);

        public event MediaPositionChangedEventHandler MediaPositionChanged;

        public PreviewPanelViewModel()
        {

            if (this.IsInDesignMode)
            {
                MediaVisibility = Visibility.Visible;
            }

            //ExplorerNet.Tools.SelectWatcher.Instance.ChangeSelected += (x, y) =>
            //    {
            //        bool b = true;
            //        int i = 0;
            //        while ((x.Count > i) && b)
            //        {
            //            if (x[i].FileSystemElement.GetType() == typeof(FileInfo))
            //            {
            //                b = false;
            //                PreviewElementStart(x[i].FileSystemElement.FullName);
            //            }
            //            i++;
            //        }
            //    };
        }

        public void PreviewElementStart(List<CustomFileSystemCover> files)
        {
            string fileName = "";
            bool b = true;
            int i = 0;
            while ((files.Count > i) && b)
            {
                if (files[i].FileSystemElement.GetType() == typeof(FileInfo))
                {
                    b = false;
                    fileName = files[i].FileSystemElement.FullName;
                }
                i++;
            }

            if (string.IsNullOrEmpty(fileName)) return;

            string ext = Path.GetExtension(fileName);

            Extentions exts = Extentions.Load();

            if (exts.Keys.Contains(ext))
            {
                var c = from entry in exts where (entry.Key == ext)
                        select entry.Value;



                var previewKind = (PreviewKind)c.First();


                //PreviewKind previewKind = exts.First( .Values[ext];

                switch (previewKind)
                {
                    case PreviewKind.Media:
                        MediaStartPreview(fileName);
                        break;
                    case PreviewKind.Web:
                        WebStartPreview(fileName);
                        break;
                    default:
                        break;
                }
            }

        }

        private void MediaStartPreview(string fileName)
        {
            MediaVisibility = Visibility.Visible;
            PreviewMediaFile = fileName;
            WebVisibility = Visibility.Hidden;
        }

        private void WebStartPreview(string fileName)
        {
            WebVisibility = Visibility.Visible;
            PreviewWebFile = fileName;
            MediaVisibility = Visibility.Hidden;
        }

    }
}
