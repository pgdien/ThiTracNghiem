using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;

namespace Model.Dao
{
    public class StudentDao
    {
        private ThiTracNghiemDbContext db;

        public StudentDao()
        {
            db = new ThiTracNghiemDbContext();
        }

        public IEnumerable<Student> LissAll()
        {
            return db.Students.ToList();
        }

        public int Insert(Student entity)
        {
            db.Students.Add(entity);
            db.SaveChanges();
            return entity.id_student;
        }

        public bool Update(Student entity)
        {
            try
            {
                Student tmp = db.Students.Find(entity.id_student);
                tmp.username = entity.username;
                tmp.id_class = entity.id_class;
                if (string.IsNullOrEmpty(entity.student_avatar))
                {
                    tmp.student_avatar = "/default.jpg";
                }
                tmp.student_birtday = entity.student_birtday;
                tmp.student_gender = entity.student_gender;
                tmp.student_name = entity.student_name;
                if (!string.IsNullOrEmpty(entity.password))
                {
                    tmp.password = entity.password;
                }
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
                Student result = db.Students.Find(id);
                db.Students.Remove(result);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Student GetByUserName(string userName)
        {
            return db.Students.SingleOrDefault((Student x) => x.username == userName);
        }

        public Student ViewDetail(int id)
        {
            return db.Students.Find(id);
        }

        public bool IsUserNameExist(string userName)
        {
            if (db.Students.Count((Student x) => x.username == userName) > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsUserNameIDExist(string userName, int id)
        {
            if (db.Students.Count((Student x) => x.username == userName && x.id_student != id) > 0)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<StudentViewModel> ListAll()
        {
            return from a in db.Classes
                   join b in db.Students on a.id_class equals b.id_class
                   orderby a.class_status
                   select new StudentViewModel
                   {
                       id_student = b.id_student,
                       class_name = a.class_name,
                       username = b.username,
                       student_name = b.student_name,
                       student_avatar = b.student_avatar,
                       student_birtday = b.student_birtday,
                       student_gender = b.student_gender
                   };
        }

        public bool IsValid(string username, string password)
        {
            if (db.Students.Count((Student x) => x.username == username && x.password == password) > 0)
            {
                return true;
            }
            return false;
        }

        public int getTesting(int id)
        {
            try
            {
                return (from Student_Thread_Info in db.Student_Thread_Info
                        where Student_Thread_Info.id_student == id && Student_Thread_Info.is_complete != (bool?)true
                        select Student_Thread_Info).Single().id_thread;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public List<int> GetStudentTestcode(int id)
        {
            List<int> score = new List<int>();
            try
            {
                score = (from x in db.Student_Thread_Info
                         where x.id_student == id && x.is_complete == (bool?)true
                         select x.id_thread).ToList();
                return score;
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
                return score;
            }
        }

        public List<TestViewModel> GetDashboard()
        {
            return (from x in db.Threads
                    join s in db.Subjects on x.id_subject equals s.id_subject
                    select new TestViewModel
                    {
                        thread = x,
                        subject = s
                    }).ToList();
        }

        public List<RoomViewModel> GetRoom(int id_thread)
        {
            return (from x in db.Threads
                    join s in db.Exam_Room on x.id_thread equals s.id_thread
                    where s.id_thread == id_thread
                    select new RoomViewModel
                    {
                        thread = x,
                        room = s
                    }).ToList();
        }

        public string GetPass(int id_room)
        {
            try
            {
                return (from Exam_Room in db.Exam_Room
                        where Exam_Room.id_room == id_room
                        select Exam_Room).Single().room_pass;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public int GeTimeToDo(int id_room)
        {
            try
            {
                int id_thread = db.Exam_Room.Find(id_room).id_thread;
                return db.Threads.Find(id_thread).time_to_do;
            }
            catch
            {
                return -1;
            }
        }

        public string GeTimeRemaning(int id_student, int id_thread)
        {
            try
            {
                return (from Student_Thread_Info in db.Student_Thread_Info
                        where Student_Thread_Info.id_student == id_student && Student_Thread_Info.id_thread == id_thread && Student_Thread_Info.is_complete != (bool?)true
                        select Student_Thread_Info.time_remaing).Single();
            }
            catch
            {
                return null;
            }
        }

        public int? GetIdExam(int id_student, int id_thread)
        {
            try
            {
                return (from Student_Thread_Info in db.Student_Thread_Info
                        where Student_Thread_Info.id_student == id_student && Student_Thread_Info.id_thread == id_thread
                        select Student_Thread_Info.id_exam).Single();
            }
            catch
            {
                return null;
            }
        }

        public List<StudentQuestViewModel> GetListQuest(int? id_exam,int id_student)
        {
            List<StudentQuestViewModel> list = new List<StudentQuestViewModel>();
            try
            {
                list = (from x in db.Student_Thread_Detail
                        join t in db.QuestionOfExams on x.id_question equals t.id_question
                        join w in db.ExamOfThreads on t.id_exam equals w.id_exam
                        join q in db.Threads on w.id_thread equals q.id_thread
                        where (int?)w.id_exam == id_exam && (int?)t.id_exam == id_exam && (int?)x.id_exam == id_exam && x.id_student==id_student 
                        select new StudentQuestViewModel
                        {
                            thread = q,
                            student_thread = x,
                            question_exam = t
                        }).ToList();
                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        public bool addThreadResult(int id_exam, int id_student, int id_question, string student_answer)
        {
            Student_Thread_Detail tmp = new Student_Thread_Detail();
            tmp.id_exam = id_exam;
            tmp.id_question = id_question;
            tmp.id_student = id_student;
            tmp.student_answer = student_answer;
            db.Student_Thread_Detail.Add(tmp);
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public List<int> getListIDQuest(int id_exam)
        {
            return (from x in db.QuestionOfExams
                    where x.id_exam == id_exam
                    select x.id_question).ToList();
        }

        public List<ResultViewModel> GetListResult(int id_student)
        {
            List<ResultViewModel> list = new List<ResultViewModel>();
            try
            {
                list = (from Thread in db.Threads
                        join Student_Thread_Info in db.Student_Thread_Info on Thread.id_thread equals Student_Thread_Info.id_thread
                        join Subject in db.Subjects on Thread.id_subject equals Subject.id_subject
                        where Student_Thread_Info.id_student == id_student && Student_Thread_Info.is_complete == (bool?)true
                        select new ResultViewModel
                        {
                            thread = Thread,
                            student_info = Student_Thread_Info,
                            subject = Subject
                        }).ToList();
                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        public bool ConnectStudentExam(int id_room, int id_student)
        {
            try
            {
                if ((from x in db.Student_Thread_Info
                     where x.id_room == id_room && x.id_student == id_student
                     select x).Count() < 1)
                {
                    IQueryable<Exam_Room> tmp = from Exam_Room in db.Exam_Room
                                                where Exam_Room.id_room == id_room
                                                select Exam_Room;
                    int id_thread = tmp.Single().id_thread;
                    int time_to_do = db.Threads.Find(id_thread).time_to_do;
                    int id_exam = (from ExamOfThread in db.ExamOfThreads
                                   where ExamOfThread.id_thread == id_thread
                                   select ExamOfThread into r
                                   orderby Guid.NewGuid()
                                   select r).Take(1).First().id_exam;
                    Student_Thread_Info temp = new Student_Thread_Info();
                    temp.id_room = id_room;
                    temp.id_student = id_student;
                    temp.id_thread = id_thread;
                    temp.start_time = DateTime.Now;
                    temp.id_exam = id_exam;
                    temp.is_complete = false;
                    temp.count_tab_focus = 0;
                    temp.time_remaing = time_to_do.ToString() + ":00";
                    db.Student_Thread_Info.Add(temp);
                    if (db.SaveChanges() > 0)
                    {
                        bool flag = true;
                        foreach (int item in getListIDQuest(id_exam))
                        {
                            if (!addThreadResult(id_exam, id_student, item, null))
                            {
                                flag = false;
                            }
                        }
                        if (flag)
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void UpdateStatus(int id_room, int id_student, string time_remaining)
        {
            IQueryable<Student_Thread_Info> update = from x in db.Student_Thread_Info
                                                     where x.id_student == id_student && x.id_room == id_room
                                                     select x;
            if (update.Count() <= 1)
            {
                update.SingleOrDefault().time_remaing = time_remaining;
            }
            db.SaveChanges();
        }

        public void InsertScore(double score, int id_student, int id_thread)
        {
            int? id_exam = GetIdExam(id_student, id_thread);
            Student_Thread_Info student_Thread_Info = (from x in db.Student_Thread_Info
                                                       where x.id_student == id_student && x.id_thread == id_thread && x.id_exam == id_exam
                                                       select x).ToList().SingleOrDefault();
            student_Thread_Info.score_number = (decimal)score;
            student_Thread_Info.end_time = DateTime.Now;
            student_Thread_Info.is_complete = true;
            db.SaveChanges();
        }

        public Student_Thread_Info GetScore(int id_student, int id_thread)
        {
            return (from x in db.Student_Thread_Info
                    where x.id_student == id_student && x.id_thread == id_thread
                    select x).ToList().SingleOrDefault();
        }

        public void UpdateStudentTest(int id_question, string answer, int id_thread, int id_student)
        {
            int? id_exam = GetIdExam(id_student, id_thread);
            (from x in db.Student_Thread_Detail
             where x.id_student == id_student && (int?)x.id_exam == id_exam && x.id_question == id_question
             select x).ToList().SingleOrDefault().student_answer = answer;
            db.SaveChanges();
        }

        public void UpdateTiming(string time, int id_student, int id_thread)
        {
            int? id_exam = GetIdExam(id_student, id_thread);
            Student_Thread_Info Update = (from x in db.Student_Thread_Info
                                          where x.id_student == id_student && x.id_thread == id_thread && x.id_exam == id_exam
                                          select x).ToList().SingleOrDefault();
            if (Update != null)
            {
                Update.time_remaing = time;
            }
            db.SaveChanges();
        }

        public void UpdateFocusTab(int focus, int id_student, int id_thread)
        {
            int? id_exam = GetIdExam(id_student, id_thread);
            Student_Thread_Info Update = (from x in db.Student_Thread_Info
                                          where x.id_student == id_student && x.id_thread == id_thread && x.id_exam == id_exam
                                          select x).ToList().SingleOrDefault();
            if (Update != null)
            {
                Update.count_tab_focus += focus;
            }
            db.SaveChanges();
        }

        public void UpdateLastLogin(int id, string ip, string info_browser)
        {
            Student student = (from x in db.Students
                               where x.id_student == id
                               select x).Single();
            student.last_login = DateTime.Now;
            student.ip = ip;
            student.info_browser = info_browser;
            db.SaveChanges();
        }

        public void UpdateLastSeen(string name, string url, int id)
        {
            Student student = (from x in db.Students
                               where x.id_student == id
                               select x).Single();
            student.last_seen = name;
            student.last_seen_url = url;
            db.SaveChanges();
        }
    }

}
