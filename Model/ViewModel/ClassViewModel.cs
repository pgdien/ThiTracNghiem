using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
  public  class ClassViewModel
    {
        [DisplayName("ID")]
        public int id_class { get; set; }

        [DisplayName("Tên lớp")]
        public string class_name { get; set; }

        [DisplayName("Ngành")]
        public string speciality_name { get; set; }

        [DisplayName("Trạng thái")]
        public bool class_status { get; set; }
    }
}
