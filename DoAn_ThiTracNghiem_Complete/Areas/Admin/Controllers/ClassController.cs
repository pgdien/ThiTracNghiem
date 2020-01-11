using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Model.Dao;
using DoAn_ThiTracNghiem_Complete.Common;
using Model.EF;

namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Controllers
{
    public class ClassController : BaseController
    {
        // GET: Admin/Class
        public ActionResult Index()
        {
            var dao = new AdminDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;
            dao.UpdateLastLogin(session.id_admin);
            dao.UpdateLastSeen("Quản lý lớp", "Admin/Class/", session.id_admin);
            var classdao = new ClassDao();
            var model = classdao.LissAll();
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
            dao.UpdateLastSeen("Thêm lớp", "Admin/Class/Create", session.id_admin);

            SetViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Class collection)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;
            if (ModelState.IsValid)
            {

                var dao = new ClassDao();


                int id = dao.Insert(collection);
                if (id > 0)
                {
                    //để thông báo thêm thành công
                    SetNotice("Hệ thống đã thêm thành công.", "success");
                    return RedirectToAction("Create");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm lớp không thành công.");
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
            var lop = new ClassDao().ViewDetail(id);
            if (lop == null) return View("Error");
            else
            {
                dao.UpdateLastLogin(session.id_admin);
                dao.UpdateLastSeen("Sửa thông tin của lớp " + lop.class_name, "/Admin/Class/Edit" + lop.id_class, session.id_admin);
                SetViewBag(lop.id_speciality);
                return View(lop);
            }
        }
        [HttpPost]
        public ActionResult Edit(Class collection)
        {


            var dao = new ClassDao();
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session.id_permission != 1)
                return View("Error");
            ViewBag.AdminName = session.name;
            var id = dao.Update(collection);
            if (id)
            {
                SetNotice("Hệ thống đã sửa thành công lớp " + collection.class_name + ".", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetNotice("Có lỗi xảy ra!!", "danger");
                ModelState.AddModelError("", "Cập nhật thông tin lớp không thành công.");
            }

            SetViewBag(collection.id_speciality);
            return View();

        }
        
        public ActionResult Delete(int id)
        {
            var result = new ClassDao().Delete(id);
            if (result == true)
            {

            }
            return RedirectToAction("Index");
        }
        public void SetViewBag(int? selectedId = null)
        {
            var dao = new SpecialityDao();
            ViewBag.id_speciality = new SelectList(dao.ListAll(), "id_speciality", "speciality_name", selectedId);
        }
    }
}