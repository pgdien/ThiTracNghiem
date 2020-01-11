using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;

namespace Model.Dao
{
    public class ExamDao
    {

        ThiTracNghiemDbContext db = null;
        public ExamDao()
        {
            db = new ThiTracNghiemDbContext();
        }
        //public IEnumerable<ThreadViewModel> LissAll()
        //{
        //    var list = from Thread in db.Threads
        //               join Exam_Room in db.Exam_Room on new { Id_thread = Thread.id_thread } equals new { Id_thread = Convert.ToString(Exam_Room.id_thread) }
        //               join ExamOfThread in db.ExamOfThreads on Thread.id_thread equals ExamOfThread.id_thread
        //               join Subject in db.Subjects on Thread.id_subject equals Subject.id_subject
        //               group new { Thread, Subject, Exam_Room, ExamOfThread } by new
        //               {
        //                   Thread.id_thread,
        //                   Subject.subject_name,
        //                   Thread.time_to_do,
        //                   Thread.thread_name,
        //                   Thread.max_question,
        //                   Thread.thread_status
        //               } into g
        //               select new ThreadViewModel
        //               {
        //                   id_thread = g.Key.id_thread,
        //                   thread_name = g.Key.thread_name,
        //                   subject_name = g.Key.subject_name,
        //                   time_to_do = g.Key.time_to_do,
        //                   thread_status = g.Key.thread_status,
        //                   max_question = g.Key.max_question,
        //                   count_room = g.Count(p => p.Exam_Room.id_room != null),
        //                   count_exam = g.Count(p => p.ExamOfThread.id_exam != null)
        //               };

        //    return list;
        //}

        //public List<Thread> ListAll()
        //{

        //    return db.Threads.ToList();
        //}

        //public bool Delete(int id)
        //{
        //    try
        //    {
        //        var result = db.Threads.Find(id);
        //        db.Threads.Remove(result);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        public int Insert(int id_thread)
        {


            ExamOfThread tmp = new ExamOfThread();
            tmp.id_thread = id_thread;
           
            db.ExamOfThreads.Add(tmp);
            db.SaveChanges();
            return tmp.id_exam;
        } 

        //public Thread ViewDetail(int id_thread)
        //{
        //    var tmp = db.Threads.Find(id_thread);
        //    return tmp;
        //}
    }
}
