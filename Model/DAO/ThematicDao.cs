using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;

namespace Model.Dao
{
    public class ThematicDao
    {

        ThiTracNghiemDbContext db = null;
        public ThematicDao()
        {
            db = new ThiTracNghiemDbContext();
        }

        public List<ThematicModel> GetThematic(int id_subject)
        {
            var tests = from Thematic in db.Thematics
                        join Question in db.Questions on Thematic.id_thematic equals Question.id_thematic into Question_join
                        from Question in Question_join.DefaultIfEmpty()
                        where
                          Thematic.id_subject == id_subject
                        group new { Thematic, Question } by new
                        {
                            Thematic.id_thematic,
                            Thematic.thematic_name
                        } into g
                        select new ThematicModel
                        {
                            id_thematic = g.Key.id_thematic,
                            thematic_name= g.Key.thematic_name,
                            total = g.Count(p => p.Question.id_question != null)
                        };
            List<ThematicModel> a = tests.ToList();
            return a;
        }
        public List<Thematic> ListThematicInSubject(int id)
        {

            return db.Thematics.Where((x => x.id_subject == id)).ToList();
        }
        public bool Delete(int id)
        {
            try
            {
                var result = db.Thematics.Find(id);
                db.Thematics.Remove(result);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int Insert(Thematic entity)
        {
            db.Thematics.Add(entity);
            db.SaveChanges();
            return entity.id_thematic;
        }
        public bool Update(Thematic entity)
        {
            try
            {
                var tmnp = db.Thematics.SingleOrDefault(x => x.id_thematic == entity.id_thematic);
                tmnp.thematic_name = entity.thematic_name;

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Thematic ViewDetail(int id)
        {
            return db.Thematics.SingleOrDefault(x => x.id_thematic == id);

        }
    }
}
