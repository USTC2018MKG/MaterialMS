﻿using MaterialMS.input;
using MaterialMS.knife_in;
using MaterialMS.output;
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
        ProductPage productPage;
        InFormPage inFormPage;
        OutFormPage outFormPage;
        KnifeInPage knifeInPage;
        public MainWindow()
        {
            InitializeComponent();
            string name = Account.Instance.GetUser().name;
            string msg = "欢迎" + name + "来到本系统!";
            welLab.Content = msg;
        }

        private void userPageClick(object sender, RoutedEventArgs e)
        {           
            userManagePage = new UserManagePage();
            ContentControl.Content = new Frame()
            {
                Content = userManagePage
            };
        }

        private void materialClick(object sender, RoutedEventArgs e)
        {       
            materialManagePage = new MaterialManagePage();
            ContentControl.Content = new Frame()
            {
                Content = materialManagePage
            };
        }

        private void knifeInClick(object sender, RoutedEventArgs e)
        {
            knifeInPage = new KnifeInPage();
            ContentControl.Content = new Frame()
            {
                Content = knifeInPage
            };
        }

        private void inputPageClick(object sender, RoutedEventArgs e)
        {           
            inFormPage = new InFormPage();
            ContentControl.Content = new Frame()
            {
                Content = inFormPage
            };
        }

        private void outputPageClick(object sender, RoutedEventArgs e)
        {
            outFormPage = new OutFormPage();
            ContentControl.Content = new Frame()
            {
                Content = outFormPage
            };
        }

        private void productClick(object sender, RoutedEventArgs e)
        {
            productPage = new ProductPage();
            ContentControl.Content = new Frame()
            {
                Content = productPage
            };
        }

        private void exitClick(object sender, RoutedEventArgs e)
        {
            Account.Instance.Logout();
            LoginWindow lw = new LoginWindow();
            this.Close();           
            lw.Show();
        }
    }
}
