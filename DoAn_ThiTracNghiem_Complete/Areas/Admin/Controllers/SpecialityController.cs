using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_ThiTracNghiem_Complete.Common;
using Model.Dao;
using Model.EF;

namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Controllers
{
    public class SpecialityController : BaseController
    {
        // GET: Admin/Speciality
        public ActionResult Index()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Quản lý ngành đào tạo", "Admin/Speciality/", session.id_admin);
            var dap1 = new SpecialityDao();
            var model = dap1.ListAll();
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
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Thêm ngành học", "Admin/Speciality/Create", session.id_admin);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Speciality collection)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;
            if (ModelState.IsValid)
            {

                var dao = new SpecialityDao();


                int id = dao.Insert(collection);
                if (id > 0)
                {
                    //để thông báo thêm thành công
                    SetNotice("Hệ thống đã thêm thành công.", "success");
                    return RedirectToAction("Create");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm ngành học không thành công.");
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

            var dao1 = new SpecialityDao().ViewDetail(id);
            if (dao1 == null) return View("Error");
            else
            {
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Sửa thông tin của ngành " + dao1.speciality_name, "/Admin/Class/Edit" + dao1.id_speciality, session.id_admin);

                return View(dao1);
            }
        }
        [HttpPost]
        public ActionResult Edit(Model.EF.Speciality collection)
        {


            var dao = new SpecialityDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;
            var id = dao.Update(collection);
            if (id)
            {
                SetNotice("Hệ thống đã sửa thành công ngành " + collection.speciality_name + ".", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetNotice("Có lỗi xảy ra!!", "danger");
                ModelState.AddModelError("", "Cập nhật thông tin ngành không thành công.");
            }
            return View();

        }
        
        public ActionResult Delete(int id)
        {
            var result = new SpecialityDao().Delete(id);
            if (result == true)
            {

            }
            return RedirectToAction("Index");
        }

    }
}