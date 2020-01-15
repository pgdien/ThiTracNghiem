namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuestionOfExam")]
    public partial class QuestionOfExam
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_exam { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_question { get; set; }

        [StringLength(255)]
        public string img { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(500)]
        public string question_content { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(500)]
        public string B { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(500)]
        public string C { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(500)]
        public string D { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(500)]
        public string A { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(500)]
        public string correct_answer { get; set; }

        public int? is_essay { get; set; }
    }
}
