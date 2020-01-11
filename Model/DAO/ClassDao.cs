using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;
namespace Model.Dao
{
    public class ClassDao
    {
        ThiTracNghiemDbContext db = null;
        public ClassDao()
        {
            db = new ThiTracNghiemDbContext();
        }

        public IEnumerable<ClassViewModel> LissAll()
        {
            var list = from a in db.Classes
                       join b in db.Specialities on a.id_speciality equals b.id_speciality
                       orderby a.class_status ascending
                       select new ClassViewModel
                       {
                           id_class = a.id_class,
                           class_name = a.class_name,
                           speciality_name = b.speciality_name,
                           class_status = a.class_status
                       };
            return list;
        }
        public List<Class> ListAll()
        {

            return db.Classes.ToList();
        }
        public bool Delete(int id)
        {
            try
            {
                var result = db.Classes.Find(id);
                db.Classes.Remove(result);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int Insert(Class entity)
        {
            db.Classes.Add(entity);
            db.SaveChanges();
            return entity.id_class;
        }
        public bool Update(Class entity)
        {
            try
            {
                var lop = db.Classes.Find(entity.id_class);
                lop.class_name = entity.class_name;
                lop.class_status = entity.class_status;
                lop.id_speciality = entity.id_speciality;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Class ViewDetail(int id_class)
        {
            var lop = db.Classes.Find(id_class);
            return lop;
        }

    }
}
