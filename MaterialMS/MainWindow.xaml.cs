using MaterialMS.input;
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
        DataAnalysisPage dataAnalysisPage;
        InFormPage inFormPage;
        OutFormPage outFormPage;
        public MainWindow()
        {
            InitializeComponent();
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

        private void dataAnalysisClick(object sender, RoutedEventArgs e)
        {
            dataAnalysisPage = new DataAnalysisPage();
            ContentControl.Content = new Frame()
            {
                Content = dataAnalysisPage
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
