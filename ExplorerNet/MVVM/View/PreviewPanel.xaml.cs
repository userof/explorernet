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

namespace ExplorerNet.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для PreviewPanel.xaml
    /// </summary>
    public partial class PreviewPanel : UserControl
    {
        public PreviewPanel()
        {
            InitializeComponent();
            //meMediaPlayer
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
