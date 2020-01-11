using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
   public class Exam_RoomViewModel
    {
        [Key]
        public int id_room
        {
            get;
            set;
        }

        [Required]
        public string thread_name
        {
            get;
            set;
        }

        [Required]
        [StringLength(50)]
        public string room_name
        {
            get;
            set;
        }

        [StringLength(50)]
        public string room_pass
        {
            get;
            set;
        }

        public string timetodo
        {
            get;
            set;
        }
    }
}
