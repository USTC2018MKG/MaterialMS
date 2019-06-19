using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialMS.product
{
    class ProductItem : INotifyPropertyChanged
    {
        public String plid { set; get; }
        public String pid { set; get; }
        public String mid { set; get; }
        // 一个产品需要此种刀具的数量
        private int Num;
        public int num {
            get {
                return this.Num;
            }

            set {
                if (this.Num != value) {
                    this.Num = value;
                    this.NotifyPropertyChanged("num");
                }
            }
        }
       
        public int maxsafe_repo { set; get; }
        public int minwarning_repo { set; get; }
        public int pred_knife_num { set; get; }
        public int rest { set; get; }

        //组件属性变更
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
