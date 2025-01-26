using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using mentorproject.Models;

namespace mentorproject.Controllers
{
    public class AdminController : Controller
    {

        school1 context = new school1();
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Login()
        {

            return View();

        }

        public ActionResult Register()
        {

            return View();

        }
        public ActionResult Profiles()
        {

            return View();

        }

        public ActionResult Login_check(string email, string pass)
        {
            var user = context.tbl_admins.Where(x=> x.email == email).FirstOrDefault(); 
            var status = 0;         // چک میکند ایمیل وجود دارد یا نه اکر وجود داشت برمیکرداند اکر نداشت نال برمیکرداند 

            if (user == null)
            {
                status = 1; // user doesnt exist... 
            }
            else 
            {
                if (user.password == pass)
                {
                    Response.Cookies["lc"].Value = user.pkID.ToString();
                    Response.Cookies["lc"].Expires = DateTime.Now.AddDays(100); 
                    status = 3;     //username and password is correct...
                }
                else
                {
                    status = 2; //wrong password...
                }
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult courses() 
        {
            ViewBag.Courses = context.View_courses.ToList();
            ViewBag.Category = context.Table_category.ToList();
            ViewBag.Teachers = context.Table_teacher.ToList();

           


            return View();

        }

        public ActionResult editCourses(string ct,string cdis,int cm,int cc, bool cs ,int cID)
        {
            try
            {
                var courseData = context.Tbl_course.Where(x => x.pkID == cID).Single();

                courseData.title = ct;
                courseData.description = cdis;
                courseData.fkTEACH = cm;
                courseData.fkCat = cc;
                courseData.status = cs;
                context.SaveChanges();

                return Json(new {status=true,m = ""}, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex) 
            { 
                return Json(new {status=false,m = ex.Message}, JsonRequestBehavior.AllowGet);
            
            }
            
        }

        public ActionResult deleteCourses(int id) {
            try
            {
                var c = context.Tbl_course.Where(x => x.pkID == id).Single();
                context.Tbl_course.Remove(c);
                context.SaveChanges();

                return Json(new { status = true, m = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                return Json(new { status = false, m = ex.Message }, JsonRequestBehavior.AllowGet);
            }
       
        }
    }

}