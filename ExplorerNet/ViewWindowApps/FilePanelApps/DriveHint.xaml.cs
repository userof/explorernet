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
using ExplorerNet.Tools;

namespace ExplorerNet.ViewWindowApps.FilePanelApps
{
    /// <summary>
    /// Логика взаимодействия для DriveHint.xaml
    /// </summary>
    public partial class DriveHint : UserControl
    {
        private DriveInfo drive = null;

        public DriveHint(DriveInfo drive)
        {
            InitializeComponent();
            this.drive = drive;
        }

        /// <summary>
        /// Заполняем элементы управления данными
        /// </summary>
        private void ShowDriveData()
        {
            string totalShort = SizeFileInString.GetSizeInStr(drive.TotalSize);
            string totalLong = drive.TotalSize.ToString();

            string availableShort = SizeFileInString.GetSizeInStr(drive.AvailableFreeSpace);
            string availableLong = drive.AvailableFreeSpace.ToString();

            txtSize.Text = string.Format("Free: {0} from {1} ({2} from {3})", availableShort, totalShort, availableLong, totalLong);

            pbSize.Maximum = drive.TotalSize;
            pbSize.Value = drive.TotalSize - drive.AvailableFreeSpace;

            txtInfo.Text = string.Format("name:{0} label:{1} type:{2}",
                drive.Name[0].ToString(), drive.VolumeLabel, drive.DriveType);

        }


        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ShowDriveData();
        }


    }
}
