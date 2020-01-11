using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ThreadViewModel
    {
        [Key]
        public int id_thread { get; set; }

        [Required]
        [StringLength(255)]
        public string thread_name { get; set; }
        
        public int max_question { get; set; }
        public int count_exam { get; set; }
        public int count_room { get; set; }

        [StringLength(255)]
        public string subject_name { get; set; }

        public int time_to_do { get; set; }

        public bool thread_status { get; set; }

    }
}
