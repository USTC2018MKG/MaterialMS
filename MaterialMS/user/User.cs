using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialMS
{
    public class User
    {
        public String user_id { get; set; }
        public String employee_id { get; set; }
        public String name { get; set; }
        public String password { get; set; }
        public String sex { get; set; }
        public String phone { get; set; }
        public String type { get; set; }
        public String state { get; set; }

        public String sexString { get; set; }
        public String stateString { get; set; }
        public String typeString { get; set; }

    }
}
