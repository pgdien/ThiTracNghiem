using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.Dao
{
    public class SpecialityDao
    {
        ThiTracNghiemDbContext db = null;
        public SpecialityDao()
        {
            db = new ThiTracNghiemDbContext();
        }

        public List<Speciality> ListAll()
        {

            return db.Specialities.ToList();
        }
        public int Insert(Speciality entity)
        {
            try
            {
                db.Specialities.Add(entity);
                db.SaveChanges();
                return entity.id_speciality;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public bool Update(Speciality entity)
        {
            try
            {
                var tmp = db.Specialities.Find(entity.id_speciality);
                tmp.speciality_name = entity.speciality_name;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Speciality ViewDetail(int id)
        {
            var tmp = db.Specialities.Find(id);
            return tmp;
        }
        public bool Delete(int id)
        {
            try
            {
                var result = db.Specialities.Find(id);
                db.Specialities.Remove(result);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
