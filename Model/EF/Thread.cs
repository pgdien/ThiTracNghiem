namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Thread")]
    public partial class Thread
    {
        [Key]
        public int id_thread { get; set; }

        [Required]
        [StringLength(255)]
        public string thread_name { get; set; }

        public int max_question { get; set; }

        public int id_subject { get; set; }

        public int time_to_do { get; set; }

        public bool thread_status { get; set; }

        public int essay_score { get; set; }
    }
}
