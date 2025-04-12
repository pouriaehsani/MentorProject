using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mentorproject.Models;

namespace mentorproject.Controllers
{
    public class HomeController : Controller
    {
        educationDb context = new educationDb();

        // GET: Home
        public ActionResult Index()
        {
            ViewBag.title = "index";
            GeneralModelView model = new GeneralModelView();
            model.Pages = context.Table_pages.Where(x => x.pkID == 1).Single();
            
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.title = "about";
            GeneralModelView model = new GeneralModelView();
            model.Pages = context.Table_pages.Where(x => x.pkID == 2).Single();
            return View(model);
        }

        public ActionResult Courses()
        {
            ViewBag.title = "courses";
            GeneralModelView model = new GeneralModelView();
            model.Pages = context.Table_pages.Where(x => x.pkID == 3).Single();
            model.Courses=context.View_courses.ToList();
            return View(model);

        }
        public ActionResult Trainers()
        {
            ViewBag.title = "trainers";
            GeneralModelView model = new GeneralModelView();
            model.Pages = context.Table_pages.Where(x => x.pkID == 5).Single();
            model.Teachers = context.View_trainers.ToList();
            return View(model);

        }

        public ActionResult Events()
        {
            ViewBag.title = "events";
            GeneralModelView model = new GeneralModelView();
            model.Pages = context.Table_pages.Where(x => x.pkID == 4).Single();

            return View(model);

        }

        public ActionResult Contact()
        {
            ViewBag.title = "contact";
            GeneralModelView model = new GeneralModelView();
            model.Pages = context.Table_pages.Where(x => x.pkID == 6).Single();

            return View(model);

        }

        public ActionResult _counter2()
        {

            ViewBag.student = context.table_student.Count();
            ViewBag.courses = context.Tbl_course.Count();
            ViewBag.events = context.Table_events.Count();
            ViewBag.trainers = context.Table_teacher.Count();

            return PartialView();


        }

        public ActionResult setLanguage(int id)
        {
            Response.Cookies["langID"].Value = id.ToString();
            Response.Cookies["langID"].Expires = DateTime.Now.AddDays(500);
            return RedirectToAction("Index");
        }
    }
}