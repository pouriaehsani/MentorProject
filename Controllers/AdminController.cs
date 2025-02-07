using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using mentorproject.Models;
using System.Web.Security;
using System.IO;
using System.Web.Helpers;

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

                    var cookietext = Encoding.UTF8.GetBytes(user.pkID.ToString());
                    var encryptionValue = Convert.ToBase64String(MachineKey.Protect(cookietext));
                    
                    Response.Cookies["lc"].Value = encryptionValue;
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult deleteCourses(int id) {

            try
            {
                var courseData = context.Tbl_course.Where(x=> x.pkID==id).Single();
                context.Tbl_course.Remove(courseData);
                //context.SaveChanges();

                return Json(new { status = true, m = "" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex) {
                return Json(new { status = false, m = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        
        [HttpPost]
        
        public ActionResult editCourseImg(int imgpkID,int coursepkID, string imgTitle, string imgAlt, HttpPostedFileBase imgsrc) {
            string fileName = "";
            if (validation()) {
                try
                {
                    var imgFileName = context.Tbl_img.Where(x=> x.pkID == imgpkID).Single();

                    if (imgsrc != null)
                    {

                        if (imgsrc.ContentLength > 10000 && imgsrc.ContentLength < 10000000)
                        {
                            var prefixName = imgsrc.FileName.Split('.');
                            fileName = Path.GetFileName("ci_" + coursepkID.ToString()+"."+ prefixName[1]);
                            var path = Path.Combine(Server.MapPath("~/AdminAssets/img/ImgCourse"), fileName);
                            imgsrc.SaveAs(path);
                            imgFileName.address = fileName;
                        }
                        else
                        {
                            return Json(new { status = false, message="فایل آپلود شده میبایست بین 10 کیلوبایت تا 10 مگابایت باشد..." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    
                        imgFileName.title = imgTitle;
                        imgFileName.alt = imgAlt;
                        context.SaveChanges();
                    return Json(new { status = true, message = "ویرایش انجام شد...",reff = imgFileName.address }, JsonRequestBehavior.AllowGet);


                }
                catch (Exception ex)
                {
                    return Json(new { status = false, ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new { status = false, message = "validation failed!!!" }, JsonRequestBehavior.AllowGet);
        }

        private bool validation()
        {

            if (Request.Cookies["lc"].Value == "null") { Response.Redirect("~/admin/login"); }
            if (Request.Cookies["lc"].Value == "") { Response.Redirect("~/Home/login"); }

            var bytes = Convert.FromBase64String(Request.Cookies["lc"].Value);
            var output = MachineKey.Unprotect(bytes);
            string result = Encoding.UTF8.GetString(output);
            int userID = int.Parse(result);

            var user = context.tbl_admins.Where(x => x.pkID == userID).FirstOrDefault();

            if (user.pkID == 1)
            {
                return true;
            }

            else { 
                return false;
            }
        }
    }

    
}