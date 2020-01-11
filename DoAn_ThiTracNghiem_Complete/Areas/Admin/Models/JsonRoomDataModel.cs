using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Models
{
    public class JsonRoomDataModel
    {
        public string id { get; set; }
        public string student_name { get; set; }
        public string id_exam { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string score { get; set; }
        public string status { get; set; }
        public string data_client { get; set; }
        public string changetab { get; set; }
        public string action { get; set; }
    }
}