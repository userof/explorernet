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

using ExplorerNet.MVVM.ViewModel;
using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers;

namespace ExplorerNet.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для PreviewPanel.xaml
    /// </summary>
    public partial class PreviewPanel : UserControl
    {
        private int mediaPositionTemp = 0;

        public static readonly DependencyProperty MediaPositionProperty =
            DependencyProperty.Register("MediaPosition", typeof(TimeSpan),
            typeof(PreviewPanel));

        /// <summary>
        /// Свойство зависимостей, идентифицирующее панель первого выделение
        /// </summary>
        public TimeSpan MediaPosition
        {
            get
            {
                return (TimeSpan)GetValue(MediaPositionProperty);
            }
            set
            {
                SetValue(MediaPositionProperty, value);
            }
        }


        public PreviewPanel()
        {
            InitializeComponent();
            //meMediaPlayer
        }

        public void ShowPreview(List<CustomFileSystemCover> files)
        {
             var vm = (PreviewPanelViewModel)this.Resources["viewModel"];
             vm.PreviewElementStart(files);

             //meMediaPlayer.Position.TotalSeconds
            //vm.MediaPositionChanged +=
 
        }

        private void sdMediaPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPositionTemp = (int)sdMediaPosition.Value;

        }

        private void btnPlayMedia_Click(object sender, RoutedEventArgs e)
        {

            meMediaPlayer.Stop();
        }

        private void meMediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            lbLongTime.Text = meMediaPlayer.NaturalDuration.TimeSpan.ToString();
            sdMediaPosition.Value = 0;
            sdMediaPosition.Maximum = 
                meMediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
            sdMediaPosition.IsEnabled = meMediaPlayer.IsLoaded;
            sdMediaVolume.IsEnabled = meMediaPlayer.IsLoaded;
        }

        private void sdMediaPosition_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, mediaPositionTemp);
            meMediaPlayer.Position = ts;
        }

        private void meMediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            //sdMediaPosition.Value = 0;
        }



        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    //wbWebViwer.
        //   // wbWebViwer.Navigate(@"c:\HDMI.log");
        //    //meMediaPlayer.LoadedBehavior = MediaState.Manual;
        //    //meMediaPlayer.Source = new Uri(@"h:\video\3 NewYear.avi");
        //    //meMediaPlayer.Play();
        //    LoadTextDocument(@"e:\temp\HDMI.log");
        //}

        //private void LoadTextDocument(string fileName)
        //{
        //    TextRange range;
        //    System.IO.FileStream fStream;
        //    if (System.IO.File.Exists(fileName))
        //    { 
        //        range = new TextRange(rt.Document.ContentStart, rt.Document.ContentEnd);
        //        fStream = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate);
        //        range.Load(fStream, System.Windows.DataFormats.Text);
        //        fStream.Close();
        //    }
        //}

    }
}
