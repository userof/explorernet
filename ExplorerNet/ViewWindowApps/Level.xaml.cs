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
    public partial class Level : UserControl
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
            spMain.Children.Add(filePanel);
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
    }
}
