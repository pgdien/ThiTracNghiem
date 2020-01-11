namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Speciality")]
    public partial class Speciality
    {
        [Key]
        public int id_speciality { get; set; }

        [Required]
        [StringLength(50)]
        public string speciality_name { get; set; }
    }
}
