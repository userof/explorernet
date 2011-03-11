using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

using ExplorerNet.Tools.ExtentionAnaliz;

using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;

namespace ExplorerNet.ViewWindowApps.FilePanelApps
{
    /// <summary>
    /// Логика взаимодействия для PreviewPanel.xaml
    /// </summary>
    public partial class PreviewPanel : UserControl
    {
        private string currentFile = "";

        private void SetCurrentFile(string fileName)
        {
            currentFile = fileName;
            txtFilePath.Text = fileName;
        }

        public PreviewPanel()
        {
            InitializeComponent();
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

            string ext = System.IO.Path.GetExtension(fileName).ToLower();

            Extentions exts = Extentions.Load();

            if (exts.Keys.Contains(ext))
            {
                var c = from entry in exts
                        where (entry.Key == ext)
                        select entry.Value;



                var previewKind = (PreviewKind)c.First();


                //PreviewKind previewKind = exts.First( .Values[ext];

                switch (previewKind)
                {
                    case PreviewKind.Media:
                        MediaStartPreview(fileName);
                        break;
                    //case PreviewKind.Web:
                    //    WebStartPreview(fileName);
                    //    break;
                    default:
                        
                        //MessageBox.Show("not");
                        break;

                }

                
            }
            else
            {
                TextStartPreview(fileName);
            }

            SetCurrentFile(fileName);

        }

        private void MediaStartPreview(string fileName)
        {
            HideAllStartPrview();

            grdMediaPlayer.Visibility = System.Windows.Visibility.Visible;
            //grdWebBrouzer.Visibility = System.Windows.Visibility.Hidden;
            mediaPlayerMain.Source = new Uri(fileName);
            sliderTime.Value = 0;
            mediaPlayerMain.Play();

            //MediaVisibility = Visibility.Visible;
            //PreviewMediaFile = fileName;
            //WebVisibility = Visibility.Hidden;
        }

        private void HideAllStartPrview()
        {
            mediaPlayerMain.Stop();
            grdMediaPlayer.Visibility = System.Windows.Visibility.Hidden;
            //grdWebBrouzer.Visibility = System.Windows.Visibility.Hidden;
            grdTextViewer.Visibility = System.Windows.Visibility.Hidden;  
        }

        private void TextStartPreview(string fileName)
        {
            HideAllStartPrview();

            FlowDocument document = new FlowDocument();
            TextRange txtRange = null;

            int maxLoadFileSize = 10000;

            FileInfo fi = new FileInfo(fileName);
            if (fi.Length < maxLoadFileSize)
            {
                maxLoadFileSize = (int)fi.Length;
            }

            //if (File.si)

            var fs = File.OpenRead(fileName);
            byte[] bytes = new byte[maxLoadFileSize];
            fs.Read(bytes, 0, maxLoadFileSize);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                txtRange = new TextRange(document.ContentStart, document.ContentEnd);
                txtRange.Load(ms, DataFormats.Text);

                var ff = new FontFamily();
                ff.LineSpacing = 0.1;

                document.FontFamily = ff;
            }

            rtbTextView.Document = document;

            grdTextViewer.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayerMain.Pause();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            sliderTime.Value = 0;
            mediaPlayerMain.Play();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayerMain.Stop();
        }

        //private void WebStartPreview(string fileName)
        //{
        //    //grdMediaPlayer.Visibility = System.Windows.Visibility.Hidden;
        //    HideAllStartPrview();

        //    grdWebBrouzer.Visibility = System.Windows.Visibility.Visible;

        //    //wbWebBrouzer.Source = new Uri(fileName);
        //    wbWebBrouzer.Navigate(new Uri(fileName));
        //    //WebVisibility = Visibility.Visible;
        //    //PreviewWebFile = fileName;
        //    //MediaVisibility = Visibility.Hidden;
        //}

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayerMain.Volume = (double)sliderVolume.Value;
        }

        private void sliderTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int SliderValue = (int)sliderTime.Value;
            // Overloaded constructor takes the arguments days, hours, minutes, seconds, miniseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            mediaPlayerMain.Position = ts;
        }

        private void mediaPlayerMain_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (mediaPlayerMain.NaturalDuration.HasTimeSpan)
            {
                sliderTime.Maximum = mediaPlayerMain.NaturalDuration.TimeSpan.TotalMilliseconds;
            }
            sliderTime.IsEnabled = mediaPlayerMain.IsLoaded;
            sliderVolume.IsEnabled = mediaPlayerMain.IsLoaded;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(currentFile))
            {
                ExplorerNet.Tools.FileStarter.Start(currentFile);
            }
        }

        private void txtFilePath_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtFilePath.SelectAll();
        }

    }
}
