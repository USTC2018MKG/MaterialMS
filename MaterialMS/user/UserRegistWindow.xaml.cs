using MySql.Data.MySqlClient;
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
using System.Windows.Shapes;

namespace MaterialMS
{
    /// <summary>
    /// UserRegistWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserRegistWindow : Window
    {
        private UserManagePage up;
        public UserRegistWindow(UserManagePage up)
        {
            this.up = up;
            InitializeComponent();
        }

        private void Button_Regist(object sender, RoutedEventArgs e)
        {
            if (txtId.Text.Trim() == "")
            {
                labIdMsg.Content = "请输入员工编号!";
                txtId.Focus();
                return;
            }else if (txtName.Text.Trim() == "")
            {
                labNameMsg.Content = "请输入员工姓名!";
                txtName.Focus();
                return;
            }else if (txtPhone.Text.Trim() == "")
            {
                labPhoneMsg.Content = "请输入电话号码!";
                txtPhone.Focus();
                return;
            }
            else
            {
                //连接数据库对象
                MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
                string uuid = System.Guid.NewGuid().ToString("N");
                int sex;
                int type = 0;
                if (txtSex.IsChecked == true)
                {
                    sex = 1;
                }else {
                    sex = 0;
                }

                if (rdbStuff.IsChecked == true)
                {
                    type = 0;
                }
                else if (rdbEngineer.IsChecked == true)
                {
                    type = 1;
                }
                else if (rdbExecutive.IsChecked == true)
                {
                    type = 2;
                }
                else if (rdbManager.IsChecked == true)
                {
                    type = 3;
                }

                string sql = string.Format("insert into user (employee_id,user_name,user_pwd,sex,phone,state,type) values('{0}','{1}',123,'{2}','{3}',0,'{4}')", txtId.Text.Trim(),txtName.Text.Trim(),sex,txtPhone.Text.Trim(),type);
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    //对数据库进行插入
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    int result = cmd.ExecuteNonQuery();
                    if (result != 0)
                    {
                        MessageBox.Show("插入成功!");
                        up.getUserTable(1);
                    }
                    else
                    {
                        MessageBox.Show("插入失败!");
                        txtId.Text = "";
                        txtName.Text = "";                       
                        txtPhone.Text = "";
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("插入失败!");
                    txtId.Text = "";
                    txtName.Text = "";                    
                    txtPhone.Text = "";
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
