using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;

namespace ExplorerNet.Tools.ViewSettings
{
    [Serializable]
    public class ViewLocation
    {
        public double Height
        {
            get;
            set;
        }

        public double Width
        {
            get;
            set;
        }

        public double Top
        {
            get;
            set;
        }

        public double Left
        {
            get;
            set;
        }

        public string TypeInString
        {
            get;
            set;
        }

        public static void SaveWindowLocation(Window window)
        {
            ViewLocations vLocations = ViewLocations.Load();
            string typeString = window.GetType().ToString();

            ViewLocation remLoc = null;
            foreach (var vl in vLocations)
            {
                if (vl.TypeInString == typeString)
                {
                    //vLocations.Remove(vl);
                    remLoc = vl;
                }
            }

            if (remLoc != null)
            {
                vLocations.Remove(remLoc);
            }

            ViewLocation vLoc = new ViewLocation();
            vLoc.TypeInString = typeString;
            vLoc.Left = window.Left;
            vLoc.Top = window.Top;
            vLoc.Height = window.Height;
            vLoc.Width = window.Width;

            vLocations.Add(vLoc);
            vLocations.Save();
        }

        public static void LoadWindowLocation(Window window)
        {
            ViewLocations vLocations = ViewLocations.Load();
            string typeString = window.GetType().ToString();

            foreach (var vl in vLocations)
            {
                if (typeString == vl.TypeInString)
                {
                    window.Left = vl.Left;
                    window.Top = vl.Top;
                    window.Height = vl.Height;
                    window.Width = vl.Width;
                }
            }
        }
    }
}
