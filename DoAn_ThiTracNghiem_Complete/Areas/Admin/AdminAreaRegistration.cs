using System.Web.Mvc;

namespace DoAn_ThiTracNghiem_Complete.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home",action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}