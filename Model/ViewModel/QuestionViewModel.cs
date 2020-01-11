using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class QuestionViewModel
    {
        [Key]
        public int id_question { get; set; }

        [Required]
        [StringLength(500)]
        public string question_content { get; set; }

        [StringLength(255)]
        public string img { get; set; }

        [Required]
        [StringLength(100)]
        public string A { get; set; }

        [Required]
        [StringLength(100)]
        public string B { get; set; }

        [Required]
        [StringLength(100)]
        public string C { get; set; }

        [Required]
        [StringLength(100)]
        public string D { get; set; }

        [Required]
        [StringLength(100)]
        public string correct_answer { get; set; }

        public int? is_change { get; set; }

        public int? is_essay { get; set; }

        [DisplayName("ID")]
        public int id_thematic { get; set; }

        [DisplayName("Tên chuyên đề")]
        public string thematic_name { get; set; }

        [DisplayName("ID môn")]
        public int id_subject { get; set; }
    }
}
