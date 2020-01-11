namespace Model.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ThiTracNghiemDbContext : DbContext
    {
        public ThiTracNghiemDbContext()
            : base("name=ThiTracNghiemDbContext")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Exam_Room> Exam_Room { get; set; }
        public virtual DbSet<ExamOfThread> ExamOfThreads { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Speciality> Specialities { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Thematic> Thematics { get; set; }
        public virtual DbSet<Thread> Threads { get; set; }
        public virtual DbSet<QuestionOfExam> QuestionOfExams { get; set; }
        public virtual DbSet<Student_Thread_Detail> Student_Thread_Detail { get; set; }
        public virtual DbSet<Student_Thread_Info> Student_Thread_Info { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.avatar)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.last_seen_url)
                .IsUnicode(false);

            modelBuilder.Entity<Exam_Room>()
                .Property(e => e.room_pass)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.img)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.student_avatar)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.last_seen_url)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.ip)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionOfExam>()
                .Property(e => e.img)
                .IsUnicode(false);

            modelBuilder.Entity<Student_Thread_Info>()
                .Property(e => e.score_number)
                .HasPrecision(4, 2);

            modelBuilder.Entity<Student_Thread_Info>()
                .Property(e => e.time_remaing)
                .IsUnicode(false);
        }
    }
}
