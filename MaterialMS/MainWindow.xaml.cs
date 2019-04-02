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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        UserManagePage userManagePage;
        MaterialManagePage materialManagePage;
        OrderDetailPage orderDetailPage;
        OrderFormPage orderFormPage;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void userPageClick(object sender, RoutedEventArgs e)
        {
            if (userManagePage == null)
                userManagePage = new UserManagePage();
            ContentControl.Content = new Frame()
            {
                Content = userManagePage
            };
        }

        private void materialClick(object sender, RoutedEventArgs e)
        {
            if (materialManagePage == null)
                materialManagePage = new MaterialManagePage();
            ContentControl.Content = new Frame()
            {
                Content = materialManagePage
            };
        }
        private void orderPageClick(object sender, RoutedEventArgs e)
        {
            if (orderFormPage == null)
                orderFormPage = new OrderFormPage();
            ContentControl.Content = new Frame()
            {
                Content = orderFormPage
            };
        }

        private void orderDetPageClick(object sender, RoutedEventArgs e)
        {
            if (orderDetailPage == null)
                orderDetailPage = new OrderDetailPage();
            ContentControl.Content = new Frame()
            {
                Content = orderDetailPage
            };
        }
    }
}
