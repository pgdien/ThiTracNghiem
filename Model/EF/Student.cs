namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Student")]
    public partial class Student
    {
        [Key]
        public int id_student { get; set; }

        [StringLength(50)]
        public string username { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [Required]
        [StringLength(100)]
        public string student_name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? student_birtday { get; set; }

        public bool? student_gender { get; set; }

        [StringLength(255)]
        public string student_avatar { get; set; }

        public int id_class { get; set; }

        public DateTime? last_login { get; set; }

        [StringLength(100)]
        public string last_seen { get; set; }

        [StringLength(100)]
        public string last_seen_url { get; set; }

        [StringLength(50)]
        public string ip { get; set; }

        [StringLength(500)]
        public string info_browser { get; set; }
    }
}
