using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_ThiTracNghiem_Complete.Common;
using Model.Dao;
namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.AdminName = session.name;
            var dao = new AdminDao();
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Bảng điều khiển", "Admin/Home/", session.id_admin);
            ViewBag.permission = session.id_permission;
            ViewBag.AdminCount = dao.CountAdmin();
            ViewBag.CountClass = dao.CountClass();
            ViewBag.CountQuestion = dao.CountQuestion();
            ViewBag.CountRoom = dao.CountRoom();
            ViewBag.CountSpeciality = dao.CountSpeciality();
            ViewBag.CountStudent = dao.CountStudent();
            ViewBag.CountSubject = dao.CountSubject();
            ViewBag.CountThread = dao.CountThread();
            try
            {
                var path = Server.MapPath(@"~/App_Data/Notice.txt");
                var text = System.IO.File.ReadAllText(path);

                ViewBag.NoticeContent = System.Net.WebUtility.HtmlDecode(text);
            }
            catch { ViewBag.NoticeContent = ""; }
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];

            if (session.id_permission == 2)
                return View("Error");

            ViewBag.Error = 0;
            ViewBag.permission = session.id_permission;
            try
            {

                var path = Server.MapPath(@"~/App_Data/Notice.txt");
                System.IO.File.WriteAllText(path, System.Net.WebUtility.HtmlDecode(collection["notice"]), System.Text.Encoding.UTF8);
                ViewBag.Error = -1;
            }
            catch
            {
                ViewBag.Error = 1;
            }
            ViewBag.AdminName = session.name;
            var dao = new AdminDao();
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Bảng điều khiển", "Admin/Home/", session.id_admin);

            ViewBag.AdminCount = dao.CountAdmin();
            ViewBag.CountClass = dao.CountClass();
            ViewBag.CountQuestion = dao.CountQuestion();
            ViewBag.CountRoom = dao.CountRoom();
            ViewBag.CountSpeciality = dao.CountSpeciality();
            ViewBag.CountStudent = dao.CountStudent();
            ViewBag.CountSubject = dao.CountSubject();
            ViewBag.CountThread = dao.CountThread();
            try
            {
                var path = Server.MapPath(@"~/App_Data/Notice.txt");
                var text = System.IO.File.ReadAllText(path);

                ViewBag.NoticeContent = text;
            }
            catch { ViewBag.NoticeContent = ""; }
            return View();
        }
    }
}