using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;

namespace Model.Dao
{
    public class SubjectDao
    {

        ThiTracNghiemDbContext db = null;
        public SubjectDao()
        {
            db = new ThiTracNghiemDbContext();
        }
        public IEnumerable<SubjectViewModel> LissAll()
        {
            var list = from a in db.Subjects
                       join b in db.Thematics on a.id_subject equals b.id_subject
                       group new { a, b } by new
                       {
                           a.id_subject
                       } into g
                       select new SubjectViewModel
                       {
                           id_subject = g.Key.id_subject,
                           subject_name = g.Max(p => p.a.subject_name),
                           count_thematic = g.Count(p => p.b.id_thematic != null)


                       };
            return list;
        }
        public List<Subject> ListAll()
        {

            return db.Subjects.ToList();
        }
        public bool Delete(int id)
        {
            try
            {
                var result = db.Subjects.Find(id);
                db.Subjects.Remove(result);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int Insert(Subject entity)
        {
          
            db.Subjects.Add(entity);
            db.SaveChanges();
            Thematic tmp = new Thematic();
            tmp.id_subject = entity.id_subject;
            tmp.thematic_name = "Chuyên đề cơ bản";
            db.Thematics.Add(tmp);
            db.SaveChanges();
            return entity.id_subject;
        }
        public bool Update(Subject entity)
        {
            try
            {
                var tmp = db.Subjects.Find(entity.id_subject);
                tmp.subject_name = entity.subject_name;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Subject ViewDetail(int id_subject)
        {
            var tmp = db.Subjects.Find(id_subject);
            return tmp;
        }
    }
}
