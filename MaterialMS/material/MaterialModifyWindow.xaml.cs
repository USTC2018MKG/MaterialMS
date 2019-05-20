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

namespace MaterialMS.material
{
    /// <summary>
    /// MaterialModifyWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialModifyWindow : Window
    {
        private Material material;
        private MaterialManagePage mp;

        public MaterialModifyWindow(Material material, MaterialManagePage mp)
        {
            InitializeComponent();
            this.material = material;
            this.mp = mp;
            InitWindow();
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (txtId.Text.Trim() == "")
            {
                labIdMsg.Content = "请输入刀具编号!";
                txtId.Focus();
                return;
            }
            else if (txtMname.Text.Trim() == "")
            {
                labMnameMsg.Content = "请输入刀具名称!";
                txtMname.Focus();
                return;
            }
            else if (txtCycle.Text.Trim() == "")
            {
                labCycleMsg.Content = "请输入交货周期!";
                txtCycle.Focus();
                return;
            }
            else if (txtBuytype.Text.Trim() == "")
            {
                labBuytypeMsg.Content = "请输入购买型号!";
                txtBuytype.Focus();
                return;
            }
            else if (txtBuybus.Text.Trim() == "")
            {
                labBuybusMsg.Content = "请输入购物车代码!";
                txtBuybus.Focus();
                return;
            }
            else if (txtFirstrepo.Text.Trim() == "")
            {
                labFirstrepoMsg.Content = "请输入首次库存!";
                txtFirstrepo.Focus();
                return;
            }
            else if (txtRepository.Text.Trim() == "")
            {
                labRepositoryMsg.Content = "请输入刀具库位!";
                txtRepository.Focus();
                return;
            }
            else if (txtNtax.Text.Trim() == "")
            {
                labNtaxMsg.Content = "请输入未税单价!";
                txtNtax.Focus();
                return;
            }
            else if (txtKnife.Text.Trim() == "")
            {
                labKnifeMsg.Content = "请输入刀片数量!";
                txtKnife.Focus();
                return;
            }
            else if (txtRotate.Text.Trim() == "")
            {
                labRotateMsg.Content = "请输入旋转刀面!";
                txtRotate.Focus();
                return;
            }
            else if (txtAge.Text.Trim() == "")
            {
                labAgeMsg.Content = "请输入预计寿命!";
                txtAge.Focus();
                return;
            }
            else if (txtExchange.Text.Trim() == "")
            {
                labExchangeMsg.Content = "请输入单片更换!";
                txtExchange.Focus();
                return;
            }
            else if (txtGetmax.Text.Trim() == "")
            {
                labGetmaxMsg.Content = "请输入领用上限!";
                txtGetmax.Focus();
                return;
            }
            else if (txtCost.Text.Trim() == "")
            {
                labCostMsg.Content = "请输入单件成本!";
                txtCost.Focus();
                return;
            }
            else if (txtRest.Text.Trim() == "")
            {
                labRestMsg.Content = "请输入剩余数量!";
                txtRest.Focus();
                return;
            }
            else
            {
                //连接数据库对象
                MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
                string sql = string.Format("update material set mname='{1}',cycle='{2}',buy_type='{3}',shopping_car='{4}',first_repo='{5}',repository_id='{6}',ntax_price='{7}',knife_num='{8}',rotate_num='{9}',pred_age='{10}',exchange='{11}',get_max='{12}',each_price='{13}',rest='{14}' where mid='{0}'", txtId.Text.Trim(), txtMname.Text.Trim(), txtCycle.Text.Trim(), txtBuytype.Text.Trim(), txtBuybus.Text.Trim(), txtFirstrepo.Text.Trim(), txtRepository.Text.Trim(), txtNtax.Text.Trim(), txtKnife.Text.Trim(), txtRotate.Text.Trim(), txtAge.Text.Trim(), txtExchange.Text.Trim(), txtGetmax.Text.Trim(), txtCost.Text.Trim(), txtRest.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    //对数据库进行插入
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    int result = cmd.ExecuteNonQuery();
                    if (result != 0)
                    {
                        MessageBox.Show("修改成功!");
                        mp.getMaterialTable(1);
                    }
                    else
                    {
                        MessageBox.Show("修改失败!");
                        txtId.Text = "";
                        txtMname.Text = "";
                        txtCycle.Text = "";
                        txtBuytype.Text = "";
                        txtBuybus.Text = "";
                        txtFirstrepo.Text = "";
                        txtNtax.Text = "";
                        txtKnife.Text = "";
                        txtRotate.Text = "";
                        txtAge.Text = "";
                        txtExchange.Text = "";
                        txtGetmax.Text = "";
                        txtCost.Text = "";
                        txtRepository.Text = "";
                        txtRest.Text = "";
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("修改失败!");
                    txtId.Text = "";
                    txtMname.Text = "";
                    txtCycle.Text = "";
                    txtBuytype.Text = "";
                    txtBuybus.Text = "";
                    txtFirstrepo.Text = "";
                    txtNtax.Text = "";
                    txtKnife.Text = "";
                    txtRotate.Text = "";
                    txtAge.Text = "";
                    txtExchange.Text = "";
                    txtGetmax.Text = "";
                    txtCost.Text = "";
                    txtRepository.Text = "";
                    txtRest.Text = "";
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void InitWindow()
        {
            txtId.Text = material.mid;
            txtMname.Text = material.mname;
            txtCycle.Text = material.cycle;
            txtBuytype.Text = material.buy_type;
            txtBuybus.Text = material.shopping_car;
            txtFirstrepo.Text = material.first_repo;
            txtRepository.Text = material.repository_id;
            txtNtax.Text = material.ntax_price;
            txtKnife.Text = material.knife_num;
            txtRotate.Text = material.rotate_num;
            txtAge.Text = material.pred_age;
            txtExchange.Text = material.exchange;
            txtGetmax.Text = material.get_max;
            txtCost.Text = material.each_price;
            txtRest.Text = material.rest;        
        }
    }
}
