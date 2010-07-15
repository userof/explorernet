﻿using System;
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

using System.IO.Tools; 
//using ExplorerNet.ViewWindowApps.FilePanelApps.IO.Tools;

namespace ExplorerNet.ViewWindowApps
{
    /// <summary>
    /// Interaction logic for FilePanel.xaml
    /// </summary>
    public partial class FilePanel : UserControl
    {
        public FilePanel()
        {
            InitializeComponent();
            //Загружаем сохранённую ширину
            this.Width = Properties.Settings.Default.WidthFilepanel;

            _BuildDrives();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.Width.ToString());
        }

        private void _BuildDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            ugDrives.Children.Clear();

            foreach (var d in drives)
            {
                Button btnDrive = new Button();
                btnDrive.Content = d.Name;

                btnDrive.Click += new RoutedEventHandler(btnDrive_Click);
                btnDrive.MouseRightButtonDown += new MouseButtonEventHandler(btnDrive_MouseRightButtonDown);

                btnDrive.Tag = d;
                ugDrives.Children.Add(btnDrive);
            }
        }

        private void btnDrive_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void btnDrive_MouseRightButtonDown(Object sender, MouseButtonEventArgs e)
        {
            Button btnDrive = (Button)sender;
            DriveInfo drive = (DriveInfo)btnDrive.Tag;

            DirectoryInfo dir = drive.RootDirectory;

            FileSystemInfoEx de = new DirectoryInfoEx(dir.FullName);

            ContextMenuWrapper cmw = new ContextMenuWrapper();

            Point pt = this.PointToScreen(e.GetPosition(this));
            cmw.Popup(new FileSystemInfoEx[] { de }, new System.Drawing.Point((int)pt.X, (int)pt.Y)); 
        }

        private void tmbMove_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            this.Width += e.HorizontalChange;
        }

        private void btnDeleteFilePanel_Click(object sender, RoutedEventArgs e)
        {
            Panel panel = (Panel)this.Parent;
            panel.Children.Remove(this);
        }
	



    }
}
