using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DoAn_ThiTracNghiem_Complete.Common;

namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
            }
            base.OnActionExecuting(filterContext);
        }
        protected void SetNotice(string message, string type)
        {
            TempData["NoticeMessage"] = message;
            TempData["NoticeType"] = type;

            if (type == "success") TempData["NoticeTitle"] = "Chúc mừng!";
            else TempData["NoticeTitle"] = "";
        }
    }
}