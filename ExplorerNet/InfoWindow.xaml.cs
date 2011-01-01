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
using System.Windows.Shapes;

using System.Diagnostics;

namespace ExplorerNet
{
    /// <summary>
    /// Логика взаимодействия для InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
        }

        private void Paragraph_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://explorernetprototype.wordpress.com/2010/12/16/%D0%BE%D0%B1%D1%81%D1%83%D0%B6%D0%B4%D0%B5%D0%BD%D0%B8%D0%B5-explorer-net/");
        }

        private void Paragraph_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://mail.mihanik.net/IceFox/");
        }

        private void Paragraph_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://forum.vingrad.ru/forum/forum-635.html");
        }
    }
}
