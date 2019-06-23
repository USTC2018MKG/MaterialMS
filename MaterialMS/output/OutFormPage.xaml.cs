using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
        private int search_type;     //全部查询为0，按日期查询为1

        public OutFormPage()
        {
            InitializeComponent();
            getInOrderTable(1);
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            if (tbForSearch.Text.Trim() == "" && dpYearStart.Text.Trim() == "" && dpYearEnd.Text.Trim() == "")
            {
                tblSearchMsg.Text = "请输入订单编号或起止日期";
                tbForSearch.Focus();
                return;
            }//按照日期查询
            else if (tbForSearch.Text.Trim() == "")
            {
                tblSearchMsg.Text = "";
                searchByTime(1);
            }
            else {
                tblSearchMsg.Text = ""; 
                string sql = string.Format("select * from out_order,user where out_order.employee_id = user.employee_id and out_order.out_id = '{0}' order by out_time desc", tbForSearch.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接
                    //对数据库进行查询
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    List<Out> outs = new List<Out>();
                    foreach (DataRow mDr in ds.Tables[0].Rows)
                    {
                        Out o = new Out();
                        foreach (DataColumn mDc in ds.Tables[0].Columns)
                        {
                            if (mDc.ColumnName.Equals("out_id"))
                            {
                                o.out_id = mDr[mDc].ToString();
                            }
                            else if (mDc.ColumnName.Equals("out_time"))
                            {
                                o.out_time = mDr[mDc].ToString();
                            }
                            else if (mDc.ColumnName.Equals("employee_id"))
                            {
                                o.employee_id = mDr[mDc].ToString();
                            }
                            else if (mDc.ColumnName.Equals("user_name"))
                            {
                                o.user_name = mDr[mDc].ToString();
                            }                           
                            else if (mDc.ColumnName.Equals("phone"))
                            {
                                o.phone = mDr[mDc].ToString();
                            }
                            
                            else if (mDc.ColumnName.Equals("type"))
                            {
                                if (mDr[mDc].ToString().Equals("0"))
                                {
                                    o.type = "员工";                 
                                }
                                else if (mDr[mDc].ToString().Equals("1"))
                                {
                                    o.type = "工程师";
                                }
                                else if (mDr[mDc].ToString().Equals("2"))
                                {
                                    o.type = "现场主管";                                  
                                }
                                else if (mDr[mDc].ToString().Equals("3"))
                                {
                                    o.type = "现场经理";
                                }
                                else if (mDr[mDc].ToString().Equals("10"))
                                {
                                    o.type = "超级管理员";
                                }
                                else if (mDr[mDc].ToString().Equals("11"))
                                {
                                    o.type = "刀具添加员";
                                }                                
                            }
                            else if (mDc.ColumnName.Equals("change_type"))
                            {
                                if (mDr[mDc].ToString().Equals("0"))
                                {
                                    o.change_type = "以旧换新";
                                }
                                else if (mDr[mDc].ToString().Equals("1"))
                                {
                                    o.change_type = "异常领取";
                                }
                                else if (mDr[mDc].ToString().Equals("2"))
                                {
                                    o.change_type = "工艺新增";
                                }
                                
                            }
                        }
                        outs.Add(o);                    
                    }
                    lvOrders.ItemsSource = outs;
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
            Out rowSelected = lvOrders.SelectedItem as Out;
            if (rowSelected != null)
            {
                outDetailWindow = new OutDetailWindow(rowSelected);
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
                string sql = string.Format("select * from (select (@i:= @i+1) as k,out_id,out_time,employee_id,admin_id,state,mode,change_type from out_order,(SELECT @i:=0) as i) as new,user where user.employee_id=new.employee_id and k>'{0}' and k<='{1}' order by out_time desc", begin, begin + limit);
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                List<Out> outs = new List<Out>();
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    Out o = new Out();
                    foreach (DataColumn mDc in ds.Tables[0].Columns)
                    {
                        if (mDc.ColumnName.Equals("out_id"))
                        {
                            o.out_id = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("out_time"))
                        {
                            o.out_time = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("employee_id"))
                        {
                            o.employee_id = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("user_name"))
                        {
                            o.user_name = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("phone"))
                        {
                            o.phone = mDr[mDc].ToString();
                        }

                        else if (mDc.ColumnName.Equals("type"))
                        {
                            if (mDr[mDc].ToString().Equals("0"))
                            {
                                o.type = "员工";
                            }
                            else if (mDr[mDc].ToString().Equals("1"))
                            {
                                o.type = "工程师";
                            }
                            else if (mDr[mDc].ToString().Equals("2"))
                            {
                                o.type = "现场主管";
                            }
                            else if (mDr[mDc].ToString().Equals("3"))
                            {
                                o.type = "现场经理";
                            }
                            else if (mDr[mDc].ToString().Equals("10"))
                            {
                                o.type = "超级管理员";
                            }
                            else if (mDr[mDc].ToString().Equals("11"))
                            {
                                o.type = "刀具添加员";
                            }
                        }
                        else if (mDc.ColumnName.Equals("change_type"))
                        {
                            if (mDr[mDc].ToString().Equals("0"))
                            {
                                o.change_type = "以旧换新";
                            }
                            else if (mDr[mDc].ToString().Equals("1"))
                            {
                                o.change_type = "异常领取";
                            }
                            else if (mDr[mDc].ToString().Equals("2"))
                            {
                                o.change_type = "工艺新增";
                            }

                        }
                        else if (mDc.ColumnName.Equals("state"))
                        {
                            if (mDr[mDc].ToString().Equals("0"))
                            {
                                o.state = "下单成功";
                            }
                            else if (mDr[mDc].ToString().Equals("1"))
                            {
                                o.state = "PLC出柜成功";
                            }
                            else if (mDr[mDc].ToString().Equals("2"))
                            {
                                o.state = "PLC出柜失败";
                            }
                            else if (mDr[mDc].ToString().Equals("3"))
                            {
                                o.state = "领取成功";
                            }
                            else if (mDr[mDc].ToString().Equals("4"))
                            {
                                o.state = "超时";
                            }

                        }
                    }
                    outs.Add(o);
                }
                lvOrders.ItemsSource = outs;
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


        public void searchByTime(int page)
        {
            try
            {
                string starttime = dpYearStart.SelectedDate.Value.ToString("yyyy-MM-dd");
                string endtime = dpYearEnd.SelectedDate.Value.ToString("yyyy-MM-dd");
                string sql_count = string.Format("select count(*) from out_order where out_time >= '{0}' and out_time <= '{1}'", starttime, endtime);
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                if (page == 1)
                {
                    Sqlutils(sql_count);
                }
                int begin = (page - 1) * limit;
                total_num.Content = totalPage;
                //string sql = string.Format("select * from (select (@i:= @i+1) as k,out_id,out_time,employee_id,state from out_order,(SELECT @i:=0) as new,user where new.employee_id = user.employee_id and out_time='{2}') as new where k>'{0}' and k<='{1}'", begin, begin + limit, dpYear.Text.Trim());
                string sql = string.Format("select * from (select (@i:= @i+1) as k,out_id,out_time,employee_id,admin_id,state,mode,change_type from out_order," +
                    "(SELECT @i:=0) as i where out_time >= '{2}' and out_time <= '{3}') as new,user " +
                    "where user.employee_id=new.employee_id and k>'{0}' and k<='{1}' order by out_time desc", begin, begin + limit, starttime, endtime);
                //对数据库进行查询
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                List<Out> outs = new List<Out>();
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    Out o = new Out();
                    foreach (DataColumn mDc in ds.Tables[0].Columns)
                    {
                        if (mDc.ColumnName.Equals("out_id"))
                        {
                            o.out_id = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("out_time"))
                        {
                            o.out_time = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("employee_id"))
                        {
                            o.employee_id = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("user_name"))
                        {
                            o.user_name = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("phone"))
                        {
                            o.phone = mDr[mDc].ToString();
                        }

                        else if (mDc.ColumnName.Equals("type"))
                        {
                            if (mDr[mDc].ToString().Equals("0"))
                            {
                                o.type = "员工";
                            }
                            else if (mDr[mDc].ToString().Equals("1"))
                            {
                                o.type = "工程师";
                            }
                            else if (mDr[mDc].ToString().Equals("2"))
                            {
                                o.type = "现场主管";
                            }
                            else if (mDr[mDc].ToString().Equals("3"))
                            {
                                o.type = "现场经理";
                            }
                            else if (mDr[mDc].ToString().Equals("10"))
                            {
                                o.type = "超级管理员";
                            }
                            else if (mDr[mDc].ToString().Equals("11"))
                            {
                                o.type = "刀具添加员";
                            }
                        }
                        else if (mDc.ColumnName.Equals("change_type"))
                        {
                            if (mDr[mDc].ToString().Equals("0"))
                            {
                                o.change_type = "以旧换新";
                            }
                            else if (mDr[mDc].ToString().Equals("1"))
                            {
                                o.change_type = "异常领取";
                            }
                            else if (mDr[mDc].ToString().Equals("2"))
                            {
                                o.change_type = "工艺新增";
                            }

                        }
                    }
                    outs.Add(o);
                }
                lvOrders.ItemsSource = outs;
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
                searchByTime(currentpage - 1);
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
                searchByTime(currentpage + 1);
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
                searchByTime(gopage);
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
