using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialMS.product
{
    class ProductItem
    {
        public String plid { set; get; }
        public String pid { set; get; }
        public String mid { set; get; }
        public int num { set; get; }
        public int maxsafe_repo { set; get; }
        public int minwarning_repo { set; get; }
        public int pred_knife_num { set; get; }
    }
}
