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
                string sql = string.Format("select * from user where emplyee_id = '{0}' order by user_name", txtId.Text.Trim());
                try
                {
                    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                    total_num.Content = 1;
                    MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    md.Fill(ds);
                    dg1.ItemsSource = ds.Tables[0].AsDataView();
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

        private void Modify_Click(object sender, RoutedEventArgs e)
        {           
            UserModifyWindow umw = new UserModifyWindow(user,this);
            umw.Show();
            getUserTable(1);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (user != null)
            {
                if (user.type.Equals("0"))
                {
                    MessageBox.Show("无删除超级管理员权限!");
                    return;
                }
                else if (user.type.Equals("1"))
                {
                    if (!Account.Instance.GetUser().type.Equals("0"))
                    {
                        MessageBox.Show("无删除管理员权限!");
                        return;
                    }
                }
                string name = user.name;
                string msg = "确定要删除用户" + name + "吗？";
                string sql = string.Format("update user set state=1 where emplyee_id='{0}'", user.emplyee_id);
                
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
                        MessageBox.Show("插入失败!");
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
                MessageBox.Show("请点击要删除的用户行!");
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
                string sql = string.Format("select * from (select (@i:= @i+1) as k,emplyee_id,user_name,sex,phone,state,age,type from user,(SELECT @i:=0) as i order by user_name) as new where k>'{0}' and k<='{1}'", begin,begin+limit);
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                dg1.ItemsSource = ds.Tables[0].AsDataView();
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
                string sql = string.Format("select * from (select (@i:= @i+1) as k,emplyee_id,user_name,sex,phone,state,age,type from user,(SELECT @i:=0) as i where user_name='{2}' order by user_name) as new where k>'{0}' and k<='{1}'", begin, begin + limit, txtName.Text.Trim());
                //对数据库进行查询
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                dg1.ItemsSource = ds.Tables[0].AsDataView();
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

        private void Dg1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            DataRowView rowSelected = dg1.SelectedItem as DataRowView;
            if (rowSelected != null) {
                user = new User();                
                user.emplyee_id = rowSelected["emplyee_id"].ToString();
                user.name = rowSelected["user_name"].ToString();
                user.phone = rowSelected["phone"].ToString();
                user.sex = rowSelected["sex"].ToString();
                user.age = rowSelected["age"].ToString();
                user.type = rowSelected["type"].ToString();
            }
            
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

