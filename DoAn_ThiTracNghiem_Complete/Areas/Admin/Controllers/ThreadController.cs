using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_ThiTracNghiem_Complete.Common;
using Model.Dao;
using Model.EF;
using Model.ViewModel;
using DoAn_ThiTracNghiem_Complete.Areas.Admin.Code;
using System.IO;
using DoAn_ThiTracNghiem_Complete.Areas.Admin.Models;

namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Controllers
{
    public class ThreadController : BaseController
    {
        // GET: Admin/Student
        public ActionResult Index()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return View("Error");
            ViewBag.AdminName = session.name;


            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Quản lý kỳ thi", "Admin/Thread/", session.id_admin);
            var dao1 = new ThreadDao();
            var model = dao1.LissAll();
            return View(model);

        }

        [HttpGet]
        public ActionResult Create()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return View("Error");
            ViewBag.AdminName = session.name;
            SetViewBag();
            var dao1 = new SubjectDao();
            ViewBag.ListSubject = dao1.ListAll();
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Tạo kì thi", "/Admin/Thread/Create", session.id_admin);
            return View();
        }
        public JsonResult GetJsonUnits(int id)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return Json("", JsonRequestBehavior.AllowGet);
            ViewBag.AdminName = session.name;
            var dao = new ThematicDao();
            return Json(dao.GetThematic(id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Thread entity, FormCollection collection)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return View("Error");
            ViewBag.AdminName = session.name;
            if (ModelState.IsValid)
            {

                var dao = new ThreadDao();
                var dao1 = new ThematicDao();
                var dao2 = new QuestionDao();
                var dao3 = new ExamDao();
                int id_subject = Convert.ToInt32(collection["id_subject"]);
                int total_exam = Convert.ToInt32(collection["total_exam"]);

                int id_thread = dao.Insert(entity);
                if (id_thread > 0)
                {
                 
                    //tạo bộ câu hỏi cho đề thi
                    List<ThematicModel> list_thematic = dao1.GetThematic(id_subject);
                    List<int> idExamOfThread = new List<int>();
                    for (int i = 1; i <= total_exam; i++)
                    {
                        int id_exam = dao3.Insert(id_thread);
                        idExamOfThread.Add(id_exam);

                    }
                    foreach (ThematicModel thematic in list_thematic)
                    {
                        int quest_of_thematic = Convert.ToInt32(collection["unit-" + thematic.id_thematic]);
                        List<Question> list_question = dao2.GetQuestionsByThematic(thematic.id_thematic, quest_of_thematic);
                        foreach (Question item in list_question)
                        {

                            foreach (int id_exam in idExamOfThread)
                            {
                                dao2.AddQuestionsToExam(id_exam, item.id_question);
                            }

                        }
                    }
                    //để thông báo thêm thành công
                    SetNotice("Hệ thống đã thêm thành công.", "success");
                    return RedirectToAction("Create");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm kì thi không thành công.");
                }

            }
            var dao4 = new SubjectDao();
            ViewBag.ListSubject = dao4.ListAll();
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return View("Error");
            ViewBag.AdminName = session.name;

            var lop = new ThreadDao().ViewDetail(id);
            if (lop == null) return View("Error");
            else
            {
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Sửa thông tin của kì thi " + lop.thread_name, "/Admin/Thread/Edit" + lop.id_thread, session.id_admin);
                TempData["thread_name"] = lop.thread_name;
                TempData["id_thread"] = lop.id_thread;
                return View(lop);
            }
        }
        [HttpPost]
        public ActionResult Edit(Thread collection)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return View("Error");
            ViewBag.AdminName = session.name;

            var dao = new ThreadDao();

            var id = dao.Update(collection);
            if (id)
            {
                SetNotice("Hệ thống đã sửa thành công kì thi " + collection.thread_name + ".", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetNotice("Có lỗi xảy ra!!", "danger");
                ModelState.AddModelError("", "Cập nhật thông tin kì thi không thành công.");
            }
            TempData["thread_name"] = collection.thread_name;
            TempData["id_thread"] = collection.id_thread;
            return View();

        }

        public ActionResult Delete(int id)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return View("Error");
            ViewBag.AdminName = session.name;
            var result = new ThreadDao().Delete(id);
            if (result == true)
            {
                // o lai trang cũ
            }
            return RedirectToAction("Index");
        }
        public ActionResult RIndex(int id)
        {
            var lop = new ThreadDao().ViewDetail(id);
            if (lop == null) return View("Error");
            else
            {
                var dao = new AdminDao();
                var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
                if (session.id_permission == 0)
                    return View("Error");
                ViewBag.AdminName = session.name;

                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Xem danh sách phòng thi của kì thi " + lop.thread_name, "Admin/Thread/RIndex" + id, session.id_admin);
                var model = new ThreadDao().LissAllRoom(id);
                TempData["thread_name"] = lop.thread_name;
                TempData["id_thread"] = lop.id_thread;
                return View(model);
            }

        }

        [HttpGet]
        public ActionResult RCreate(int id)
        {
            var dao = new AdminDao();
            var dao2 = new ThreadDao().ViewDetail(id);
            if (dao2 == null) return View("Error");
            else
            {
                var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
                if (session.id_permission == 0)
                    return View("Error");
                ViewBag.AdminName = session.name;

                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Thêm phòng thi cho kì thi " + dao2.thread_name, "Admin/Thread/RCreate/" + id, session.id_admin);
                TempData["thread_name"] = dao2.thread_name;
                TempData["id_thread"] = id;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RCreate(Exam_Room collection)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return View("Error");
            ViewBag.AdminName = session.name;
            if (ModelState.IsValid)
            {

                var dao = new ThreadDao();

                var dao2 = new ThreadDao().ViewDetail(collection.id_thread);
                TempData["thread_name"] = dao2.thread_name;
                TempData["id_thread"] = collection.id_thread;
                int id = dao.InsertRoom(collection);
                if (id > 0)
                {
                    //để thông báo thêm thành công
                    SetNotice("Hệ thống đã thêm thành công.", "success");
                    return RedirectToAction("RIndex", new { id = collection.id_thread });
                }
                else
                {

                    ModelState.AddModelError("", "Thêm phòng thi không thành công.");
                }

            }

            return View();
        }
        
        public ActionResult RDelete(int id)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return View("Error");
            ViewBag.AdminName = session.name;
            var result = new ThreadDao().DeleteRoom(id);
            if (result == true)
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult QDetail(int id)
        {


            var lop = new ThreadDao().ViewDetail(id);
            if (lop == null) return View("Error");
            else
            {
                var dao = new AdminDao();
                var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
                if (session.id_permission == 0)
                    return View("Error");
                ViewBag.AdminName = session.name;
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Xem danh sách câu hỏi của kì thi " + lop.thread_name, "Admin/Thread/RIndex" + id, session.id_admin);
                var tmp = new ThreadDao();
                int x = tmp.ListAllExam(id).FirstOrDefault();
                var model = tmp.ListAllQuestion(x);
                TempData["thread_name"] = lop.thread_name;
                TempData["id_thread"] = lop.id_thread;
                return View(model);
            }

        }
        [HttpGet]
        public ActionResult Export(int id)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return View("Error");
            ViewBag.AdminName = session.name;
            var tmp = new ImportExport();
            // Gọi lại hàm để tạo file excel
            var stream = tmp.CreateExcelFile(id);
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", "attachment; filename=DeThi.xlsx");
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
            // Redirect về luôn trang index :D
            return RedirectToAction("Index");
        }

        public ActionResult OpenRoom()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.AdminName = session.name;
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Giám sát kì thi", "Admin/Thread/OpenT", session.id_admin);

            var dao1 = new ThreadDao();
            var model = dao1.GetOpenRoom();
            return View(model);

        }
        public ActionResult Room(int id)
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.AdminName = session.name;
            var dao1 = new ThreadDao();
            if (dao1.GetListRoomOpen().IndexOf(id) == -1)
                return View("Error");
            var model = dao1.GetStudentRoomData(id);
            var tmp = dao1.LissRoom(id).SingleOrDefault();
            ViewBag.RoomName = tmp.room_name;
            ViewBag.ThreadName = tmp.thread_name;
            ViewBag.TimeToDo = tmp.timetodo;
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Giám thị phòng thi " + tmp.room_name, "Admin/Thread/Room/" + id, session.id_admin);
            ViewBag.Id = id;
            return View(model);

        }
        public JsonResult GetJsonData(int id)
        {
            var dao1 = new ThreadDao();
            var tmp = dao1.GetStudentRoomData(id);
            List<JsonRoomDataModel> data = new List<JsonRoomDataModel>();
            foreach (StudentThreadInfoViewModel item in tmp)
            {
                TimeSpan? lastOnline = DateTime.Now - item.student.last_login;
                TimeSpan time = lastOnline ?? default(TimeSpan);
                var a = "";
                if (time.TotalSeconds < 10)
                {

                    a = "<span class='label label-success'>" + "Đang online" + "</span>";
                }
                else
                {
                    if (time.TotalSeconds < 60)
                    {
                        a = " <span class='label label-default'>" + "Đã offline " + Math.Round(time.TotalSeconds, 0) + " giây</span>";

                    }
                    else if (time.TotalMinutes < 60)
                    {
                        a = " <span class='label label-default'>" + "Đã offline " + Math.Round(time.TotalMinutes, 0) + " phút</span>";

                    }
                    else if (time.TotalHours < 24)
                    {
                        a = " <span class='label label-default'>" + "Đã offline " + Math.Round(time.TotalHours, 0) + " giờ</span>";
                    }
                    else if (time.TotalDays < 9)
                    {
                        a = " <span class='label label-default'>" + "Đã offline " + Math.Round(time.TotalDays, 0) + " ngày</span>";

                    }
                    else
                    {
                        a = " <span class='label label-default'>" + "Lần online cuối " + item.student.last_login + " </span>";
                    }
                }
                int i = 0;
                i++;
                string dd = item.student_thread.id_exam.ToString() + "i" + item.student.id_student;
                string uid = "<a href = '/Admin/Thread/SDelete/" + dd + "' class='btn btn-danger waves-effect' data-ajax='true' data-ajax-complete='$('#" + dd + "').remove()' data-ajax-confirm='Bạn có chắc xóa bản ghi này?' data-ajax-method='Delete'><i class='material-icons'>delete</i> <span>Xóa</span> </a>";

                data.Add(new JsonRoomDataModel()
                {
                    id = i.ToString(),
                    student_name = item.student.student_name,
                    changetab = item.student_thread.count_tab_focus.ToString(),
                    data_client = "IP " + item.student.ip + " truy cập qua " + item.student.info_browser,
                    start_time = item.student_thread.start_time.ToString(),
                    end_time = item.student_thread.end_time.ToString(),
                    id_exam = item.student_thread.id_exam.ToString(),
                    score = item.student_thread.score_number.ToString(),
                    status = a,
                    action = uid
                });
            }



            return Json(new
            {
                data = data,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpDelete]
        
        public void SDelete(string id)
        {
            var id_exam = id.Split('i')[0];
            var id_student = id.Split('i')[1];
            var result = new ThreadDao().SDelete(int.Parse(id_exam), int.Parse(id_student));
            if (result == true)
            {

            }
           // return RedirectToAction("OpenRoom");
        }

        [HttpGet]
        public ActionResult Result()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return View("Error");
            ViewBag.AdminName = session.name;


            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Xuất kết quả của kỳ thi", "Admin/Thread/Result", session.id_admin);
            var dao1 = new ThreadDao();
            var model = dao1.GetThreadResult();
            return View(model);

        }
        public ActionResult DownloadResult(int id)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 0)
                return View("Error");
            ViewBag.AdminName = session.name;
            var tmp = new ImportExport();
            // Gọi lại hàm để tạo file excel
            var stream = tmp.CreateExcelResultFile(id);
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", "attachment; filename=KetQua.xlsx");
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
            // Redirect về luôn trang index :D
            return RedirectToAction("Admin/Thread/Result");
        }
        public void SetViewBag(int? selectedId = null)
        {
            var dao = new SubjectDao();
            ViewBag.id_subject = new SelectList(dao.ListAll(), "id_subject", "subject_name", selectedId);
        }
    }
}
