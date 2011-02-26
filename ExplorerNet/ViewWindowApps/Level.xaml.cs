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

namespace ExplorerNet.ViewWindowApps
{
    /// <summary>
    /// Уровень. Элемент управления, содержащий файловые панели.
    /// </summary>
    public partial class Level : RadioButton
    {
        public Level()
        {
            InitializeComponent();
            //Загружаем сохранённую высоту
            this.Height = Properties.Settings.Default.HeightLevel;
        }

        /// <summary>
        /// Добавляем новую файловую панель
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewFilePanel_Click(object sender, RoutedEventArgs e)
        {
            FilePanel filePanel = new FilePanel();
            filePanel.Height = spMain.Height;
            spMain.Children.Add(filePanel);

            Binding binding = new Binding();
            binding.Source = spMain;
            binding.Path = new PropertyPath("Height");
            binding.Mode = BindingMode.OneWay;
            filePanel.SetBinding(FilePanel.HeightProperty, binding);

            //Binding binding = new Binding();
            //binding.Source = filePanel;
            //binding.Path = new PropertyPath("Height");
            //binding.Mode = BindingMode.OneWay;
            //this.SetBinding(UserControl.HeightProperty, binding);
        }

        /// <summary>
        /// Уничтожаем данный уровень
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteLevel_Click(object sender, RoutedEventArgs e)
        {
            Panel panel = (Panel)this.Parent;
            panel.Children.Remove(this);
        }

        private void tmbMove_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            this.Height += e.VerticalChange;
        }

        private void CloneLevel()
        {
            Panel panel = (Panel)this.Parent;

            Level level = new Level();
            panel.Children.Add(level);

            foreach (var child in this.spMain.Children)
            {
                FilePanel fp = (FilePanel)child;

                FilePanel newFp = new FilePanel();
                newFp.FilePanelSettings = fp.FilePanelSettings;

                //newFp.Width = fp.Width;
                //newFp.Path = fp.Path;

                level.spMain.Children.Add(newFp);
            }

            level.Height = this.Height;
        }

        private void btnClobeLevel_Click(object sender, RoutedEventArgs e)
        {
            CloneLevel();
        }

        private void level_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.IsChecked = true;
            //MessageBox.Show(this.IsChecked.ToString());
        }

        private void btnNewPreviewPanel_Click(object sender, RoutedEventArgs e)
        {
            
            //this.spMain.Children.Add(pp);

            ExplorerNet.MVVM.View.PreviewPanel pp = new MVVM.View.PreviewPanel();
            pp.Height = spMain.Height;
            spMain.Children.Add(pp);

            Binding binding = new Binding();
            binding.Source = spMain;
            binding.Path = new PropertyPath("Height");
            binding.Mode = BindingMode.OneWay;
            pp.SetBinding(ExplorerNet.MVVM.View.PreviewPanel.HeightProperty, binding);
        }
    }
}
