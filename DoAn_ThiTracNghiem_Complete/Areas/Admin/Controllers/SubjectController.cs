using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_ThiTracNghiem_Complete.Common;
using Model.Dao;
using Model.EF;
using DoAn_ThiTracNghiem_Complete.Areas.Admin.Code;
using System.Threading.Tasks;
using System.IO;
using System.Web.Hosting;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Messaging;

namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Controllers
{
    public class SubjectController : BaseController
    {
        // GET: Admin/Subject
        //phần về thông tin môn :D
        public ActionResult Index()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;

            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Quản lý môn học", "Admin/Subject/", session.id_admin);
            var dap1 = new SubjectDao();
            var model = dap1.LissAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;

            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Thêm môn học", "Admin/Subject/Create", session.id_admin);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subject collection)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;

            if (ModelState.IsValid)
            {

                var dao = new SubjectDao();


                int id = dao.Insert(collection);
                if (id > 0)
                {
                    //để thông báo thêm thành công
                    SetNotice("Hệ thống đã thêm thành công.", "success");
                    return RedirectToAction("Create");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm môn học không thành công.");
                }

            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;


            var dao1 = new SubjectDao().ViewDetail(id);
            if (dao1 == null) return View("Error");
            else
            {
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Sửa thông tin của môn " + dao1.subject_name, "/Admin/Subject/Edit" + dao1.id_subject, session.id_admin);

                return View(dao1);
            }
        }
        [HttpPost]
        public ActionResult Edit(Model.EF.Subject collection)
        {

            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;

            var dao = new SubjectDao();

            var id = dao.Update(collection);
            if (id)
            {
                SetNotice("Hệ thống đã sửa thành công môn " + collection.subject_name + ".", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetNotice("Có lỗi xảy ra!!", "danger");
                ModelState.AddModelError("", "Cập nhật thông tin môn không thành công.");
            }
            return View();

        }


        public ActionResult Delete(int id)
        {
            var result = new SubjectDao().Delete(id);
            if (result == true)
            {

            }
            return RedirectToAction("Index");
        }

        //phần về thông tin các chuyên đề của môn
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;


            var dao2 = new SubjectDao().ViewDetail(id);
            var dao1 = new ThematicDao().ListThematicInSubject(id);
            if (dao2 == null) return View("Error");
            else
            {
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Xem thông tin của môn học " + dao2.subject_name, "/Admin/Subject/Detail" + dao2.id_subject, session.id_admin);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = id;
                return View(dao1);
            }
        }
        [HttpGet]
        public ActionResult CThematic(int id)
        {
            var dao = new AdminDao();
            var dao2 = new SubjectDao().ViewDetail(id);
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;

            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Thêm chuyên đề cho môn " + dao2.subject_name, "Admin/Subject/CThematic/" + dao2.id_subject, session.id_admin);
            TempData["subject_name"] = dao2.subject_name;
            TempData["id_subject"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CThematic(Thematic collection)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;

            if (ModelState.IsValid)
            {

                var dao = new ThematicDao();


                int id = dao.Insert(collection);
                if (id > 0)
                {
                    //để thông báo thêm thành công
                    SetNotice("Hệ thống đã thêm thành công.", "success");
                    return RedirectToAction("Detail", new { id = collection.id_subject });
                }
                else
                {
                    var dao2 = new SubjectDao().ViewDetail(collection.id_subject);
                    TempData["subject_name"] = dao2.subject_name;
                    TempData["id_subject"] = collection.id_subject;
                    ModelState.AddModelError("", "Thêm chuyên đề không thành công.");
                }

            }
            var dao3 = new SubjectDao().ViewDetail(collection.id_subject);
            TempData["subject_name"] = dao3.subject_name;
            TempData["id_subject"] = collection.id_subject;
            return View();
        }

        [HttpGet]
        public ActionResult EThematic(int id)
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;


            var dao1 = new ThematicDao().ViewDetail(id);
            if (dao1 == null) return View("Error");
            else
            {
                var dao2 = new SubjectDao().ViewDetail(dao1.id_subject);
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Sửa tên chuyên đề của môn " + dao2.subject_name, "/Admin/Subject/EThematic/" + dao2.id_subject, session.id_admin);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = dao2.id_subject;
                return View(dao1);
            }
        }
        [HttpPost]
        public ActionResult EThematic(Model.EF.Thematic collection)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;


            var dao = new ThematicDao();

            var id = dao.Update(collection);
            if (id)
            {
                SetNotice("Hệ thống đã sửa thành công chuyên đề " + collection.thematic_name + ".", "success");
                return RedirectToAction("Detail", new { id = collection.id_subject });
            }
            else
            {
                var dao2 = new SubjectDao().ViewDetail(collection.id_subject);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = collection.id_subject;
                SetNotice("Có lỗi xảy ra!!", "danger");
                ModelState.AddModelError("", "Cập nhật thông tin chuyên đề không thành công.");
            }
            var dao3 = new SubjectDao().ViewDetail(collection.id_subject);
            TempData["subject_name"] = dao3.subject_name;
            TempData["id_subject"] = collection.id_subject;
            return View();

        }


        public ActionResult DThematic(int id)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;

            var result = new ThematicDao().Delete(id);
            if (result == true)
            {

            }
            return RedirectToAction("Index");
        }

        // phần các câu hỏi của môn
        [HttpGet]
        public ActionResult QDetail(int id)
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;


            var dao2 = new SubjectDao().ViewDetail(id);
            var dao1 = new QuestionDao().ListQuestionInSubject(id);
            if (dao2 == null) return View("Error");
            else
            {
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Xem các câu hỏi của môn học " + dao2.subject_name, "/Admin/Subject/QDetail" + dao2.id_subject, session.id_admin);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = id;
                var list = dao1.ToList();
                foreach (var item in list)
                {
                    item.question_content = RemoveHtml(RemoveFomula(item.question_content));
                    if (item.question_content.Length >= 90)
                        item.question_content = item.question_content.Substring(0, 90) + "....";
                }

                return View(list);
            }
        }
        [HttpGet]
        public ActionResult QCreate(int id)
        {
            var dao = new AdminDao();
            var dao2 = new SubjectDao().ViewDetail(id);
            if (dao2 == null) return View("Error");
            else
            {
                var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
                if (session.id_permission == 2)
                    return View("Error");
                ViewBag.AdminName = session.name;

                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Thêm câu hỏi cho môn " + dao2.subject_name, "Admin/Subject/QCreate/" + dao2.id_subject, session.id_admin);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = id;
                SetViewBag(id);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QCreate(Question collection, int id_subject, HttpPostedFileBase File)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;

            if (ModelState.IsValid)
            {

                var dao = new QuestionDao();

                try
                {
                    string fileName = Path.GetFileName(File.FileName);
                    //Upload image
                    string path = Server.MapPath("~/Assets/img_questions/");
                    //Đuối hỗ trợ
                    var allowedExtensions = new[] { ".png", ".jpg" };
                    //Lấy phần mở rộng của file
                    string extensionName = Path.GetExtension(File.FileName).ToLower();
                    //Kiểm tra đuôi file
                    if (!allowedExtensions.Contains(extensionName))
                    {
                        var dao2 = new SubjectDao().ViewDetail(id_subject);
                        TempData["subject_name"] = dao2.subject_name;
                        TempData["id_subject"] = id_subject;
                        SetViewBag(id_subject);
                        ModelState.AddModelError("", "Chỉ chọn file ảnh đuôi .PNG .png .JPG .jpg");

                        return View();
                    }
                    else
                    {
                        // Tạo tên file ngẫu nhiên
                        collection.img = DateTime.Now.Ticks.ToString() + extensionName;
                        // Upload file lên server
                        File.SaveAs(path + collection.img);
                    }

                }
                catch (Exception) { }
                int id = dao.Insert(collection);
                if (id > 0)
                {
                    SetViewBag(id_subject);
                    //để thông báo thêm thành công
                    SetNotice("Hệ thống đã thêm thành công.", "success");
                    return RedirectToAction("QDetail", new { id = id_subject });
                }
                else
                {
                    var dao2 = new SubjectDao().ViewDetail(id_subject);
                    TempData["subject_name"] = dao2.subject_name;
                    TempData["id_subject"] = id_subject;
                    SetViewBag(id_subject);
                    ModelState.AddModelError("", "Thêm câu hỏi không thành công.");
                }

            }
            var dao3 = new SubjectDao().ViewDetail(id_subject);
            TempData["subject_name"] = dao3.subject_name;
            TempData["id_subject"] = id_subject;
            SetViewBag(id_subject);
            return View();
        }

        [HttpGet]
        public ActionResult QEssayCreate(int id)
        {
            var dao = new AdminDao();
            var dao2 = new SubjectDao().ViewDetail(id);
            if (dao2 == null) return View("Error");
            else
            {
                var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
                if (session.id_permission == 2)
                    return View("Error");
                ViewBag.AdminName = session.name;

                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Thêm câu hỏi cho môn " + dao2.subject_name, "Admin/Subject/QCreate/" + dao2.id_subject, session.id_admin);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = id;
                SetViewBag(id);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult QEssayCreate(Question collection, int id_subject)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;

            if (ModelState.IsValid)
            {
                var dao = new QuestionDao();

                collection.question_content = System.Net.WebUtility.HtmlDecode(collection.question_content);
                collection.A = System.Net.WebUtility.HtmlDecode(collection.A);
                collection.is_essay = 1;
                collection.is_change = 0;
                collection.B = "-1";
                collection.C = "-1";
                collection.D = "-1";
                int id = dao.Insert(collection);
                if (id > 0)
                {
                    SetViewBag(id_subject);
                    //để thông báo thêm thành công
                    SetNotice("Hệ thống đã thêm thành công.", "success");
                    return RedirectToAction("QDetail", new { id = id_subject });
                }
                else
                {
                    var dao2 = new SubjectDao().ViewDetail(id_subject);
                    TempData["subject_name"] = dao2.subject_name;
                    TempData["id_subject"] = id_subject;
                    SetViewBag(id_subject);
                    ModelState.AddModelError("", "Thêm câu hỏi tự luận không thành công.");
                }
            }
            var dao3 = new SubjectDao().ViewDetail(id_subject);
            TempData["subject_name"] = dao3.subject_name;
            TempData["id_subject"] = id_subject;
            SetViewBag(id_subject);
            return View();
        }

        [HttpGet]
        public ActionResult QEssayEdit(int id)
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;
            var dao1 = new QuestionDao().ViewDetail(id);
            if (dao1 == null || dao1.is_essay == 0) return View("Error");
            else
            {
                var dao3 = new ThematicDao().ViewDetail(dao1.id_thematic);
                var dao2 = new SubjectDao().ViewDetail(dao3.id_subject);
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Sửa câu hỏi của môn " + dao2.subject_name, "/Admin/Subject/QEssayEdit/" + id, session.id_admin);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = dao2.id_subject;
                SetViewBag(dao3.id_subject, dao1.id_thematic);
                return View(dao1);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult QEssayEdit(Question collection, int id_subject)
        {

            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;

            var dao = new QuestionDao();
            collection.correct_answer = System.Net.WebUtility.HtmlDecode(collection.question_content);
            collection.A = System.Net.WebUtility.HtmlDecode(collection.A);
            var id = dao.Update(collection);
            if (id)
            {
                SetNotice("Hệ thống đã sửa thành công câu hỏi có id " + collection.id_question + ".", "success");
                return RedirectToAction("QDetail", new { id = id_subject });
            }
            else
            {
                var dao2 = new SubjectDao().ViewDetail(id_subject);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = id_subject;
                SetNotice("Có lỗi xảy ra!!", "danger");
                ModelState.AddModelError("", "Cập nhật thông tin câu hỏi không thành công.");
            }
            var dao3 = new SubjectDao().ViewDetail(id_subject);
            TempData["subject_name"] = dao3.subject_name;
            TempData["id_subject"] = id_subject;
            SetViewBag(id_subject);
            return View();

        }

        [HttpGet]
        public ActionResult QEdit(int id)
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;


            var dao1 = new QuestionDao().ViewDetail(id);
            if (dao1 == null || dao1.is_essay == 1) return View("Error");
            else
            {
                var dao3 = new ThematicDao().ViewDetail(dao1.id_thematic);
                var dao2 = new SubjectDao().ViewDetail(dao3.id_subject);
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Sửa câu hỏi của môn " + dao2.subject_name, "/Admin/Subject/QEdit/" + id, session.id_admin);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = dao2.id_subject;
                //dao1.img = Server.MapPath("~/Assets/img_questions/") + dao1.img;
                SetViewBag(dao3.id_subject, dao1.id_thematic);
                return View(dao1);
            }
        }

        [HttpPost]
        public ActionResult Qedit(Question collection, int id_subject, HttpPostedFileBase File)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;
            try
            {
                if (File != null)
                {
                    string fileName = Path.GetFileName(File.FileName);
                    //Upload image
                    string path = Server.MapPath("~/Assets/img_questions/");
                    //Đuối hỗ trợ
                    var allowedExtensions = new[] { ".png", ".jpg" };
                    //Lấy phần mở rộng của file
                    string extensionName = Path.GetExtension(File.FileName).ToLower();
                    //Kiểm tra đuôi file
                    if (!allowedExtensions.Contains(extensionName))
                    {
                        ModelState.AddModelError("", "Chỉ chọn file ảnh đuôi .PNG .png .JPG .jpg");
                        var dao2 = new SubjectDao().ViewDetail(id_subject);
                        TempData["subject_name"] = dao2.subject_name;
                        TempData["id_subject"] = id_subject;
                        SetViewBag(id_subject);
                        return View();
                    }
                    else
                    {
                        // Tạo tên file ngẫu nhiên
                        collection.img = DateTime.Now.Ticks.ToString() + extensionName;
                        // Upload file lên server
                        File.SaveAs(path + collection.img);
                    }
                }
                else
                {
                    //  collection.
                }

            }
            catch (Exception) { }
            var dao = new QuestionDao();

            var id = dao.Update(collection);
            if (id)
            {
                SetNotice("Hệ thống đã sửa thành công câu hỏi có id " + collection.id_question + ".", "success");
                return RedirectToAction("QDetail", new { id = id_subject });
            }
            else
            {
                var dao2 = new SubjectDao().ViewDetail(id_subject);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = id_subject;
                SetNotice("Có lỗi xảy ra!!", "danger");
                ModelState.AddModelError("", "Cập nhật thông tin chuyên đề không thành công.");
            }
            var dao3 = new SubjectDao().ViewDetail(id_subject);
            TempData["subject_name"] = dao3.subject_name;
            TempData["id_subject"] = id_subject;
            SetViewBag(id_subject);
            return View();

        }


        public ActionResult QDelete(int id)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission == 2)
                return View("Error");
            ViewBag.AdminName = session.name;

            var result = new QuestionDao().Delete(id);
            if (result == true)
            {

            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult AddQuest(int id)
        {
            var dao = new AdminDao();
            var dao2 = new SubjectDao().ViewDetail(id);
            if (dao2 == null) return View("Error");
            else
            {
                var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
                if (session.id_permission == 2)
                    return View("Error");
                ViewBag.AdminName = session.name;


                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Thêm câu hỏi cho môn " + dao2.subject_name, "Admin/Subject/QCreate/" + dao2.id_subject, session.id_admin);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = id;
                SetViewBag(id);
            }
            return View();
        }



        [HttpPost]
        public ActionResult AddQuest(HttpPostedFileBase file, int id_subject, int id_thematic)
        {
            try
            {
                var dao = new QuestionDao();
                var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
                if (session.id_permission == 2)
                    return View("Error");
                ViewBag.AdminName = session.name;

                var ad = new ImportExport();
                HttpPostedFileBase upload = Request.Files["file"];
                string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), "cauhoi.xlsx");
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    file.SaveAs(path);
                }
                var data = ad.ReadFromExcelfile(path, "");

                int id = dao.InsertList(data, id_thematic);
                if (id > 0)
                {
                    SetViewBag(id_subject);
                    //để thông báo thêm thành công
                    SetNotice("Hệ thống đã thêm thành công " + id.ToString() + " câu hỏi", "success");
                    return RedirectToAction("QDetail", new { id = id_subject });
                }
                else
                {
                    var dao2 = new SubjectDao().ViewDetail(id_subject);
                    TempData["subject_name"] = dao2.subject_name;
                    TempData["id_subject"] = id_subject;
                    SetViewBag(id_subject);
                    ModelState.AddModelError("", "Thêm câu hỏi không thành công.");
                }
            }
            catch
            {
                var dao2 = new SubjectDao().ViewDetail(id_subject);
                TempData["subject_name"] = dao2.subject_name;
                TempData["id_subject"] = id_subject;
                SetViewBag(id_subject);
                ModelState.AddModelError("", "Thêm câu hỏi không thành công. Vui lòng kiểm tra file!");
            }

            var dao3 = new SubjectDao().ViewDetail(id_subject);
            TempData["subject_name"] = dao3.subject_name;
            TempData["id_subject"] = id_subject;
            SetViewBag(id_subject);
            return View();
        }

        public void SetViewBag(int id_subject, int? selectedId = null)
        {
            var dao = new ThematicDao();
            ViewBag.id_thematic = new SelectList(dao.ListThematicInSubject(id_subject), "id_thematic", "thematic_name", selectedId);
        }

        public string RemoveHtml(string text)
        {
            return Regex.Replace(text, "<[^>]*>", string.Empty);
        }

        public string RemoveFomula(string text)
        {
            string text_start = "<span class=\"math-tex\">";
            string text_end = "</span>";
            int index = -1;
            index = text.IndexOf(text_start);
            while (index > -1)
            {
                int last_index = text.IndexOf(text_end, index);
                text = text.Remove(index, last_index - index + 7);
                text = text.Insert(index, "#MATH_FOMULA#");
                index = text.IndexOf(text_start);
            }
            return text;
        }
    }
}