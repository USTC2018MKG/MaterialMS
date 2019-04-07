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
                string sql = string.Format("select * from user where user_name = '{0}' order by user_name", txtName.Text.Trim());
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
                string sql = string.Format("select * from user where emplyee_id = '{0}' order by user_name", txtId.Text.Trim());
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
            UserRegistWindow urw = new UserRegistWindow(this);
            urw.Show();
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {           
            UserModifyWindow umw = new UserModifyWindow(user,this);
            umw.Show();
            getUserTable();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (user != null)
            {
                string name = user.name;
                string msg = "确定要删除用户" + name + "吗？";

                MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
                string sql = string.Format("update user set state=1 where emplyee_id='{0}'", user.emplyee_id);
                
                //MessageBox.Show(msg, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                MessageBoxResult dr = MessageBox.Show(msg, "删除用户", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (dr == MessageBoxResult.OK)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        int result = cmd.ExecuteNonQuery();
                        if (result != 0)
                        {
                            MessageBox.Show("删除成功!");
                        }
                        else
                        {
                            MessageBox.Show("删除失败!");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("插入失败!");
                    }
                    finally
                    {
                        conn.Close();
                        getUserTable();
                    }
                }
            }
            else
            {
                MessageBox.Show("请点击要删除的用户行!");
            }
        }

        public void getUserTable() {
            MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
            string sql = string.Format("select * from user order by user_name");
            
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
                user.emplyee_id = rowSelected["emplyee_id"].ToString();
                user.name = rowSelected["user_name"].ToString();
                user.phone = rowSelected["phone"].ToString();
                user.sex = rowSelected["sex"].ToString();
                user.age = rowSelected["age"].ToString();
            }
            
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

