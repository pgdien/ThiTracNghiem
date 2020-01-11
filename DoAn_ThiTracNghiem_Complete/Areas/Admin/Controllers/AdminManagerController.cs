using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using DoAn_ThiTracNghiem_Complete.Common;
namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Controllers
{
    public class AdminManagerController : BaseController
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Quản lý admin", "/Admin/AdminManager/Index", session.id_admin);
            var model = dao.LissAll();
            ViewBag.AdminName = session.name;
            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePass()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.AdminName = session.name;
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Đổi mật khẩu", "/Admin/AdminManager/ChangePass", session.id_admin);
            return View();


        }
        [HttpPost]
        public ActionResult ChangePass(string password)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];

            ViewBag.AdminName = session.name;

            var dao = new AdminDao();

            if (!string.IsNullOrEmpty(password))
            {
                var encryptedMd5Pas = Encryptor.MD5Hash(password);
                password = encryptedMd5Pas;
            }
            else ModelState.AddModelError("", "Mật khẩu quá ngắn.");

            var id = dao.ChangePass(password, session.id_admin);
            if (id)
            {
                SetNotice("Hệ thống đã đổi thành công.", "success");
                return RedirectToAction("ChangePass");
            }
            else
            {
                ModelState.AddModelError("", "Cập nhật tài khoản không thành công.");
            }


            return View();

        }

        [HttpGet]
        public ActionResult Create()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Thêm tài khoản admin", "/Admin/AdminManager/Create", session.id_admin);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Model.EF.Admin admin)
        {

            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;
            if (ModelState.IsValid)
            {

                var dao = new AdminDao();
                if (dao.IsUserNameExist(admin.username))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                }
                else
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(admin.password);
                    admin.password = encryptedMd5Pas;
                    int id = dao.Insert(admin);
                    if (id > 0)
                    {
                        //để thông báo thêm thành công
                        SetNotice("Hệ thống đã thêm thành công.", "success");
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm tài khoản không thành công.");
                    }
                }
            }
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
            var admin = new AdminDao().ViewDetail(id);
            if (admin == null) return View("Error");
            else
            {
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Sửa tài khoản admin " + admin.name, "/Admin/AdminManager/Edit" + admin.id_admin, session.id_admin);
                return View(admin);
            }

        }
        [HttpPost]
        public ActionResult Edit(Model.EF.Admin admin)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;

            var dao = new AdminDao();
            if (!dao.IsUserNameIDExist(admin.username, admin.id_admin))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
            }
            else
            {
                if (!string.IsNullOrEmpty(admin.password))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(admin.password);
                    admin.password = encryptedMd5Pas;
                }

                var id = dao.Update(admin);
                if (id)
                {
                    SetNotice("Hệ thống đã sửa thành công " + admin.name + ".", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật tài khoản không thành công.");
                }

            }
            return View();

        }
        
        public ActionResult Delete(int id)
        {
            var result = new AdminDao().Delete(id);
            if (result == true)
            {

            }
            return RedirectToAction("Index");
        }


    }
}