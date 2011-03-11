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

using ExplorerNet.MVVM.ViewModel;

namespace ExplorerNet.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для NoteWindow.xaml
    /// </summary>
    public partial class NoteWindow : Window
    {
        public static readonly DependencyProperty DialogAnswerProperty =
            DependencyProperty.Register("DialogAnswer", typeof(bool?),
            typeof(NoteWindow));

        public bool? DialogAnswer
        {
            get
            {
                return (bool?)GetValue(DialogAnswerProperty);
            }
            set
            {
                //if (value != null)
                //{
                //    Close();
                //}
                SetValue(DialogAnswerProperty, value);
            }
        }

        public NoteWindow()
        {
            InitializeComponent();

            NoteWindowViewModel nVM = (NoteWindowViewModel)this.Resources["viewModel"];
            nVM.RequestClose += () => this.Close();

            Binding b = new Binding("DialogAnswer");
            //b.ElementName = nVM;
            b.Source = nVM;
            //b.Path = new PropertyPath(nVM.DialogAnswer);
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.SetBinding(NoteWindow.DialogAnswerProperty, b);
            //this.b .InputBindings.Add(b);
            ExplorerNet.Tools.ViewSettings.ViewLocation.LoadWindowLocation(this);
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ExplorerNet.Tools.ViewSettings.ViewLocation.SaveWindowLocation(this);
        }

    }
}
