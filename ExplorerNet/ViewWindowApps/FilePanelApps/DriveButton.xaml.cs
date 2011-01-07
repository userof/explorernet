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
    /// Логика взаимодействия для DriveButton.xaml
    /// </summary>
    public partial class DriveButton : RadioButton
    {
        private DriveInfo currentDrive = null;

        public DriveButton(DriveInfo drive)
        {
            InitializeComponent();

            this.currentDrive = drive;

            txtDriveName.Text = drive.Name[0].ToString();

            ShowDriveData(drive);

            //switch (drive.DriveType)
            //{
            //    case DriveType.CDRom:
            //        imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_cdrom.ico", UriKind.Relative));
            //        break;
            //    case DriveType.Fixed:
            //        imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_fixed.ico", UriKind.Relative));
            //        break;
            //    case DriveType.Network:
            //        imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_network.ico", UriKind.Relative));
            //        break;
            //    case DriveType.NoRootDirectory:
            //        break;
            //    case DriveType.Ram:
            //        break;
            //    case DriveType.Removable:
            //        imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_removable.ico", UriKind.Relative));
            //        break;
            //    case DriveType.Unknown:
            //        imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_unknown.ico", UriKind.Relative));
            //        break;
            //    default:
            //        break;
            //}
        }

        private void ShowDriveData(DriveInfo drive)
        {
            DriveData driveData = new DriveData(drive);

            txtSize.Text = driveData.LongSize;
            txtLabel.Text = driveData.VolumeLabel;

            

            try
            {
                pbSize.Maximum = driveData.MaximumSize;

                pbSize.Value = driveData.MaximumSize -
                    driveData.AvailableFreeSpace;
            }
            catch (Exception)
            {
                
            }

            imgDriveType.Source = driveData.DriveIcon;


            //if (drive.IsReady)
            //{
            //    string totalShort = SizeFileInString.GetSizeInStr(drive.TotalSize);
            //    string totalLong = drive.TotalSize.ToString();

            //    string availableShort = SizeFileInString.GetSizeInStr(drive.AvailableFreeSpace);
            //    string availableLong = drive.AvailableFreeSpace.ToString();

            //    txtSize.Text = string.Format("(Free: {0} from {1} ({2} from {3}))", availableShort, totalShort, availableLong, totalLong);

            //    txtLabel.Text = drive.VolumeLabel;

            //    pbSize.Maximum = drive.TotalSize;

            //    try
            //    {
            //        pbSize.Value = drive.TotalSize - drive.AvailableFreeSpace;
            //    }
            //    catch (Exception)
            //    {

            //    }

            //}
            //else
            //{

            //}

            //switch (drive.DriveType)
            //{
            //    case DriveType.CDRom:
            //        imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_cdrom.ico", UriKind.Relative));
            //        break;
            //    case DriveType.Fixed:
            //        imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_fixed.ico", UriKind.Relative));
            //        break;
            //    case DriveType.Network:
            //        imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_network.ico", UriKind.Relative));
            //        break;
            //    case DriveType.NoRootDirectory:
            //        break;
            //    case DriveType.Ram:
            //        break;
            //    case DriveType.Removable:
            //        imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_removable.ico", UriKind.Relative));
            //        break;
            //    case DriveType.Unknown:
            //        imgDriveType.Source = new BitmapImage(new Uri("/Icons/drive_unknown.ico", UriKind.Relative));
            //        break;
            //    default:
            //        break;
            //}

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.IsChecked = true;
        }
    }
}
