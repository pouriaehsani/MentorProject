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
           

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Courses()
        {

            return View();

        }
        public ActionResult Trainers()
        {

            return View();

        }

        public ActionResult Events()
        {

            return View();

        }

        public ActionResult Pricing()
        {

            return View();

        }

        public ActionResult Contact()
        {

            return View();

        }

        public ActionResult _counter2()
        {

            ViewBag.student = context.table_student.Count();
            ViewBag.courses = context.Tbl_course.Count();
            ViewBag.events = context.Table_events.Count();
            ViewBag.trainers = context.Table_teacher.Count();

            return PartialView();


        }

    }
}