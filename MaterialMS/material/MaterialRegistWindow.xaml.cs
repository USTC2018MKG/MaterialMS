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
    /// MaterialRegistWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialRegistWindow : Window
    {
        private MaterialManagePage mp;
        public MaterialRegistWindow(MaterialManagePage mp)
        {
            this.mp = mp;
            InitializeComponent();
        }

        private void Button_Regist(object sender, RoutedEventArgs e)
        {
            if (txtMname.Text.Trim() == "")
            {
                labMnameMsg.Content = "请输入零件名称!";
                txtMname.Focus();
                return;
            }
            else if (txtRepository.Text.Trim() == "")
            {
                labRepositoryMsg.Content = "请输入仓库编号!";
                txtRepository.Focus();
                return;
            }
            else if (txtRest.Text.Trim() == "")
            {
                labRestMsg.Content = "请输入剩余数量!";
                txtRest.Focus();
                return;
            }
            else if (txtCategory.Text.Trim() == "")
            {
                labCategoryMsg.Content = "请输入所属分类!";
                txtCategory.Focus();
                return;
            }
            else
            {
                //连接数据库对象
                MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
                string uuid = System.Guid.NewGuid().ToString("N");
  
                string sql = string.Format("insert into material (mid,mname,repository_id,rest,category_id,price) values('{0}','{1}','{2}','{3}','{4}','{5}')", uuid, txtMname.Text.Trim(), txtRepository.Text.Trim(), txtRest.Text.Trim(), txtCategory.Text.Trim(),txtPrice.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    //对数据库进行插入
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    int result = cmd.ExecuteNonQuery();
                    if (result != 0)
                    {
                        MessageBox.Show("插入成功!");
                        mp.getMaterialTable();
                    }
                    else
                    {
                        MessageBox.Show("插入失败!");                      
                        txtMname.Text = "";
                        txtRepository.Text = "";
                        txtRest.Text = "";
                        txtCategory.Text = "";
                        txtPrice.Text = "";                       
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("插入失败!");
                    txtMname.Text = "";
                    txtRepository.Text = "";
                    txtRest.Text = "";
                    txtCategory.Text = "";
                    txtPrice.Text = "";
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}

