using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class StudentViewModel
    {

        [Key]
        [DisplayName("ID")]
        public int id_student { get; set; }

        [StringLength(50)]
        [DisplayName("Tên đăng nhập")]
        public string username { get; set; }


        [Required]
        [StringLength(100)]
        [DisplayName("Tên sinh viên")]
        public string student_name { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày sinh")]
        public DateTime? student_birtday { get; set; }
        [DisplayName("Giới tính")]
        public bool? student_gender { get; set; }

        [StringLength(255)]
        public string student_avatar { get; set; }

        public string class_name { get; set; }


       
    }
}
