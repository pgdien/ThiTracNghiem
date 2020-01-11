using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using DoAn_ThiTracNghiem_Complete.Common;
using Model.Dao;
using Model.EF;

namespace DoAn_ThiTracNghiem_Complete.Areas.Student.Controllers
{
    public class HomeController : BaseController
    {
        Model.EF.Student student = new Model.EF.Student();
        StudentDao studentDao = new StudentDao();
        // GET: Student/Home
        public ActionResult Index()
        {
            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            var id_student = session.id_student;
            if (studentDao.getTesting(id_student) > 0)
                return RedirectToAction("DoingTest");

            studentDao.UpdateLastLogin(id_student, GetIPAddress(), GetUserEnvironment());
            studentDao.UpdateLastSeen("Trang chủ", Url.Action("Index"), id_student);
            ViewBag.score = studentDao.GetStudentTestcode(id_student);
            ViewBag.studenname = studentDao.ViewDetail(id_student).student_name;
            return View(studentDao.GetDashboard());
        }

        [HttpGet]
        public ActionResult SelectRoom(int id)
        {
            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            var id_student = session.id_student;
            if (studentDao.getTesting(id_student) > 0)
                return RedirectToAction("DoingTest");

            studentDao.UpdateLastLogin(id_student, GetIPAddress(), GetUserEnvironment());
            var model = studentDao.GetRoom(id);
            ViewBag.studenname = studentDao.ViewDetail(id_student).student_name;
            studentDao.UpdateLastSeen("Chọn phòng thi cho kỳ thi" + model.FirstOrDefault().thread.thread_name, "/Student/Home/SelectRoom" + id, id_student);
            return View(model);
        }

        [HttpPost]
        public ActionResult CheckPassword(FormCollection form)
        {
            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            var id_student = session.id_student;
            if (studentDao.getTesting(id_student) > 0)
                return RedirectToAction("DoingTest");
            string code = form["id_room"];
            int id_room = Convert.ToInt32(code);
            string password = form["password"];
            string room_password = studentDao.GetPass(id_room);
            if (!password.Equals(room_password))
            {
                TempData["status_id"] = false;
                TempData["status"] = "Mật khẩu không đúng!";
                return RedirectToAction("Index");
            }
            else
            {
                studentDao.ConnectStudentExam(id_room, id_student);
                studentDao.UpdateStatus(id_room, id_student, studentDao.GeTimeToDo(id_room) + ":00");
                return RedirectToAction("DoingTest");
            }
        }
        public ActionResult DoingTest()
        {
            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            var id_student = session.id_student;
            if (studentDao.getTesting(id_student) < 0)
                return View("Error");
            var id_thread = studentDao.getTesting(id_student);
            var time_remaning = studentDao.GeTimeRemaning(id_student, id_thread);
            if (time_remaning != null)
            {
                string[] time = Regex.Split(time_remaning, ":");
                ViewBag.min = time[0];
                ViewBag.sec = time[1];
            }

            var id_exam = studentDao.GetIdExam(id_student, id_thread);

            var model = studentDao.GetListQuest(id_exam,id_student);
            if (model.Count > 0)
            {
                ViewBag.idStudent = id_student;
                ViewBag.idExam = id_exam;
                studentDao.UpdateLastLogin(id_student, GetIPAddress(), GetUserEnvironment());
                studentDao.UpdateLastSeen("Đang làm bài " + model.FirstOrDefault().thread.thread_name, Url.Action("DoingTest"), id_student);
                ViewBag.studenname = studentDao.ViewDetail(id_student).student_name;
                return View(model);
            }
            else
            {
                TempData["status_id"] = false;
                TempData["status"] = "Không tìm thấy câu hỏi !!";
                return View("DoingTest");
            }
        }
        public ActionResult SubmitTest()
        {

            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            var id_student = session.id_student;
            if (studentDao.getTesting(id_student) < 0)
                return View("Error");
            var id_thread = studentDao.getTesting(id_student);
            var list = studentDao.GetListQuest(studentDao.GetIdExam(id_student, id_thread),id_student);
            int total_quest = list.FirstOrDefault().thread.max_question;
            double coefficient = 10.0 / (double)total_quest;
            int count_correct = 0;
            foreach (var item in list)
            {
                if (item.student_thread.student_answer != null && item.student_thread.student_answer.Trim().Equals(item.question_exam.correct_answer.Trim()))
                    count_correct++;
            }
            double score = Math.Round(coefficient * count_correct, 2);
            string detail = count_correct + "/" + total_quest;
            studentDao.InsertScore(score, id_student, id_thread);
            return RedirectToAction("TestResult/" + id_thread);
        }
        public ActionResult TestResult(int id)
        {

            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            var id_student = session.id_student;
            if (studentDao.getTesting(id_student) > 0)
                return View("DoingTest");
            if (studentDao.GetStudentTestcode(id_student).IndexOf(id) == -1)
                return View("Error");
            ViewBag.score = studentDao.GetScore(id_student, id);
            ViewBag.studenname = studentDao.ViewDetail(id_student).student_name;
            var model = studentDao.GetListQuest(studentDao.GetIdExam(id_student, id),id_student);
            studentDao.UpdateLastLogin(id_student, GetIPAddress(), GetUserEnvironment());
            studentDao.UpdateLastSeen("Đang xem kết quả của " + model.FirstOrDefault().thread.thread_name, "/Student/Home/TestResult/" + id, id_student);
            return View(model);

        }
        [HttpPost]
        public void UpdateTiming(FormCollection form)
        {
            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            var id_student = session.id_student;
            var id_thread = studentDao.getTesting(id_student);
            string min = form["min"];
            string sec = form["sec"];
            string time = min + ":" + sec;
            studentDao.UpdateTiming(time, id_student, id_thread);
        }
        [HttpPost]
        public void UpdateStudentTest(FormCollection form)
        {
            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            var id_student = session.id_student;
            var id_thread = studentDao.getTesting(id_student);
            int id_quest = Convert.ToInt32(form["id"]);
            string answer = form["answer"];
            answer = answer.Trim();
            string time = form["min"] + ":" + form["sec"];
            studentDao.UpdateStudentTest(id_quest, answer, id_thread, id_student);
            studentDao.UpdateTiming(time, id_student, id_thread);
        }

        public void UpdateFocusTab(FormCollection form)
        {

            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            var id_student = session.id_student;
            var id_thread = studentDao.getTesting(id_student);
            studentDao.UpdateLastLogin(id_student, GetIPAddress(), GetUserEnvironment());
            int focus = Convert.ToInt32(form["focustab"]);
            if (focus < 0) focus = 0;
            studentDao.UpdateFocusTab(focus, id_student, id_thread);
        }
        public ActionResult ListResult()
        {

            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            var id_student = session.id_student;
            if (studentDao.getTesting(id_student) > 0)
                return View("DoingTest");
            var model = studentDao.GetListResult(id_student);
            studentDao.UpdateLastLogin(id_student, GetIPAddress(), GetUserEnvironment());
            ViewBag.studenname = studentDao.ViewDetail(id_student).student_name;
            studentDao.UpdateLastSeen("Đang xem danh sách kết quả của các kỳ thi ", Url.Action("ListResult"), id_student);
            return View(model);

        }

        public ActionResult Logout()
        {

            Session[Common.CommonConstants.STUDENT_SESSION] = null;
            return RedirectToAction("Index", "StudentLogin");
        }

        //lấy ip và info client
        public String GetIPAddress()
        {
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
                ip = Request.ServerVariables["REMOTE_ADDR"];
            else
                ip = ip.Split(',')[0];

            return ip;
        }
        public String GetUserEnvironment()
        {
            var browser = Request.Browser;
            var platform = GetUserPlatform();
            return string.Format("{0} {1} / {2}", browser.Browser, browser.Version, platform);
        }
        public String GetUserPlatform()
        {
            var ua = Request.UserAgent;

            if (ua.Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                return "Black Berry";

            if (ua.Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                return "Mac OS";

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua.Contains("Windows NT 10"))
                return "Windows 10";

            //fallback to basic platform:
            return Request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");
        }
        public String GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }
    }
}