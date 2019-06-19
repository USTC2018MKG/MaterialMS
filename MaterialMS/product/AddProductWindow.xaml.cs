using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
    /// AddProductWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddProductWindow : Window
    {
        private ObservableCollection<ProductItem> productItems;
        private List<ProductItem> list = new List<ProductItem>();
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (txtId.Text.Trim() == "")
            {
                labIdMsg.Content = "请输入产品编号!";
                txtId.Focus();
                return;
            }
            else if (txtName.Text.Trim() == "")
            {
                labNameMsg.Content = "请输入产品名称!";
                txtName.Focus();
                return;
            }
            else if (txtPred.Text.Trim() == "")
            {
                labPredMsg.Content = "请输入预测产能!";
                txtPred.Focus();
                return;
            }
            else if (txtTool.Text.Trim() == "")
            {
                labToolMsg.Content = "请输入机床名称";
                txtTool.Focus();
                return;
            }
            else if (list.Count < 1)
            {
                labChoiceMsg.Content = "请添加刀具信息";
                return;
            }
            else {
                insertProduct();
            }
        }

        //写入产品信息，写入成功，在添加产品项信息
        private void insertProduct() {
            //连接数据库对象
            MySqlConnection conn = new MySqlConnection(Constant.myConnectionString);
            conn.Open();
            //打开通道，建立连接，可能出现异常,使用try catch语句   
            MySqlTransaction transaction = conn.BeginTransaction();
            string sql = string.Format("insert into product (pid,pro_pred,tool,pname) values('{0}','{1}','{2}','{3}')", 
                txtId.Text.Trim(), txtPred.Text.Trim(),txtTool.Text.Trim(),txtName.Text.Trim() );           
        
            try {            
                MySqlCommand cmd = new MySqlCommand(sql, conn,transaction);
                int result = cmd.ExecuteNonQuery();
                if (result != 0)
                {
                    int count = 0;
                    foreach (ProductItem item in list)
                    {
                        Console.WriteLine(item.mid);
                        string msql = string.Format("insert into product_item " +
                            "(pid,mid,num,maxsafe_repo,minwarning_repo,pred_knife_num) " +
                            "values('{0}','{1}','{2}','{3}','{4}','{5}')", txtId.Text.Trim(),item.mid,item.num,item.maxsafe_repo,item.minwarning_repo,item.pred_knife_num);
                        MySqlCommand mycmd = new MySqlCommand(msql, conn, transaction);
                        mycmd.ExecuteNonQuery();
                        count++;
                    }
                    if (count == list.Count())
                    {
                        MessageBox.Show("插入成功!");
                        transaction.Commit();
                        productItems.Clear();
                        this.Close();
                    }
                    else {
                        MessageBox.Show("插入失败!");
                        txtId.Text = "";
                        txtName.Text = "";
                        txtPred.Text = "";
                        txtTool.Text = "";
                        transaction.Rollback();
                    }
                                     
                }
                else
                {
                    MessageBox.Show("插入失败!");
                    txtId.Text = "";
                    txtName.Text = "";
                    txtPred.Text = "";
                    txtTool.Text = "";
                    transaction.Rollback();
                }
                
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("插入失败!");
                txtId.Text = "";
                txtName.Text = "";
                txtPred.Text = "";
                txtTool.Text = "";
                transaction.Rollback();                    
            }
            finally
            {
                conn.Close();
            }
        }


        //产品项添加
        private void SelectKnife_Click(object sender, RoutedEventArgs e)
        {
            if (txtKnife.Text.Trim() != "" && txtNum.Text.Trim() != ""  && txtPred.Text.Trim() != "")
            {
                MySqlConnection connection = new MySqlConnection(Constant.myConnectionString);
                connection.Open();
                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "select * from material where mid=@mid";
                    cmd.Parameters.Add(new MySqlParameter("@mid", MySqlDbType.VarChar, 50));
                    cmd.Parameters["@mid"].Value = txtKnife.Text;
                    MySqlDataReader sdr = cmd.ExecuteReader();
                    if (!sdr.Read())
                    {
                        labKnifeMsg.Content = "刀具不存在！请重新输入";
                        txtKnife.Text = "";
                        txtNum.Text = "";
                        txtKnife.Focus();
                    }
                    else if (int.Parse(sdr["rest"].ToString().Trim()) < int.Parse(txtNum.Text.Trim()))
                    {
                        labNumMsg.Content = "刀具库存剩余" + sdr["rest"].ToString().Trim();
                        txtNum.Text = "";
                        txtNum.Focus();
                    }
                    else
                    {

                        ProductItem itemOld = findItem(txtKnife.Text.Trim());
                        if (itemOld != null)
                        {
                            if(itemOld.num + int.Parse(txtNum.Text.Trim()) > int.Parse(sdr["rest"].ToString().Trim()))
                            {
                                labNumMsg.Content = "刀具库存剩余" + sdr["rest"].ToString().Trim();
                            }
                            else {
                                itemOld.num += int.Parse(txtNum.Text.Trim());
                            }
                        }                                               
                        else
                        {
                            ProductItem prols = new ProductItem();
                            prols.mid = txtKnife.Text.Trim();
                            prols.num = int.Parse(txtNum.Text.Trim());
                            prols.rest = int.Parse(sdr["rest"].ToString().Trim());
                            prols.pred_knife_num = int.Parse(txtPred.Text.Trim())/int.Parse(sdr["pred_age"].ToString().Trim())
                                *int.Parse(sdr["knife_num"].ToString().Trim());
                            prols.maxsafe_repo = (int)(prols.pred_knife_num * 1.1);
                            prols.minwarning_repo = (int)(prols.pred_knife_num * 1.05);
                            list.Add(prols);
                        }
                        productItems = new ObservableCollection<ProductItem>(list);
                        lv.ItemsSource = productItems;                        
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();

                    }
                }
            }
            else if (txtKnife.Text.Trim() == "")
            {
                labKnifeMsg.Content = "请输入刀具编号";                
                txtKnife.Focus();
            }
            else {
                labNumMsg.Content = "请输入刀具数量";
                txtNum.Focus();
            }
        }

        private ProductItem findItem(String mid) {
            foreach (ProductItem i in list)
            {
                if (i.mid.Equals( mid))
                {
                    // item.num += int.Parse(txtNum.Text);
                    return i;
                }

            }
            return null;
        }

        // 刀具数量减一
        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            lv.SelectedItem = ((Button)sender).DataContext;
            ProductItem item = lv.SelectedItem as ProductItem;
            if (item.num > 0)
            {
                item.num--;
            }
            else
            {
                MessageBox.Show("刀具数量不能为负");
            }
        }

        // 刀具数量+1
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            lv.SelectedItem = ((Button)sender).DataContext;
            ProductItem item = lv.SelectedItem as ProductItem;
            if (item.num < item.rest)
            {
                item.num++;
            }
            else
            {
                MessageBox.Show("数量不可超过库存！");
            }
        }

        // 不需要某种刀具，直接删除条目
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确定不需要此种刀具吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.OK)
            {
                lv.SelectedItem = ((Button)sender).DataContext;
                ProductItem item = lv.SelectedItem as ProductItem;
                productItems.Remove(item);
            }
        }

        //生成二维码
        private void CreateQR_Click(object sender, RoutedEventArgs e) {
            if (txtId.Text != "")
            {
                Bitmap bitmap = BarcodeHelper.CreatQR(txtId.Text.ToString(), 70, 70);
                ImageSource imageSource = BarcodeHelper.loadBitmap(bitmap);
                img_br.Source = imageSource;
            }
            else {
                labIdMsg.Content = "请输入产品编号!";
                txtId.Focus();
                return;
            }    
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确定不需要此种刀具吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.OK)
            {
                lv.SelectedItem = ((Button)sender).DataContext;
                ProductItem item = lv.SelectedItem as ProductItem;
                productItems.Remove(item);
            }
        }
    }
}
