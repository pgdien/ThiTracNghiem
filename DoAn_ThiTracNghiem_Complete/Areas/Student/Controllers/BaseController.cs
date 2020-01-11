using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DoAn_ThiTracNghiem_Complete.Common;
namespace DoAn_ThiTracNghiem_Complete.Areas.Student.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (StudentLogin)Session[CommonConstants.STUDENT_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "StudentLogin", action = "Index", Area = "Student" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}