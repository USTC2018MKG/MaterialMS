using MaterialMS;
using MaterialMS.product;
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
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, RoutedEventArgs e) { }

        private void Create_Click(object sender, RoutedEventArgs e) {
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.Show();
        }

        private void Detail_Click(object sender, RoutedEventArgs e) { }

        private void Dg2_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
        }

        private void ordersItemClick(object sender, SelectionChangedEventArgs e)
        {

        }
        //上一页
        private void LastPage_Click(object sender, RoutedEventArgs e)
        {

        }

        //下一页
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {

        }

        //跳转任意页
        private void Go_Click(object sender, RoutedEventArgs e)
        {

        }

        //sql分页工具类
        public void Sqlutils(string sql_count)
        {

        }
    }
}
