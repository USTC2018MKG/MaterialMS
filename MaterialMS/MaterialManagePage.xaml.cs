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
        private Material material;

        public MaterialManagePage()
        {
            InitializeComponent();
            getMaterialTable();
        }

        private void Search_Click(object sender, RoutedEventArgs e) {
            if (txtMname.Text.Trim() == "" && txtMid.Text.Trim() == "")
            {
                labSearchMsg.Content = "请输入零件名或零件编号!";
                txtMname.Focus();
                return;
            }
            //按照零件名查询
            else if (txtMid.Text.Trim() == "")
            {
                labSearchMsg.Content = "";
                //连接数据库对象
                MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
                string sql = string.Format("select * from material where mname = '{0}'", txtMname.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接
                    //对数据库进行查询
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    dg2.ItemsSource = ds.Tables[0].AsDataView();
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
                //连接数据库对象
                MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
                string sql = string.Format("select * from material where mid = '{0}'", txtMid.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    dg2.ItemsSource = ds.Tables[0].AsDataView();
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

        }

        public void getMaterialTable(){
            MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
            string sql = string.Format("select * from material");
            try
            {
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                dg2.ItemsSource = ds.Tables[0].AsDataView();
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
            DataRowView rowSelected = dg2.SelectedItem as DataRowView;
            if (rowSelected != null) {
                material = new Material();               
                material.mid = rowSelected["mid"].ToString();
                material.mname = rowSelected["mname"].ToString();
                material.repository_id = rowSelected["repository_id"].ToString();
                material.rest = rowSelected["rest"].ToString();
                material.price = rowSelected["price"].ToString();
            }
        }


    }
}
