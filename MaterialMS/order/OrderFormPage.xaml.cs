using MaterialMS.order;
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

namespace MaterialMS
{
    /// <summary>
    /// OrderFormPage.xaml 的交互逻辑
    /// </summary>
    public partial class OrderFormPage : Page
    {
        private Order order;
        OrderDetailWindow orderDetailWindow;
        public OrderFormPage()
        {
            InitializeComponent();
            getInOrderTable();
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            if (tbForSearch.Text.Trim() == "") {
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
            DataRowView rowSelected = lvOrders.SelectedItem as DataRowView;
            lvOrders.SelectedItem = ((Button)sender).DataContext;
            if (rowSelected != null)
            {
                order = new Order();
                order.in_id = rowSelected["in_id"].ToString();
                orderDetailWindow = new OrderDetailWindow(order);
                orderDetailWindow.Show();
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
    }
}
