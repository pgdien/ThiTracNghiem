using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ThematicViewModel
    {
        [DisplayName("ID")]
        public int id_thematic { get; set; }

        [DisplayName("Tên chuyên đề")]
        public string thematic_name { get; set; }

        [DisplayName("Tên môn")]
        public string subject_name { get; set; }

    }
}
