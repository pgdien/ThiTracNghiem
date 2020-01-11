namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        [Key]
        public int id_question { get; set; }

        public int id_thematic { get; set; }

        [Required]
        [StringLength(500)]
        public string question_content { get; set; }

        [StringLength(255)]
        public string img { get; set; }

        [Required]
        [StringLength(500)]
        public string A { get; set; }

        [Required]
        [StringLength(500)]
        public string B { get; set; }

        [Required]
        [StringLength(500)]
        public string C { get; set; }

        [Required]
        [StringLength(500)]
        public string D { get; set; }

        [Required]
        [StringLength(500)]
        public string correct_answer { get; set; }

        public int? is_change { get; set; }

        public int? is_essay { get; set; }
    }
}
