namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Exam_Room
    {
        [Key]
        public int id_room { get; set; }

        public int id_thread { get; set; }

        [Required]
        [StringLength(50)]
        public string room_name { get; set; }

        [StringLength(50)]
        public string room_pass { get; set; }
    }
}
