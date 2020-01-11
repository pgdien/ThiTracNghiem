using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using DoAn_ThiTracNghiem_Complete.Areas.Admin.Code;
using Model.Dao;
using DoAn_ThiTracNghiem_Complete.Common;
using DoAn_ThiTracNghiem_Complete.Areas.Admin.Models;

namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        [HttpGet]
        public ActionResult Index()
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            try
            {
                if (session.id_admin > 0) return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
            return View();
        }//#index

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index(LoginModel model)
        //{
        //    var result = new AdminModel().Login(model.username, model.password);
        //    if (result && ModelState.IsValid)
        //    {
        //        SessionHelper.SetSession(new UserSession() { username = model.username });
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
        //      }
        //    return View(model);
        //} //#index
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Index(LoginModel model)
        {


            if (ModelState.IsValid)
            {
                var dao = new AdminDao();
                var result = dao.Login(model.username, Encryptor.MD5Hash(model.password));
                if (result)
                {
                    var admin = dao.GetByUserName(model.username);
                    var adminSession = new AdminLogin();
                    adminSession.username = admin.username;
                    adminSession.id_admin = admin.id_admin;
                    adminSession.id_permission = admin.id_permission;
                    adminSession.name = admin.name;
                    Session.Add(CommonConstants.USER_SESSION, adminSession);
                    dao.UpdateLastLogin(adminSession.id_admin);
                    dao.UpdateLastSeen("Trang chủ", Url.Action("Index"), adminSession.id_admin);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Sai thông tin đăng nhập.");
                }

            }
            return View(model);
        } //#index


        public ActionResult Logout()
        {

            Session[Common.CommonConstants.USER_SESSION] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}