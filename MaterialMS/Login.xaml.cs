using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
using System.Windows.Shapes;

namespace MaterialMS
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (txtUserName.Text.Trim() == "")
            {
                labNameMsg.Content = "用户名不能为空";
                txtUserName.Focus();
                return;
            }
            else if (txtPwd.Password.Trim() == "")
            {
                labPwdMsg.Content = "密码不能为空!";
                txtPwd.Focus();
                return;
            }

            ConnectionDB();
        }
    
        public void ConnectionDB()
        {

            string name = txtUserName.Text;
            MySqlConnection connection = new MySqlConnection(Constant.myConnectionString);
            connection.Open();
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from user where employee_id=@username";
                cmd.Parameters.Add(new MySqlParameter("@username", MySqlDbType.VarChar, 50));
                cmd.Parameters["@username"].Value = txtUserName.Text;
                MySqlDataReader sdr = cmd.ExecuteReader();
                if (!sdr.Read())
                {
                    labNameMsg.Content = "用户名不存在！请重新输入";
                    txtUserName.Text = "";
                    txtPwd.Password = "";
                    txtUserName.Focus();
                }
                
                else if (sdr["user_pwd"].ToString().Trim() == txtPwd.Password.Trim())
                {
                    String in_type = sdr["type"].ToString().Trim();
                    if (in_type.Equals("0") || in_type.Equals("1") || in_type.Equals("2") || in_type.Equals("3"))
                    {
                        labNameMsg.Content = "普通用户无访问权限！";
                        txtUserName.Text = "";
                        txtPwd.Password = "";
                        txtUserName.Focus();
                    }
                    else
                    {
                        User user = new User();
                        user.employee_id = sdr["employee_id"].ToString().Trim();
                        user.name = sdr["user_name"].ToString().Trim();
                        user.password = sdr["user_pwd"].ToString().Trim();
                        user.sex = sdr["sex"].ToString().Trim();
                        user.phone = sdr["phone"].ToString().Trim();
                        user.state = sdr["state"].ToString().Trim();
                        user.type = sdr["type"].ToString().Trim();

                        Account.Instance.Login(user);
                        MainWindow Mn = new MainWindow();
                        this.Close();
                        Mn.Show();
                    }
                }
                else
                {
                    labPwdMsg.Content = "密码错误!请重新输入！";
                    txtUserName.Text = "";
                    txtPwd.Password = "";
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();

                }
            }
        }
    }

}
