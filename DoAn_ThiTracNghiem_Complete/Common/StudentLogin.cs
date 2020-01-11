using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_ThiTracNghiem_Complete.Common
{
    [Serializable]
    public class StudentLogin
    {
        public int id_student { get; set; }
        public string student_name { get; set; }
        public string student_avatar { get; set; }
        public string name { get; set; }

    }
}