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

namespace ExplorerNet
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            _LoadSettings();
        }

        private void _LoadSettings()
        {
            txtHeightLevel.Text = Properties.Settings.Default.HeightLevel.ToString();
            txtWidthFilepanel.Text = Properties.Settings.Default.WidthFilepanel.ToString();
        }

        private void _SaveSettings()
        {
            Properties.Settings.Default.HeightLevel = double.Parse(txtHeightLevel.Text);
            Properties.Settings.Default.WidthFilepanel = double.Parse(txtWidthFilepanel.Text);

            Properties.Settings.Default.Save();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDefault_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reset();

            _LoadSettings();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            _SaveSettings();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _SaveSettings();
            this.Close();
        }
    }
}
