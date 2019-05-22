using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// UserManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagePage : Page
    {
        private MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
        private User user;
        private int totalCount = 0;          //查询总数
        private int limit = 12;          //设置每页显示记录数
        private int totalPage;       //最大的页码数
        private int search_type;     //全部查询为0，按用户名查询为1

        public UserManagePage()
        {
            InitializeComponent();
            getUserTable(1);
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

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Trim() == "" && txtId.Text.Trim() == "")
            {
                labSearchMsg.Content = "请输入员工姓名或编号!";
                txtName.Focus();
                return;
            }
            //按照姓名查询
            else if (txtId.Text.Trim() == "")
            {
                labSearchMsg.Content = "";
                searchByName(1);
            }
            //按照ID查询
            else
            {
                string sql = string.Format("select * from user where employee_id = '{0}' order by user_name", txtId.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    total_num.Content = 1;
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    List<User> users = new List<User>();
                    foreach (DataRow mDr in ds.Tables[0].Rows)
                    {
                        User u = new User();
                        foreach (DataColumn mDc in ds.Tables[0].Columns)
                        {
                            if (mDc.ColumnName.Equals("employee_id"))
                            {
                                u.employee_id = mDr[mDc].ToString();
                            }
                            else if (mDc.ColumnName.Equals("user_name"))
                            {
                                u.name = mDr[mDc].ToString();
                            }
                            else if (mDc.ColumnName.Equals("sex"))
                            {
                                if (mDr[mDc].ToString().Equals("0"))
                                {
                                    u.sexString = "女";
                                    u.sex = "0";
                                }
                                else
                                {
                                    u.sexString = "男";
                                    u.sex = "1";
                                }
                            }
                            else if (mDc.ColumnName.Equals("phone"))
                            {
                                u.phone = mDr[mDc].ToString();
                            }
                            else if (mDc.ColumnName.Equals("state"))
                            {
                                if (mDr[mDc].ToString().Equals("1"))
                                {
                                    u.stateString = "离职";
                                }
                                else
                                {
                                    u.stateString = "在职";
                                }
                            }
                            else if (mDc.ColumnName.Equals("type"))
                            {
                                if (mDr[mDc].ToString().Equals("0"))
                                {
                                    u.typeString = "员工";
                                    u.type = "0";
                                }
                                else if (mDr[mDc].ToString().Equals("1"))
                                {
                                    u.typeString = "工程师";
                                    u.type = "1";
                                }
                                else if (mDr[mDc].ToString().Equals("2"))
                                {
                                    u.typeString = "现场主管";
                                    u.type = "2";
                                }
                                else if (mDr[mDc].ToString().Equals("3"))
                                {
                                    u.typeString = "现场经理";
                                    u.type = "3";
                                }
                                else if (mDr[mDc].ToString().Equals("10"))
                                {
                                    u.typeString = "超级管理员";
                                }
                                else if (mDr[mDc].ToString().Equals("11"))
                                {
                                    u.typeString = "刀具添加员";
                                }
                            }
                        }
                        users.Add(u);
                    }
                    dg1.ItemsSource = users;
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

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            UserRegistWindow urw = new UserRegistWindow(this);
            urw.Show();
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
                string sql = "load data local infile \"" + url + "\" into table user fields terminated by '\t';";
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
                    getUserTable(1);
                }                
            }
            else
            {
                getUserTable(1);
            }           
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            dg1.SelectedItem = ((Button)sender).DataContext;
            User rowSelected = dg1.SelectedItem as User;
            
            if (rowSelected != null)
            {
                UserModifyWindow umw = new UserModifyWindow(rowSelected, this);
                umw.Show();
            }          
            getUserTable(1);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            dg1.SelectedItem = ((Button)sender).DataContext;
            User rowSelected = dg1.SelectedItem as User;
            if (rowSelected != null)
            {
                if (rowSelected.type.Equals("11"))
                {
                    MessageBox.Show("无删除权限!");
                    return;
                }
                string name = rowSelected.name;
                string msg = "确定要删除用户" + name + "吗？";
                string sql = string.Format("update user set state=1 where employee_id='{0}'", rowSelected.employee_id);
                
                MessageBoxResult dr = MessageBox.Show(msg, "删除用户", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (dr == MessageBoxResult.OK)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        int result = cmd.ExecuteNonQuery();
                        if (result != 0)
                        {
                            MessageBox.Show("删除成功!");
                        }
                        else
                        {
                            MessageBox.Show("删除失败!");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("删除失败!");
                    }
                    finally
                    {
                        conn.Close();
                        getUserTable(1);
                    }
                }
            }
            else
            {
                MessageBox.Show("请点击要选中的用户行!");
            }
        }

        public void getUserTable(int page) {
            try
            {
                string sql_count = string.Format("select count(*) from user");
                conn.Open();//建立连接 
                if(page == 1)
                {
                    Sqlutils(sql_count);
                }
                int begin = (page - 1) * limit;
                total_num.Content = totalPage;
                string sql = string.Format("select * from (select (@i:= @i+1) as k,employee_id,user_name,sex,phone,state,type from user,(SELECT @i:=0) as i order by user_name) as new where k>'{0}' and k<='{1}'", begin,begin+limit);
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);  

                List<User> users = new List<User>();
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    User u = new User();
                    foreach (DataColumn mDc in ds.Tables[0].Columns)
                    {
                        if (mDc.ColumnName.Equals("employee_id"))
                        {
                            u.employee_id = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("user_name"))
                        {
                            u.name = mDr[mDc].ToString();
                        } 
                        else if (mDc.ColumnName.Equals("sex"))
                        {
                            if (mDr[mDc].ToString().Equals("0"))
                            {
                                u.sexString = "女";
                                u.sex = "0";
                            }
                            else
                            {
                                u.sexString = "男";
                                u.sex = "1";
                            }
                        }
                        else if (mDc.ColumnName.Equals("phone"))
                        {
                            u.phone = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("state"))
                        {
                            if (mDr[mDc].ToString().Equals("1"))
                            {
                                u.stateString = "离职";
                                u.state = "1";
                            }
                            else
                            {
                                u.stateString = "在职";
                                u.state = "0";
                            }
                        }
                        else if (mDc.ColumnName.Equals("type"))
                        {
                            if (mDr[mDc].ToString().Equals("0"))
                            {
                                u.typeString = "员工";
                                u.type = "0";
                            }
                            else if (mDr[mDc].ToString().Equals("1")) {
                                u.typeString = "工程师";
                                u.type = "1";
                            }
                            else if (mDr[mDc].ToString().Equals("2"))
                            {
                                u.typeString = "现场主管";
                                u.type = "2";
                            }
                            else if (mDr[mDc].ToString().Equals("3"))
                            {
                                u.typeString = "现场经理";
                                u.type = "3";
                            }
                            else if (mDr[mDc].ToString().Equals("10"))
                            {
                                u.typeString = "超级管理员";
                                u.type = "10";
                            }
                            else if (mDr[mDc].ToString().Equals("11"))
                            {
                                u.typeString = "刀具添加员";
                                u.type = "11";
                            }
                        }
                    }
                    users.Add(u);
                }
                // dg1.ItemsSource = ds.Tables[0].AsDataView();
                dg1.ItemsSource = users;
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

        public void searchByName(int page)
        {
            try
            {
                string sql_count = string.Format("select count(*) from user where user_name='{0}'", txtName.Text.Trim());
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                if (page == 1)
                {
                    Sqlutils(sql_count);
                }
                int begin = (page - 1) * limit;
                total_num.Content = totalPage;               
                string sql = string.Format("select * from (select (@i:= @i+1) as k,employee_id,user_name,sex,phone,state,type from user,(SELECT @i:=0) as i where user_name='{2}' order by user_name) as new where k>'{0}' and k<='{1}'", begin, begin + limit, txtName.Text.Trim());
                //对数据库进行查询
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                List<User> users = new List<User>();

                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    User u = new User();
                    foreach (DataColumn mDc in ds.Tables[0].Columns)
                    {
                        if (mDc.ColumnName.Equals("employee_id"))
                        {
                            u.employee_id = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("user_name"))
                        {
                            u.name = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("sex"))
                        {
                            if (mDr[mDc].ToString().Equals("0"))
                            {
                                u.sexString = "女";
                                u.sex = "0";
                            }
                            else
                            {
                                u.sexString = "男";
                                u.sex = "1";
                            }
                        }
                        else if (mDc.ColumnName.Equals("phone"))
                        {
                            u.phone = mDr[mDc].ToString();
                        }
                        else if (mDc.ColumnName.Equals("state"))
                        {
                            if (mDr[mDc].ToString().Equals("1"))
                            {
                                u.stateString = "离职";
                                u.state = "1";
                            }
                            else
                            {
                                u.stateString = "在职";
                                u.state = "0";
                            }
                        }
                        else if (mDc.ColumnName.Equals("type"))
                        {
                            if (mDr[mDc].ToString().Equals("0"))
                            {
                                u.typeString = "员工";
                                u.type = "0";
                            }
                            else if (mDr[mDc].ToString().Equals("1"))
                            {
                                u.typeString = "工程师";
                                u.type = "1";
                            }
                            else if (mDr[mDc].ToString().Equals("2"))
                            {
                                u.typeString = "现场主管";
                                u.type = "2";
                            }
                            else if (mDr[mDc].ToString().Equals("3"))
                            {
                                u.typeString = "现场经理";
                                u.type = "3";
                            }
                            else if (mDr[mDc].ToString().Equals("10"))
                            {
                                u.typeString = "超级管理员";
                                u.type = "10";
                            }
                            else if (mDr[mDc].ToString().Equals("11"))
                            {
                                u.typeString = "刀具添加员";
                                u.type = "11";
                            }
                        }
                    }
                    users.Add(u);
                }
                dg1.ItemsSource = users;
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

        private void Dg1_SelectedCellsChanged(object sender, SelectionChangedEventArgs e) {
            
        }

        //上一页
        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            int currentpage = (int)current_num.Content;
            if(currentpage == 1) { return; }
            else if(search_type == 0)
            {
                getUserTable(currentpage - 1);

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
                getUserTable(currentpage + 1);
            }
            else if (search_type == 1)
            {
                searchByName(currentpage + 1);
            }
        }

        //跳转任意页
        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (go_num.Text.Trim() == "") {
                return;
            }
            int gopage = Convert.ToInt32(go_num.Text.Trim()); 
            if (gopage > totalPage) { return; }
            else if(gopage < 1) { return; }
            else if (search_type == 0)
            {
                getUserTable(gopage);
            }
            else if (search_type == 1)
            {
                searchByName(gopage);
            }
            go_num.Text = "";
        }
    }
}

