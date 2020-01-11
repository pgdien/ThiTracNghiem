namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExamOfThread")]
    public partial class ExamOfThread
    {
        public int id_thread { get; set; }

        [Key]
        public int id_exam { get; set; }
    }
}
