using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_ThiTracNghiem_Complete.Areas.Student.Models;
using DoAn_ThiTracNghiem_Complete.Common;
using Model.Dao;
namespace DoAn_ThiTracNghiem_Complete.Areas.Student.Controllers
{
    public class StudentLoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            //try
            //{
            if (session != null && session.id_student > 0) return RedirectToAction("Index", "Home");
            //}
            ////catch
            ////{
            //    return View();
            //}
            return View();

        }
        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new StudentDao();
                var result = dao.IsValid(model.username, Encryptor.MD5Hash(model.password));
                if (result)
                {
                    var admin = dao.GetByUserName(model.username);
                    var studenSession = new StudentLogin();

                    studenSession.id_student = admin.id_student;
                    studenSession.student_name = admin.student_name;
                    studenSession.student_avatar = admin.student_avatar;
                    Session.Add(CommonConstants.STUDENT_SESSION, studenSession);
                    dao.UpdateLastLogin(studenSession.id_student, GetIPAddress(), GetUserEnvironment());
                    dao.UpdateLastSeen("Trang chủ", Url.Action("Index"), studenSession.id_student);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Sai thông tin đăng nhập.");
                }

            }
            return View(model);
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