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
    /// UserModifyWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserModifyWindow : Window
    {
        private User user;
        public UserModifyWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            InitWindow();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (txtAge.Text.Trim() == "")
            {
                labAgeMsg.Content = "请输入员工年龄!";
                txtAge.Focus();
                return;
            }
            else if (txtPhone.Text.Trim() == "")
            {
                labPhoneMsg.Content = "请输入电话号码!";
                txtPhone.Focus();
                return;
            }
            else
            {
                //连接数据库对象
                MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
                int sex;
                if (rdbMan.IsChecked == true)
                {
                    sex = 1;
                }
                else
                {
                    sex = 0;
                }
                string sql = string.Format("update user set user_name='{0}',sex='{1}',phone='{2}',age='{3}' where emplyee_id='{4}'", txtName.Text.Trim(), sex, txtPhone.Text.Trim(), txtAge.Text.Trim(), user.emplyee_id);
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    //对数据库进行插入
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    int result = cmd.ExecuteNonQuery();
                    if (result != 0)
                    {
                        MessageBox.Show("插入成功!");
                    }
                    else
                    {
                        MessageBox.Show("插入失败!");

                        txtName.Text = "";
                        txtAge.Text = "";
                        txtPhone.Text = "";
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("插入失败!");

                    txtName.Text = "";
                    txtAge.Text = "";
                    txtPhone.Text = "";
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void InitWindow()
        {

            txtName.Text = user.name;
            txtAge.Text = user.age;
            txtPhone.Text = user.phone;
            if (user.sex.Equals("1"))
            {
                rdbMan.IsChecked = true;
            }
            else
            {
                rdbWoman.IsChecked = true;
            }
        }
    }
}
