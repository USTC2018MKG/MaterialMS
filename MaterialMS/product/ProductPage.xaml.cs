using MaterialMS.dataanalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialMS
{
    /// <summary>
    /// DataAnalysisPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataAnalysisPage : Page
    {
        public DataAnalysisPage()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, RoutedEventArgs e) { }

        private void Create_Click(object sender, RoutedEventArgs e) {
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.Show();
        }

        private void Modify_Click(object sender, RoutedEventArgs e) { }

        private void Dg2_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
        }
    }
}
