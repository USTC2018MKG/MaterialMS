using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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


namespace MaterialMS.input
{
    /// <summary>
    /// OrderFormPage.xaml 的交互逻辑
    /// </summary>
    public partial class InFormPage : Page
    {
        private In input;
        InDetailWindow inDetailWindow;
        public InFormPage()
        {
            InitializeComponent();
            getInOrderTable();
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            if (tbForSearch.Text.Trim() == "")
            {
                tblSearchMsg.Text = "请输入零件名或零件编号!";
                tbForSearch.Focus();
                return;
            }//按照零件名查询
            else
            {
                tblSearchMsg.Text = "";
                //连接数据库对象
                MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
                string sql = string.Format("select * from in_order where in_id = '{0}'", tbForSearch.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接
                    //对数据库进行查询
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    lvOrders.ItemsSource = ds.Tables[0].AsDataView();
                }
                catch (MySqlException ex)
                {
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            lvOrders.SelectedItem = ((Button)sender).DataContext;
            DataRowView rowSelected = lvOrders.SelectedItem as DataRowView;
            if (rowSelected != null)
            {
                input = new In();
                input.in_id = rowSelected["in_id"].ToString();
                inDetailWindow = new InDetailWindow(input);
                inDetailWindow.Show();
            }
        }

        public void getInOrderTable()
        {
            MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
            string sql = string.Format("select * from in_order");
            try
            {
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                lvOrders.ItemsSource = ds.Tables[0].AsDataView();
            }
            catch (MySqlException ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
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
