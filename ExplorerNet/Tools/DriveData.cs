using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Media.Imaging;


namespace ExplorerNet.Tools
{

    /// <summary>
    /// Класс, предназначенный для предоставления информации о 
    /// логическом диске в формате, удобном для отображения на панелях
    /// </summary>
    public class DriveData
    {
        private DriveInfo drive = null;

        public DriveData(DriveInfo drive)
        {
            this.drive = drive;
        }

        /// <summary>
        /// Возвращает размер диска в единицах информации зависящих от размера и с 
        /// выводом типа единиц измерения. 
        /// В случае, если устройство не готово, возвращается пустая строка
        /// </summary>
        public string TotalSizeShort
        {
            get
            {
                if (this.drive.IsReady)
                {
                    return SizeFileInString.GetSizeInStr(this.drive.TotalSize);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Возвращает размер диска в байтах. 
        /// В случае, если устройство не готово, возвращается пустая строка
        /// </summary>
        public string TotalSizeLong
        {
            get
            {
                if (this.drive.IsReady)
                {
                    return this.drive.TotalSize.ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Возвращает размер доступного пространства диска в единицах информации 
        /// зависящих от размера и с выводом типа единиц измерения. 
        /// В случае, если устройство не готово, возвращается пустая строка
        /// </summary>
        public string AvailableSizeShort
        {
            get
            {
                if (this.drive.IsReady)
                {
                    return SizeFileInString.GetSizeInStr(this.drive.AvailableFreeSpace);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Возвращает размер доступного пространства диска в байтах. 
        /// В случае, если устройство не готово, возвращается пустая строка
        /// </summary>
        public string AvailableSizeLong
        {
            get
            {
                if (this.drive.IsReady)
                {
                    return this.drive.AvailableFreeSpace.ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Возвращает полный размер диска и размер свободного пространства в форматированном виде.
        /// В случае, если устройство не готово, возвращается пустая строка
        /// </summary>
        public string LongSize
        {
            get
            {
                if (this.drive.IsReady)
                {
                    return string.Format("(Free: {0} from {1} ({2} from {3}))",
                        AvailableSizeShort, TotalSizeShort,
                        AvailableSizeLong, TotalSizeLong);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Возвращает метку диска.
        /// В случае, если устройство не готово, возвращается пустая строка
        /// </summary>
        public string VolumeLabel
        {
            get
            {
                if (this.drive.IsReady)
                {
                    return this.drive.VolumeLabel;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Возвращает полный размер диска. В случае, если устройство не готово, возвращает 0
        /// </summary>
        public long MaximumSize
        {
            get
            {
                if (this.drive.IsReady)
                {
                    return this.drive.TotalSize;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Возвращает размер доступного пространства. 
        /// В случае, если устройство не готово, возвращает 0
        /// </summary>
        public long AvailableFreeSpace
        {
            get
            {
                if (this.drive.IsReady)
                {
                    return this.drive.AvailableFreeSpace;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// В случае, если устройство не готово, 
        /// возвращает сообщение об этом в зависимости от установленного языка
        /// </summary>
        public string IsDriveNotReady
        {
            get
            {
                return Properties.Resources.IsDriveNotReady;
            }
        }

        /// <summary>
        /// Возвращает букву диска в один символ
        /// </summary>
        public string DriveLetter
        {
            get
            {
                return this.drive.Name[0].ToString();
            }
        }

        /// <summary>
        /// Возвращает иконку для данного типа диска
        /// </summary>
        public BitmapImage DriveIcon
        {
            get
            {
                BitmapImage result = null;

                switch (this.drive.DriveType)
                {
                    case DriveType.CDRom:
                        result = new BitmapImage(new Uri("/Icons/drive_cdrom.ico", UriKind.Relative));
                        break;
                    case DriveType.Fixed:
                        result = new BitmapImage(new Uri("/Icons/drive_fixed.ico", UriKind.Relative));
                        break;
                    case DriveType.Network:
                        result = new BitmapImage(new Uri("/Icons/drive_network.ico", UriKind.Relative));
                        break;
                    case DriveType.NoRootDirectory:
                        break;
                    case DriveType.Ram:
                        break;
                    case DriveType.Removable:
                        result = new BitmapImage(new Uri("/Icons/drive_removable.ico", UriKind.Relative));
                        break;
                    case DriveType.Unknown:
                        result = new BitmapImage(new Uri("/Icons/drive_unknown.ico", UriKind.Relative));
                        break;
                    default:
                        break;
                }

                return result;
            }
        }

        /// <summary>
        /// Возвращает объект диска, с которым работает данный класс
        /// </summary>
        public DriveInfo Drive
        {
            get
            {
                return this.drive;
            }
        }


    }
}
