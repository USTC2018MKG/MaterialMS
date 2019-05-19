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
        private UserManagePage up;
        private Boolean admin;

        public UserModifyWindow(User user,UserManagePage up)
        {
            InitializeComponent();
            this.user = user;
            this.up = up;
            InitWindow();
            string type = Account.Instance.GetUser().type;
            if (type.Equals("10")) {
                admin = true;
            }
            else
            {
                admin = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtPhone.Text.Trim() == "")
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
                int type = 0;
                if (admin == false) {
                    MessageBox.Show("无修改员工权限!");
                }

                if (rdbMan.IsChecked == true)
                {
                    sex = 1;
                }else
                {
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

                string sql = string.Format("update user set user_name='{0}',sex='{1}',phone='{2}',type='{3}' where employee_id='{4}'", txtName.Text.Trim(), sex, txtPhone.Text.Trim(), type, user.employee_id);
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

                        txtName.Text = "";
                        txtPhone.Text = "";
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("插入失败!");
                    txtName.Text = "";                    
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
            txtPhone.Text = user.phone;
            if (user.sex.Equals("1"))
            {
                rdbMan.IsChecked = true;
            }else
            {
                rdbWoman.IsChecked = true;
            }

            if(user.type.Equals("0"))
            {
                rdbStuff.IsChecked = true;
            }else if (user.type.Equals("1"))
            {
                rdbEngineer.IsChecked = true;
            }else if (user.type.Equals("2"))
            {
                rdbExecutive.IsChecked = true;
            }else if (user.type.Equals("3"))
            {
                rdbManager.IsChecked = true;
            }
        }
    }
}
