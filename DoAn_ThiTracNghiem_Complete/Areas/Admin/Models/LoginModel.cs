using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Hãy nhập tài khoản")]

        public string username { set; get; }
        [Required(ErrorMessage = "Hãy nhập mật khẩu")]

        public string password { set; get; }

    }
}