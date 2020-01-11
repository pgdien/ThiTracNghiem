using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_ThiTracNghiem_Complete.Common;
using Model.Dao;
namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Controllers
{
    public class StudentController : BaseController
    {
        // GET: Admin/Student
        public ActionResult Index()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;

            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Quản lý sinh viên", "Admin/Student/", session.id_admin);
            var dao1 = new StudentDao();
            var model = dao1.ListAll();
            return View(model);

        }

        [HttpGet]
        public ActionResult Create()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;
            SetViewBag();
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Thêm sinh viên", "/Admin/Student/Create", session.id_admin);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Model.EF.Student student)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;
            if (ModelState.IsValid)
            {

                var dao = new StudentDao();
                if (!string.IsNullOrEmpty(student.password) && dao.IsUserNameExist(student.username))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(student.password))
                    {
                        var encryptedMd5Pas = Encryptor.MD5Hash(student.password);
                        student.password = encryptedMd5Pas;
                    }
                    int id = dao.Insert(student);
                    if (id > 0)
                    {
                        //để thông báo thêm thành công
                        SetNotice("Hệ thống đã thêm thành công.", "success");
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm sinh viên không thành công.");
                    }
                }
            }
            SetViewBag();
            return View();

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;

            var student = new StudentDao().ViewDetail(id);
            if (student == null) return View("Error");
            else
            {
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Sửa thông tin sinh viên " + student.student_name, "/Admin/Student/Edit" + student.id_student, session.id_admin);
                SetViewBag(student.id_student);
                return View(student);
            }

        }
        [HttpPost]
        public ActionResult Edit(Model.EF.Student tmp)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;

            var dao = new StudentDao();
            if (!dao.IsUserNameIDExist(tmp.username, tmp.id_student))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
            }
            else
            {
                if (!string.IsNullOrEmpty(tmp.password))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(tmp.password);
                    tmp.password = encryptedMd5Pas;
                }

                var id = dao.Update(tmp);
                if (id)
                {
                    SetNotice("Hệ thống đã sửa thành công " + tmp.student_name + ".", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật tài khoản không thành công.");
                }

            }
            SetViewBag(tmp.id_student);
            return View();

        }
        
        public ActionResult Delete(int id)
        {
            var result = new StudentDao().Delete(id);
            if (result == true)
            {

            }
            return RedirectToAction("Index");
        }

        public void SetViewBag(int? selectedId = null)
        {
            var dao = new ClassDao();
            ViewBag.id_class = new SelectList(dao.ListAll(), "id_class", "class_name", selectedId);
        }
    }
}