using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class SubjectViewModel
    {
        [DisplayName("ID")]
        public int id_subject { get; set; }

        [DisplayName("Tên môn")]
        public string subject_name { get; set; }

        [DisplayName("Số lượng chuyên đề")]
        public int count_thematic { get; set; }

    }
}
