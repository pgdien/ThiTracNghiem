namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Student_Thread_Info
    {
        public int? id_exam { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_thread { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_student { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_room { get; set; }

        public DateTime? start_time { get; set; }

        public bool? is_complete { get; set; }

        public DateTime? end_time { get; set; }

        public decimal? score_number { get; set; }

        [StringLength(8)]
        public string time_remaing { get; set; }

        public int? count_tab_focus { get; set; }
    }
}
