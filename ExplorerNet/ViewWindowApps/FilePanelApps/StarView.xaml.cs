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

using Microsoft.Expression.Shapes;
using ExplorerNet.ViewWindowApps.FilePanelApps.FileSystemCovers.CoverExts;

namespace ExplorerNet.ViewWindowApps.FilePanelApps
{
    /// <summary>
    /// Логика взаимодействия для StarView.xaml
    /// </summary>
    public partial class StarView : UserControl
    {

        public event EventHandler Click;

        public static readonly DependencyProperty StarLevelProperty =
            DependencyProperty.Register("StarLevel", typeof(StarKind?),
            typeof(StarView));

        

        public StarKind? StarLevel
        {
            get
            {
                return (StarKind?)GetValue(StarLevelProperty);
            }
            set
            {
                SetValue(StarLevelProperty, value);
            }
        }

        public StarView()
        {
            InitializeComponent();
            //System.Windows.Shapes.
        }

        private void RegularPolygon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            NextIterEnum();

            if (Click != null)
            {
                Click(this, e);
            }
            //rpStar.Opacity = 1;
        }

        private void NextIterEnum()
        {
            if (StarLevel == null)
            {
                StarLevel = StarKind.One;
            }
            else
            {
                switch (StarLevel)
                {
                    case StarKind.One:
                        StarLevel = StarKind.Two;
                        break;
                    case StarKind.Two:
                        StarLevel = StarKind.Three;
                        break;
                    case StarKind.Three:
                        StarLevel = null;
                        break;
                    //case StarKind.Foor:
                    //    StarLevel = StarKind.Five;
                    //    break;
                    //case StarKind.Five:
                    //    StarLevel = null;
                    //    break;
                }
            }


        }

        private void star_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
