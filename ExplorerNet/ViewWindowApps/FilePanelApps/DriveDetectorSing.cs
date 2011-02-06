using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dolinay;

namespace ExplorerNet.ViewWindowApps.FilePanelApps
{
    /// <summary>
    /// Статический класс, предоставляющий экземпляр класса DriveDetector
    /// </summary>
    public static class DriveDetectorSing
    {
        /// <summary>
        /// Возвращает экземпляр класса DriveDetector
        /// </summary>
        private static DriveDetector driveDetector = null;

        /// <summary>
        /// Возвращает экземпляр класса DriveDetector
        /// </summary>
        public static DriveDetector DriveDetectorProp
        {
            get
            {
                if (driveDetector == null)
                {
                    driveDetector = new DriveDetector();
                }
                return driveDetector;
            }
        }

    }
}
