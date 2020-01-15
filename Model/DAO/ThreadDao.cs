using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;

namespace Model.Dao
{
    public class ThreadDao
    {
        private ThiTracNghiemDbContext db;

        public ThreadDao()
        {
            db = new ThiTracNghiemDbContext();
        }

        public IEnumerable<ThreadViewModel> LissAll()
        {
            return from Thread in db.Threads
                   join Exam_Room in db.Exam_Room on new
                   {
                       Thread.id_thread
                   } equals new
                   {
                       Exam_Room.id_thread
                   }
                   join ExamOfThread in db.ExamOfThreads on Thread.id_thread equals ExamOfThread.id_thread
                   join Subject in db.Subjects on Thread.id_subject equals Subject.id_subject
                   group new
                   {
                       Thread,
                       Subject,
                       Exam_Room,
                       ExamOfThread
                   } by new
                   {
                       Thread.id_thread,
                       Subject.subject_name,
                       Thread.time_to_do,
                       Thread.thread_name,
                       Thread.max_question,
                       Thread.thread_status
                   } into g
                   select new ThreadViewModel
                   {
                       id_thread = g.Key.id_thread,
                       thread_name = g.Key.thread_name,
                       subject_name = g.Key.subject_name,
                       time_to_do = g.Key.time_to_do,
                       thread_status = g.Key.thread_status,
                       max_question = g.Key.max_question,
                       count_room = g.Count(p => (int?)p.Exam_Room.id_room != null),
                       count_exam = g.Count(p => (int?)p.ExamOfThread.id_exam != null)
                   };
        }

        public List<StudentThreadInfoViewModel> GetThreadResult(int id_thread)
        {
            return (from Student in db.Students
                    join Student_Thread_Info in db.Student_Thread_Info on Student.id_student equals Student_Thread_Info.id_student
                    where Student_Thread_Info.id_thread == id_thread
                    select new StudentThreadInfoViewModel
                    {
                        student = Student,
                        student_thread = Student_Thread_Info
                    }).ToList();
        }

        public List<StudentThreadInfoViewModel> GetStudentRoomData(int id_room)
        {
            return (from Student in db.Students
                    join Student_Thread_Info in db.Student_Thread_Info on Student.id_student equals Student_Thread_Info.id_student
                    where Student_Thread_Info.id_room == id_room
                    select new StudentThreadInfoViewModel
                    {
                        student = Student,
                        student_thread = Student_Thread_Info
                    }).ToList();
        }

        public List<StudentThreadViewModel> GetThreadResult()
        {
            return (from student in db.Students
                    join sti in db.Student_Thread_Info on student.id_student equals sti.id_student
                    join thread in db.Threads on sti.id_thread equals thread.id_thread
                    join subject in db.Subjects on thread.id_subject equals subject.id_subject
                    where thread.thread_status == false && sti.is_complete == (bool?)true
                    orderby thread.id_thread
                    select new StudentThreadViewModel
                    {
                        student = student,
                        student_thread = sti,
                        thread = thread,
                        subject = subject
                    }).ToList();
        }

        public List<int> GetListRoomOpen()
        {
            return (from Thread in db.Threads
                    join Exam_Room in db.Exam_Room on Thread.id_thread equals Exam_Room.id_thread
                    where Thread.thread_status == true
                    select Exam_Room.id_room).ToList();
        }

        public IEnumerable<Exam_RoomViewModel> LissAllRoom(int id)
        {
            return from Exam_Room in db.Exam_Room
                   join Thread in db.Threads on new
                   {
                       Exam_Room.id_thread
                   } equals new
                   {
                       Thread.id_thread
                   }
                   where Exam_Room.id_thread == id
                   select new Exam_RoomViewModel
                   {
                       id_room = Exam_Room.id_room,
                       room_name = Exam_Room.room_name,
                       room_pass = Exam_Room.room_pass,
                       thread_name = Thread.thread_name
                   };
        }

        public IEnumerable<Exam_RoomViewModel> LissRoom(int id_room)
        {
            return from Exam_Room in db.Exam_Room
                   join Thread in db.Threads on new
                   {
                       Exam_Room.id_thread
                   } equals new
                   {
                       Thread.id_thread
                   }
                   where Exam_Room.id_room == id_room
                   select new Exam_RoomViewModel
                   {
                       id_room = Exam_Room.id_room,
                       room_name = Exam_Room.room_name,
                       room_pass = Exam_Room.room_pass,
                       thread_name = Thread.thread_name,
                       timetodo = Thread.time_to_do.ToString()
                   };
        }

        public List<Thread> ListAll()
        {
            return db.Threads.ToList();
        }

        public List<RoomViewModel> GetOpenRoom()
        {
            return (from x in db.Threads
                    join s in db.Exam_Room on x.id_thread equals s.id_thread
                    where x.thread_status == true
                    select new RoomViewModel
                    {
                        thread = x,
                        room = s
                    }).ToList();
        }

        public bool isExitsStudentThread(int id)
        {
            if (db.Student_Thread_Info.SingleOrDefault((Student_Thread_Info x) => x.id_thread == id) != null)
            {
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            try
            {
                Thread result = db.Threads.Find(id);
                db.Threads.Remove(result);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SDelete(int id_exam, int id_student)
        {
            try
            {
                Student_Thread_Info result = (from x in db.Student_Thread_Info
                                              where x.id_exam == (int?)id_exam && x.id_student == id_student
                                              select x).SingleOrDefault();
                db.Student_Thread_Info.Remove(result);
                db.Student_Thread_Detail.RemoveRange(from x in db.Student_Thread_Detail
                                                     where x.id_exam == id_exam && x.id_student == id_student
                                                     select x);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Insert(Thread entity)
        {
            Thread tmp2 = new Thread();
            tmp2.id_subject = entity.id_subject;
            tmp2.max_question = entity.max_question;
            tmp2.thread_name = entity.thread_name;
            tmp2.thread_status = entity.thread_status;
            tmp2.time_to_do = entity.time_to_do;
            tmp2.essay_score = entity.essay_score;
            db.Threads.Add(tmp2);
            db.SaveChanges();
            Exam_Room tmp = new Exam_Room();
            tmp.id_room = 2;
            tmp.id_thread = tmp2.id_thread;
            tmp.room_name = "MẶC ĐỊNH";
            tmp.room_pass = "dhhl";
            db.Exam_Room.Add(tmp);
            db.SaveChanges();
            return tmp2.id_thread;
        }

        public bool Update(Thread entity)
        {
            try
            {
                Thread thread = db.Threads.Find(entity.id_thread);
                thread.thread_name = entity.thread_name;
                thread.thread_status = entity.thread_status;
                thread.time_to_do = entity.time_to_do;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Thread ViewDetail(int id_thread)
        {
            return db.Threads.Find(id_thread);
        }

        public int InsertRoom(Exam_Room entity)
        {
            db.Exam_Room.Add(entity);
            db.SaveChanges();
            return entity.id_room;
        }

        public bool DeleteRoom(int id)
        {
            try
            {
                Exam_Room result = db.Exam_Room.Find(id);
                db.Exam_Room.Remove(result);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<int> ListAllExam(int id_thread)
        {
            return (from ExamOfThread in db.ExamOfThreads
                    where ExamOfThread.id_thread == id_thread
                    select ExamOfThread.id_exam).ToList();
        }

        public List<Student> ListStudenCompleteThread(int id_thread)
        {
            return (from Student_Thread_Info in db.Student_Thread_Info
                    join Student in db.Students on Student_Thread_Info.id_student equals Student.id_student
                    where Student_Thread_Info.id_thread == id_thread
                    select Student).ToList();
        }

        public List<QuestionViewModel> ListAllQuestion(int id_exam)
        {
            return (from QuestionOfExam in db.QuestionOfExams
                    join Question in db.Questions on QuestionOfExam.id_question equals Question.id_question
                    join Thematic in db.Thematics on Question.id_thematic equals Thematic.id_thematic
                    where QuestionOfExam.id_exam == id_exam
                    select new QuestionViewModel
                    {
                        id_question = Question.id_question,
                        question_content = QuestionOfExam.question_content,
                        A = QuestionOfExam.A,
                        B = QuestionOfExam.B,
                        C = QuestionOfExam.C,
                        D = QuestionOfExam.D,
                        correct_answer = QuestionOfExam.correct_answer,
                        thematic_name = Thematic.thematic_name
                    }).ToList();
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
                return 0;
            }
        }

        public List<StudentQuestViewModel> ListQuestionResult(int id_student, int id_thread)
        {
            List<StudentQuestViewModel> list = new List<StudentQuestViewModel>();
            try
            {
                int? id_exam = GetIdExam(id_student, id_thread);
                list = (from x in db.Student_Thread_Detail
                        join t in db.QuestionOfExams on x.id_question equals t.id_question
                        join w in db.ExamOfThreads on t.id_exam equals w.id_exam
                        join q in db.Threads on w.id_thread equals q.id_thread
                        where (int?)w.id_exam == id_exam && (int?)t.id_exam == id_exam && (int?)x.id_exam == id_exam
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
    }

}
