namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Class")]
    public partial class Class
    {
        [Key]
        public int id_class { get; set; }

        [Required]
        [StringLength(50)]
        public string class_name { get; set; }

        public int id_speciality { get; set; }

        public bool class_status { get; set; }
    }
}
