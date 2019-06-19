using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialMS
{
    public class Material: INotifyPropertyChanged
    {
        public String mid{ get; set; }
        public String mname { get; set; }
        public String cycle { get; set; }
        public String buy_type { get; set; }
        public String shopping_car{ get; set; }
        public String first_repo { get; set; }
        public String repository_id { get; set; }
        public String ntax_price { get; set; }
        public String knife_num { get; set; }
        public String rotate_num { get; set; }
        public String pred_age { get; set; }
        public String exchange { get; set; }
        public String get_max { get; set; }
        public String each_price { get; set; }
        public String rest { get; set; }
        private String num;


        public String add_num
        {
            get
            {
                return this.num;
            }

            set
            {
                if (this.num != value)
                {
                    this.num = value;
                    this.NotifyPropertyChanged("add_num");
                }
            }
        }
        //组件属性变更
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
