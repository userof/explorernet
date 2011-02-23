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

namespace ExplorerNet.ViewWindowApps.FilePanelApps
{
    /// <summary>
    /// Логика взаимодействия для NoteView.xaml
    /// </summary>
    public partial class NoteView : UserControl
    {

        public event EventHandler Click;

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string),
            typeof(NoteView));



        public string Description
        {
            get
            {
                return (string)GetValue(DescriptionProperty);
            }
            set
            {
                SetValue(DescriptionProperty, value);
            }
        }

        public NoteView()
        {
            InitializeComponent();
        }

        private void UserControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }

           
            //ExplorerNet.MVVM.View.NoteWindow nw = new MVVM.View.NoteWindow();
            //nw.Title = Description;
            //nw.ShowDialog();
        }
    }
}
