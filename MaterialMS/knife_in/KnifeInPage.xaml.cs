using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
                string sql = string.Format("select * from material where mid = '{0}' order by mname", txtMid.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    knife.ItemsSource = ds.Tables[0].AsDataView();
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
            string msg = "确定要入库刀具" + material.mid + ":" + addnum + "只吗?";
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
