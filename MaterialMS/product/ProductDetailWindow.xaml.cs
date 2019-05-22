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
using System.Windows.Shapes;

namespace MaterialMS.product
{
    /// <summary>
    /// ProductDetailWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProductDetailWindow : Window
    {
        private MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
        private Product product;
        public ProductDetailWindow(Product product)
        {
            InitializeComponent();
            this.product = product;
            getProductItemTable();
        }

        private void getProductItemTable()
        {
            try
            {
                string sql = string.Format("select * from product_item where pid = '{0}'", product.pid);
                Console.WriteLine(product.pid);
                MySqlDataAdapter md = new MySqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                Console.WriteLine(ds);
                md.Fill(ds);
                lvProductDetail.ItemsSource = ds.Tables[0].AsDataView();
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
