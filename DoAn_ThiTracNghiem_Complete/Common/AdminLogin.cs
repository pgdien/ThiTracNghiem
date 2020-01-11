using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_ThiTracNghiem_Complete.Common
{
    [Serializable]
    public class AdminLogin
    {
        public int id_admin { get; set; }
        public string username { get; set; }
        public int id_permission { get; set; }
        public string name { get; set; }

    }
}