using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace MaterialMS.output
{
    /// <summary>
    /// OutFormPage.xaml 的交互逻辑
    /// </summary>
    public partial class OutFormPage : Page
{
        private Out output;
        private MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
        OutDetailWindow outDetailWindow;
        private int totalCount = 0;          //查询总数
        private int limit = 15;          //设置每页显示记录数
        private int totalPage;       //最大的页码数
        private int search_type;     //全部查询为0，按用户名查询为1

        public OutFormPage()
        {
            InitializeComponent();
            getInOrderTable(1);
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            if (tbForSearch.Text.Trim() == "" && dpYear.Text.Trim() == "")
            {
                tblSearchMsg.Text = "请输入订单编号或日期";
                tbForSearch.Focus();
                return;
            }//按照零件名查询
            else if (tbForSearch.Text.Trim() == "")
            {
                tblSearchMsg.Text = "";
                searchById(1);
            }
            else {
                tblSearchMsg.Text = "";
                //string date = Convert.ToDateTime(dpYear.Text).ToString("yyyyMMdd")
                string sql = string.Format("select * from out_order where out_id = '{0}'", tbForSearch.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接
                    //对数据库进行查询
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    lvOrders.ItemsSource = ds.Tables[0].AsDataView();
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

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            lvOrders.SelectedItem = ((Button)sender).DataContext;
            DataRowView rowSelected = lvOrders.SelectedItem as DataRowView;
            if (rowSelected != null)
            {
                output = new Out();
                output.out_id = rowSelected["out_id"].ToString();
                outDetailWindow = new OutDetailWindow(output);
                outDetailWindow.Show();
            }
        }

        public void getInOrderTable(int page)
        {
            try
            {
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                string sql_count = string.Format("select count(*) from out_order");
                if (page == 1)
                {
                    Sqlutils(sql_count);
                }
                int begin = (page - 1) * limit;
                total_num.Content = totalPage;                
                string sql = string.Format("select * from (select (@i:= @i+1) as k,out_id,out_time,employee_id from out_order,(SELECT @i:=0) as i) as new where k>'{0}' and k<='{1}'", begin, begin + limit, tbForSearch.Text.Trim());
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                lvOrders.ItemsSource = ds.Tables[0].AsDataView();
                current_num.Content = page;
                search_type = 0;
            }
            catch (MySqlException e)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void searchById(int page)
        {
            try
            {
                string sql_count = string.Format("select count(*) from out_order where out_time='{0}'", dpYear.Text.Trim());
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                if (page == 1)
                {
                    Sqlutils(sql_count);
                }
                int begin = (page - 1) * limit;
                total_num.Content = totalPage;
                string sql = string.Format("select * from (select (@i:= @i+1) as k,out_id,out_time,employee_id from out_order,(SELECT @i:=0) as i where out_time='{2}') as new where k>'{0}' and k<='{1}'", begin, begin + limit, dpYear.Text.Trim());
                //对数据库进行查询
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                lvOrders.ItemsSource = ds.Tables[0].AsDataView();
                current_num.Content = page;
                search_type = 1;
            }
            catch (MySqlException e)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        private void ordersItemClick(object sender, SelectionChangedEventArgs e)
        {

        }

        //上一页
        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            int currentpage = (int)current_num.Content;
            if (currentpage == 1) { return; }
            else if (search_type == 0)
            {
                getInOrderTable(currentpage - 1);

            }
            else if (search_type == 1)
            {
                searchById(currentpage - 1);
            }
        }

        //下一页
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            int currentpage = (int)current_num.Content;
            if (currentpage == totalPage) { return; }
            else if (search_type == 0)
            {
                getInOrderTable(currentpage + 1);
            }
            else if (search_type == 1)
            {
                searchById(currentpage + 1);
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
                getInOrderTable(gopage);
            }
            else if (search_type == 1)
            {
                searchById(gopage);
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
            catch (MySqlException e)
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
