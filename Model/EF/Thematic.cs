namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Thematic")]
    public partial class Thematic
    {
        [Key]
        public int id_thematic { get; set; }

        public int id_subject { get; set; }

        [Required]
        [StringLength(100)]
        public string thematic_name { get; set; }
    }
}
