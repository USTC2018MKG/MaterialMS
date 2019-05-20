using MaterialMS.material;
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

namespace MaterialMS
{
    /// <summary>
    /// MaterialManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialManagePage : Page
    {
        private MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
        private Material material;
        private int totalCount = 0;          //查询总数
        private int limit = 12;          //设置每页显示记录数
        private int totalPage;       //最大的页码数
        private int search_type;     //全部查询为0，按刀具名查询为1

        public MaterialManagePage()
        {
            InitializeComponent();
            getMaterialTable(1);
        }

        private void Search_Click(object sender, RoutedEventArgs e) {
            if (txtMname.Text.Trim() == "" && txtMid.Text.Trim() == "")
            {
                tblSearchMsg.Text = "请输入零件名或零件编号!";
                txtMname.Focus();
                return;
            }
            //按照零件名查询
            else if (txtMid.Text.Trim() == "")
            {
                tblSearchMsg.Text = "";
                searchByName(1);
            }
            //按照ID查询
            else
            {
                string sql = string.Format("select * from material where mid = '{0}' order by mname", txtMid.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    total_num.Content = 1;
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    lv.ItemsSource = ds.Tables[0].AsDataView();
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

        private void Create_Click(object sender, RoutedEventArgs e) {
            MaterialRegistWindow rgw = new MaterialRegistWindow(this);
            rgw.Show();
        }

        //批量导入
        private void BatchCreate_Click(object sender, RoutedEventArgs e)
        {
            string url = "";
            // 在WPF中， OpenFileDialog位于Microsoft.Win32名称空间
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "文本文件|*.txt";
            if (dialog.ShowDialog() == true)
            {
                url = dialog.FileName.Replace("\\", "/");
                Console.WriteLine(url);
                string sql = "load data local infile \"" + url + "\" into table material fields terminated by '\t';";
                Console.WriteLine(sql);
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    int result = cmd.ExecuteNonQuery();
                    if (result != 0)
                    {
                        MessageBox.Show("导入成功!");
                    }
                    else
                    {
                        MessageBox.Show("导入失败!");
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("导入失败!");
                }
                finally
                {
                    conn.Close();
                    getMaterialTable(1);
                }
            }
            else
            {
                getMaterialTable(1);
            }
        }
        

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            MaterialModifyWindow mmw = new MaterialModifyWindow(material,this);
            mmw.Show();
            getMaterialTable(1);
        }

        public void getMaterialTable(int page){
            try
            {
                string sql_count = string.Format("select count(*) from material");
                conn.Open();//建立连接 
                if (page == 1)
                {
                    Sqlutils(sql_count);
                }
                int begin = (page - 1) * limit;
                total_num.Content = totalPage;
                string sql = string.Format("select * from (select (@i:= @i+1) as k,mid,mname,repository_id,rest,cycle,each_price,buy_type from material,(SELECT @i:=0) as i order by mname) as new where k>'{0}' and k<='{1}'", begin, begin + limit);
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                lv.ItemsSource = ds.Tables[0].AsDataView();
                current_num.Content = page;  //设置当前页码
                search_type = 0;
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

        private void ordersItemClick(object sender, SelectionChangedEventArgs e) {
            DataRowView rowSelected = lv.SelectedItem as DataRowView;
            if (rowSelected != null) {
                material = new Material();               
                material.mid = rowSelected["mid"].ToString();
                material.mname = rowSelected["mname"].ToString();
                material.repository_id = rowSelected["repository_id"].ToString();
                material.rest = rowSelected["rest"].ToString();
                material.cycle = rowSelected["cycle"].ToString();
                material.each_price = rowSelected["each_price"].ToString();
                material.buy_type = rowSelected["buy_type"].ToString();
            }
        }


        public void searchByName(int page)
        {
            try
            {
                string sql_count = string.Format("select count(*) from material where mname='{0}'", txtMname.Text.Trim());
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                if (page == 1)
                {
                    Sqlutils(sql_count);
                }
                int begin = (page - 1) * limit;
                total_num.Content = totalPage;
                string sql = string.Format("select * from (select (@i:= @i+1) as k,mid,mname,repository_id,rest,cycle,each_price,buy_type from material,(SELECT @i:=0) as i where mname='{2}' order by mname) as new where k>'{0}' and k<='{1}'", begin, begin + limit, txtMname.Text.Trim());
                //对数据库进行查询
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                lv.ItemsSource = ds.Tables[0].AsDataView();
                current_num.Content = page;
                search_type = 1;
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
        /*
        private void ordersItemClick(object sender, SelectionChangedEventArgs e)
        {

        }
        */
        //上一页
        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            int currentpage = (int)current_num.Content;
            if (currentpage == 1) { return; }
            else if (search_type == 0)
            {
                getMaterialTable(currentpage - 1);

            }
            else if (search_type == 1)
            {
                searchByName(currentpage - 1);
            }
        }

        //下一页
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            int currentpage = (int)current_num.Content;
            if (currentpage == totalPage) { return; }
            else if (search_type == 0)
            {
                getMaterialTable(currentpage + 1);
            }
            else if (search_type == 1)
            {
                searchByName(currentpage + 1);
            }
        }

        //跳转任意页
        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (go_num.Text.Trim() == "")
            {
                return;
            }
            int gopage = Convert.ToInt32(go_num.Text.Trim());
            if (gopage > totalPage) { return; }
            else if (gopage < 1) { return; }
            else if (search_type == 0)
            {
                getMaterialTable(gopage);
            }
            else if (search_type == 1)
            {
                searchByName(gopage);
            }
            go_num.Text = "";
        }

        //sql分页工具类
        public void Sqlutils(string sql_count)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql_count, conn);
                //执行查询，并返回查询结果集中第一行的第一列。所有其他的列和行将被忽略。
                Object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    totalCount = int.Parse(result.ToString());
                }
                if (totalCount % limit == 0)
                {
                    totalPage = totalCount / limit;
                }
                else
                {
                    totalPage = totalCount / limit + 1;
                }
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
}
