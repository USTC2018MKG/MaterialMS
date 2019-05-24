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
        private List<Material> materials;
        private ObservableCollection<Material> os;

        public KnifeInPage()
        {
            InitializeComponent();
            materials = new List<Material>();
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
                    if (sdr.Read()) {
                        os = new ObservableCollection<Material>() {
                          new Material(){
                              mid=sdr["mid"].ToString(),
                              rest=sdr["rest"].ToString(),
                              mname=sdr["mname"].ToString(),
                              add_num="",
                              buy_type=sdr["buy_type"].ToString(),
                              cycle = sdr["cycle"].ToString(),
                              each_price = sdr["each_price"].ToString(),
                              repository_id = sdr["repository_id"].ToString()
                          }

                        };
                        Console.WriteLine(sdr["mname"].ToString());
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
            Button btn = sender as Button;
            knife.SelectedItem = btn.DataContext;
            DataRowView rowSelected = knife.SelectedItem as DataRowView;
            
            
            material = new Material();
            if (rowSelected != null)
            {  
                material.rest = rowSelected["rest"].ToString();
                material.mid = rowSelected["mid"].ToString();               
                String tbText = btn.Tag as String;
                material.add_num = tbText;
            }
            string addnum = material.add_num;
            string msg = "确定要入库刀具" + material.mid + ":" + os[0].add_num + "只吗?";
            MessageBoxResult dr = MessageBox.Show(msg, "刀具入库", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr == MessageBoxResult.OK)
            {
                material.add_num = addnum;
                materials.Add(material);
                MessageBox.Show("添加成功!");
            }
            foreach(Material m in materials)
            {
                Console.WriteLine(m.mid);
            }
        }

        private void InList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void knifesItemClick(object sender, SelectionChangedEventArgs e)
        {
            //Console.Write(sender);
        }
        

    }
}
