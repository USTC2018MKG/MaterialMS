﻿using System;
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
        UserManagePage userManagePage =new UserManagePage();
        MaterialManagePage materialManagePage = new MaterialManagePage();
        OrderDetailPage orderDetailPage;
        OrderFormPage orderFormPage;
        public MainWindow()
        {
            InitializeComponent();
            //UserManagePage userManagePage = new UserManagePage();
            //gridPage.Navigate(userManagePage);
        }

        private void userPageClick(object sender, RoutedEventArgs e)
        {

            ContentControl.Content = new Frame()
            {
                Content = userManagePage
                //gridPage.Navigate(userManagePage);

            };
        }

        private void materialClick(object sender, RoutedEventArgs e)
        {
     
            ContentControl.Content = new Frame()
            {
                Content = materialManagePage
                //gridPage.Navigate(userManagePage);

            };
        }
            private void orderPageClick(object sender, RoutedEventArgs e)
        {
            //gridPage.Children.Add(new OrderFormPage());
        }

        private void orderDetPageClick(object sender, RoutedEventArgs e)
        {
           // gridPage.Children.Add(new OrderDetailPage());
        }
    }
}
