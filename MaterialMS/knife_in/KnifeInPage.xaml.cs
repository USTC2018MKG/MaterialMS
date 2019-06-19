using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MaterialMS.knife_in
{
    /// <summary>
    /// KnifeInPage.xaml 的交互逻辑
    /// </summary>
    public partial class KnifeInPage : Page
    {
        private MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
        private Material material;
        private ObservableCollection<Material> materials = new ObservableCollection<Material>();
        private ObservableCollection<Material> os;   

        public KnifeInPage()
        {
            InitializeComponent();
            knifeAddInfo.ItemsSource = materials;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (txtMid.Text.Trim() == "")
            {
                tblSearchMsg.Content = "请输入刀具编号!";
                txtMid.Focus();
                return;
            } 
            //按照ID查询
            else
            {
                MySqlConnection connection = new MySqlConnection(Constant.myConnectionString);
                connection.Open();
         
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "select * from material where mid=@mid";
                    cmd.Parameters.Add(new MySqlParameter("@mid", MySqlDbType.VarChar, 50));
                    cmd.Parameters["@mid"].Value = txtMid.Text;
                    MySqlDataReader sdr = cmd.ExecuteReader();
                    Console.WriteLine(sdr);
                    if (!sdr.Read()) {
                        tblSearchMsg.Content = "刀具不存在！请重新输入";
                        txtMid.Text = "";
                        txtMid.Focus();
                    }
                    else{
                        os = new ObservableCollection<Material>();
                        Material material = new Material();
                        material.mid = sdr["mid"].ToString();
                        material.rest = sdr["rest"].ToString();
                        material.mname = sdr["mname"].ToString();
                        material.add_num = "";
                        material.buy_type = sdr["buy_type"].ToString();
                        material.cycle = sdr["cycle"].ToString();
                        material.each_price = sdr["each_price"].ToString();
                        material.repository_id = sdr["repository_id"].ToString();                            
                        Console.WriteLine(sdr["mname"].ToString());
                        os.Add(material);
                    }
                    
                    knife.ItemsSource = os;
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (isNum(os[0].add_num))
            {
                string addnum = os[0].add_num;
                string msg = "确定要入库刀具" + os[0].mid + ":" + os[0].add_num + "只吗?";
                MessageBoxResult dr = MessageBox.Show(msg, "刀具入库", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (dr == MessageBoxResult.OK)
                {
                    Material materialOld = findItem(os[0].mid);
                    if (null != materialOld)
                    {
                        materialOld.add_num = (int.Parse(os[0].add_num)+int.Parse(materialOld.add_num)).ToString();
                    }
                    else {
                        materials.Add(os[0]);
                    }
                    
                    MessageBox.Show("添加成功!");                  
                    Console.WriteLine(materials[0].add_num);
                    os.Clear();
                }
            }
            else {
                MessageBox.Show("入库数量有误");
            }
            
        }

        private Material findItem(String mid)
        {
            foreach (Material i in materials)
            {
                if (i.mid.Equals(mid))
                {
                    // item.num += int.Parse(txtNum.Text);
                    return i;
                }

            }
            return null;
        }

        private void InList_Click(object sender, RoutedEventArgs e)
        {
            //连接数据库对象
            MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
            conn.Open();
            //打开通道，建立连接，可能出现异常,使用try catch语句   
            MySqlTransaction transaction = conn.BeginTransaction();
            string in_id = System.Guid.NewGuid().ToString("N");
            string sql = string.Format("insert into in_order (in_id,in_time,employee_id) values('{0}','{1}','{2}')",
                in_id, DateTime.Now.ToLocalTime(), Account.Instance.GetUser().employee_id);

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn, transaction);
                int result = cmd.ExecuteNonQuery();
                if (result != 0 && materials.Count > 0)
                {
                    int count = 0;
                    foreach (Material item in materials)
                    {
                        int restNew = int.Parse(item.rest) + int.Parse(item.add_num);
                        string msql = string.Format("update material set rest='{0}' where mid='{1}'", restNew, item.mid);
                        MySqlCommand mycmd1 = new MySqlCommand(msql, conn, transaction);
                        mycmd1.ExecuteNonQuery();
                        string insql = string.Format("insert into in_item (in_id,mid,num) values('{0}','{1}','{2}')",
                            in_id,item.mid,int.Parse(item.add_num));
                        MySqlCommand mycmd2 = new MySqlCommand(insql, conn, transaction);
                        mycmd2.ExecuteNonQuery();
                        count++;
                    }
                    if (count == materials.Count)
                    {
                        MessageBox.Show("入库成功");
                        transaction.Commit();
                        
                    }
                }
                else
                {
                    MessageBox.Show("插入失败!");
                    transaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("插入失败!");
                transaction.Rollback();
            }
            finally {
                conn.Close();
                materials.Clear(); 
            }
            
        }

        private void ClearList_Click(object sender, RoutedEventArgs e)
        {
            materials = new ObservableCollection<Material>();
            knifeAddInfo.ItemsSource = materials;
            
        }

        private void knifesItemClick(object sender, SelectionChangedEventArgs e)
        {
            //Console.Write(sender);
        }

        private Boolean isNum(String add_num) {
            int a = 0;
            if (add_num != null && !add_num.Equals("") && int.TryParse(add_num, out a))           
                return true;          
            else
                return false;
        }

    }
}
