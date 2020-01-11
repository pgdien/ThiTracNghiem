namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        public int id_admin { get; set; }

        [Required]
        [StringLength(20)]
        public string username { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }

        [StringLength(255)]
        public string avatar { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        public int id_permission { get; set; }

        public DateTime? last_login { get; set; }

        [StringLength(100)]
        public string last_seen { get; set; }

        [StringLength(100)]
        public string last_seen_url { get; set; }
    }
}
