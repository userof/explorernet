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
            if (drive.IsReady)
            {
                string totalShort = SizeFileInString.GetSizeInStr(drive.TotalSize);
                string totalLong = drive.TotalSize.ToString();

                string availableShort = SizeFileInString.GetSizeInStr(drive.AvailableFreeSpace);
                string availableLong = drive.AvailableFreeSpace.ToString();

                txtSize.Text = string.Format("Free: {0} from {1} ({2} from {3})", availableShort, totalShort, availableLong, totalLong);

                pbSize.Maximum = drive.TotalSize;

                try
                {
                    pbSize.Value = drive.TotalSize - drive.AvailableFreeSpace;
                }
                catch (Exception)
                {

                }
                

                txtInfo.Text = string.Format("name:{0} label:{1} type:{2}",
                    drive.Name[0].ToString(), drive.VolumeLabel, drive.DriveType);

            }
            else
            {
                txtInfo.Text = string.Format("name:{0} type:{1}",
                    drive.Name[0].ToString(), drive.DriveType);
                txtSize.Text = "The drive is not read";
            }

            switch (drive.DriveType)
            {
                case DriveType.CDRom:
                    imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_cdrom.ico", UriKind.Relative));
                    break;
                case DriveType.Fixed:
                    imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_fixed.ico", UriKind.Relative));
                    break;
                case DriveType.Network:
                    imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_network.ico", UriKind.Relative));
                    break;
                case DriveType.NoRootDirectory:
                    break;
                case DriveType.Ram:
                    break;
                case DriveType.Removable:
                    imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_removable.ico", UriKind.Relative));
                    break;
                case DriveType.Unknown:
                    imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_unknown.ico", UriKind.Relative));
                    break;
                default:
                    break;
            }

        }


        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ShowDriveData();
        }


    }
}
