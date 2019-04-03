using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// UserManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagePage : Page
    {        
        private User user;
        public UserManagePage()
        {
            InitializeComponent();
            getUserTable();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Trim() == "" && txtId.Text.Trim() == "")
            {
                labSearchMsg.Content = "请输入员工姓名或编号!";
                txtName.Focus();
                return;
            }
            //按照姓名查询
            else if (txtId.Text.Trim() == "")
            {
                labSearchMsg.Content = "";
                //连接数据库对象
                MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
                string sql = string.Format("select * from user where user_name = '{0}'", txtName.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    Console.WriteLine("已经建立连接");
                    //对数据库进行查询
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    dg1.ItemsSource = ds.Tables[0].AsDataView();
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
                string sql = string.Format("select * from user where emplyee_id = '{0}'", txtId.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    dg1.ItemsSource = ds.Tables[0].AsDataView();
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

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            UserRegistWindow urw = new UserRegistWindow();
            urw.Show();
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            UserModifyWindow umw = new UserModifyWindow(user);
            umw.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void getUserTable() {
            MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
            string sql = string.Format("select * from user");
            try
            {
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                dg1.ItemsSource = ds.Tables[0].AsDataView();
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

        private void Dg1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            DataRowView rowSelected = dg1.SelectedItem as DataRowView;
            if (rowSelected != null) {
                user = new User();                
                user.emplyee_id = rowSelected["emplyee_id"].ToString(); ;
                user.name = rowSelected["user_name"].ToString();
                user.phone = rowSelected["phone"].ToString();
                user.sex = rowSelected["sex"].ToString();
                user.age = rowSelected["age"].ToString();

            }
            
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

