using MaterialMS.output;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaterialMS.output
{
    /// <summary>
    /// OutDetailWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OutDetailWindow : Window
    {
        Out output;
        public OutDetailWindow(Out output)
        {
            this.output = output;
            InitializeComponent();
            getItemTable();
        }

        public void getItemTable()
        {
            //连接数据库对象
            MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
            string sql = string.Format("select * from out_item where out_id = '{0}'", output.out_id);
            try
            {
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                md.Fill(ds);
                lvOrderDetail.ItemsSource = ds.Tables[0].AsDataView();
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
