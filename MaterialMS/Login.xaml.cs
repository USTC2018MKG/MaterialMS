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

        private String myConnectionString = "Server=localhost;Database=mms;Uid=root;Pwd=root;";
        public void ConnectionDB()
        {

            string name = txtUserName.Text;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            connection.Open();
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select emplyee_id,user_pwd from user where emplyee_id=@username";
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
                    MainWindow Mn = new MainWindow();
                    Mn.Show();
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
