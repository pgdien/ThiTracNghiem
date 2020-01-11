using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.Dao
{
    public class AdminDao
    {
        private ThiTracNghiemDbContext db;

        public AdminDao()
        {
            db = new ThiTracNghiemDbContext();
        }

        public IEnumerable<Admin> LissAll()
        {
            return db.Admins.ToList();
        }

        public int Insert(Admin entity)
        {
            entity.avatar = "/user.png";
            db.Admins.Add(entity);
            db.SaveChanges();
            return entity.id_admin;
        }

        public bool Update(Admin entity)
        {
            try
            {
                Admin admin = db.Admins.Find(entity.id_admin);
                admin.id_permission = entity.id_permission;
                admin.name = entity.name;
                if (!string.IsNullOrEmpty(entity.password))
                {
                    admin.password = entity.password;
                }
                admin.username = entity.username;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                Admin result = db.Admins.Find(id);
                db.Admins.Remove(result);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangePass(string password, int id_admin)
        {
            try
            {
                db.Admins.Find(id_admin).password = password;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Admin GetByUserName(string userName)
        {
            return db.Admins.SingleOrDefault((Admin x) => x.username == userName);
        }

        public Admin ViewDetail(int id_admin)
        {
            return db.Admins.Find(id_admin);
        }

        public bool IsUserNameExist(string userName)
        {
            if (db.Admins.Count((Admin x) => x.username == userName) > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsUserNameIDExist(string userName, int id)
        {
            if (db.Admins.Count((Admin x) => x.username == userName && x.id_admin != id) > 0)
            {
                return false;
            }
            return true;
        }

        public bool Login(string userName, string passWord)
        {
            if (db.Admins.Count((Admin x) => x.username == userName && x.password == passWord) > 0)
            {
                return true;
            }
            return false;
        }

        public void UpdateLastLogin(int idAdmin)
        {
            (from x in db.Admins
             where x.id_admin == idAdmin
             select x).Single().last_login = DateTime.Now;
            db.SaveChanges();
        }

        public void UpdateLastSeen(string name, string url, int idAdmin)
        {
            Admin admin = (from x in db.Admins
                           where x.id_admin == idAdmin
                           select x).Single();
            admin.last_seen = name;
            admin.last_seen_url = url;
            db.SaveChanges();
        }

        public int CountAdmin()
        {
            return db.Admins.Count();
        }

        public int CountStudent()
        {
            return db.Students.Count();
        }

        public int CountQuestion()
        {
            return db.Questions.Count();
        }

        public int CountSubject()
        {
            return db.Subjects.Count();
        }

        public int CountThread()
        {
            return (from x in db.Threads
                    where x.thread_status == true
                    select x).Count();
        }

        public int CountSpeciality()
        {
            return db.Specialities.Count();
        }

        public int CountRoom()
        {
            return (from er in db.Exam_Room
                    join Thread in db.Threads on er.id_thread equals Thread.id_thread
                    where Thread.thread_status == true
                    select er.id_room).Count();
        }

        public int CountClass()
        {
            return (from x in db.Classes
                    where x.class_status == true
                    select x).Count();
        }
    }

}
