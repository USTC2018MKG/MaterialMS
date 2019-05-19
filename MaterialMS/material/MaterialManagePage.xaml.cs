using MaterialMS.material;
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
    /// MaterialManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialManagePage : Page
    {
        private MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
        private Material material;

        public MaterialManagePage()
        {
            InitializeComponent();
            getMaterialTable();
        }

        private void Search_Click(object sender, RoutedEventArgs e) {
            if (txtMname.Text.Trim() == "" && txtMid.Text.Trim() == "")
            {
                tblSearchMsg.Text = "请输入零件名或零件编号!";
                txtMname.Focus();
                return;
            }
            //按照零件名查询
            else if (txtMid.Text.Trim() == "")
            {
                tblSearchMsg.Text = "";
                string sql = string.Format("select * from material where mname = '{0}' order by mname", txtMname.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接
                    //对数据库进行查询
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    lv.ItemsSource = ds.Tables[0].AsDataView();
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
            //按照ID查询
            else
            {
                string sql = string.Format("select * from material where mid = '{0}' order by mname", txtMid.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    lv.ItemsSource = ds.Tables[0].AsDataView();
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

        private void Create_Click(object sender, RoutedEventArgs e) {
            MaterialRegistWindow rgw = new MaterialRegistWindow(this);
            rgw.Show();
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            MaterialModifyWindow mmw = new MaterialModifyWindow(material,this);
            mmw.Show();
            getMaterialTable();
        }

        public void getMaterialTable(){
            string sql = string.Format("select * from material order by mname");
            try
            {
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                lv.ItemsSource = ds.Tables[0].AsDataView();
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

        private void Dg2_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            DataRowView rowSelected = lv.SelectedItem as DataRowView;
            if (rowSelected != null) {
                material = new Material();               
                material.mid = rowSelected["mid"].ToString();
                material.mname = rowSelected["mname"].ToString();
                material.repository_id = rowSelected["repository_id"].ToString();
                material.rest = rowSelected["rest"].ToString();
                material.cycle = rowSelected["cycle"].ToString();
                material.each_price = rowSelected["each_price"].ToString();
                material.buy_type = rowSelected["buy_type"].ToString();
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
