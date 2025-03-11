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
using System.Web.UI;


namespace mentorproject.Controllers
{
    public class AdminController : Controller
    {

        educationDb context = new educationDb();
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
            var user = context.tbl_admins.Where(x => x.email == email).FirstOrDefault();
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

        public ActionResult editCourses(string ct, string cdis, int cm, int cc, bool cs, int cID)
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

                return Json(new { status = true, m = "" }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { status = false, m = ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult deleteCourses(int id)
        {

            try
            {
                var courseData = context.Tbl_course.Where(x => x.pkID == id).Single();
                var imgData = context.Tbl_img.Where(x => x.pkID == courseData.fkImg).Single();

                var path = Path.Combine(Server.MapPath("~/AdminAssets/img/ImgCourse"), imgData.address);
                if (imgData.address != "defultImg.jpg")
                {
                    if (System.IO.File.Exists(path))  // Check if the file exists before deleting it.
                    {
                        System.IO.File.Delete(path);   // Delete the file from the file system.
                    }
                }

                context.Tbl_course.Remove(courseData);
                context.Tbl_img.Remove(imgData);
                context.SaveChanges();

                return Json(new { status = true, m = "" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = false, m = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }



        [HttpPost]
        public ActionResult editCourseImg(int imgpkID, int coursepkID, string imgTitle, string imgAlt, HttpPostedFileBase imgsrc)
        {
            string fileName = "";
            if (validation())
            {
                try
                {

                    var imgFileName = context.Tbl_img.Where(x => x.pkID == imgpkID).Single();

                    if (imgsrc != null)
                    {

                        if (imgsrc.ContentLength > 10000 && imgsrc.ContentLength < 10000000)
                        {
                            var prefixName = imgsrc.FileName.Split('.');
                            fileName = Path.GetFileName("ci_" + coursepkID.ToString() + "." + prefixName[1]);
                            var path = Path.Combine(Server.MapPath("~/AdminAssets/img/ImgCourse"), fileName);
                            imgsrc.SaveAs(path);
                            imgFileName.address = fileName;
                        }
                        else
                        {
                            return Json(new { status = false, message = "فایل آپلود شده میبایست بین 10 کیلوبایت تا 10 مگابایت باشد..." }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    imgFileName.title = imgTitle;
                    imgFileName.alt = imgAlt;
                    context.SaveChanges();
                    return Json(new { status = true, message = "ویرایش انجام شد...", reff = imgFileName.address }, JsonRequestBehavior.AllowGet);


                }
                catch (Exception ex)
                {
                    return Json(new { status = false, ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new { status = false, message = "validation failed!!!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult createCourse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult createCourse(string newCurseName, string newCourseDis)
        {

            try
            {
                if (validation())
                {
                    Tbl_img createCourseImg = new Tbl_img();
                    createCourseImg.address = "defultImg.jpg";
                    context.Tbl_img.Add(createCourseImg);


                    Tbl_course newCourse = new Tbl_course();
                    newCourse.title = newCurseName;
                    newCourse.description = newCourseDis;
                    newCourse.fkCat = 1;
                    newCourse.fkTEACH = 2;
                    newCourse.status = false;
                    newCourse.fkImg = createCourseImg.pkID;
                    context.Tbl_course.Add(newCourse);
                    context.SaveChanges();


                }
                else
                {
                    ViewBag.message = "شما دسترسی ندارید...";

                }

            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;

            }

            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult createCourses2()
        {
            try
            {
                if (validation())
                {
                    var existingCourse = context.Tbl_course.Where(x => x.title == "دوره جدید").FirstOrDefault();
                    if (existingCourse == null)
                    {
                        Tbl_img createCourseImg = new Tbl_img();
                        createCourseImg.address = "defultImg.jpg";
                        context.Tbl_img.Add(createCourseImg);


                        Tbl_course newCourse = new Tbl_course();
                        newCourse.title = "دوره جدید";
                        newCourse.description = "توضیحات جدید";
                        newCourse.fkCat = 1;
                        newCourse.fkTEACH = 7;
                        newCourse.status = false;
                        newCourse.fkImg = createCourseImg.pkID;
                        context.Tbl_course.Add(newCourse);
                        context.SaveChanges();


                        return Json(new { status = true, message = "دوره جدیدایجاد شد" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false, message = "یک دوره قبلاً ایجاد شده است" }, JsonRequestBehavior.AllowGet);
                    }


                }
                else
                {

                    return Json(new { status = false, message = "شما دسترسی ندارید..." }, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult teachers()
        {

            var teachers = context.Table_teacher.ToList();
            ViewBag.teachers = teachers;

            return View();
        }

        [HttpGet]
        public ActionResult createTeachers(int id)
        {
            var teacherData = context.View_trainers.Where(x => x.pkID == id).Single();
            ViewBag.teacherData = teacherData;

            return View();
        }

        [HttpPost]
        public ActionResult createTeachers(int teacherID, string firstname, string lastname, HttpPostedFileBase teacherImgFile, string imgTeacherTitle, string teacherImgAlt)
        {
            string fileName = "";
            var teacher = context.Table_teacher.Where(x => x.pkID == teacherID).Single();
            var tImg = context.Tbl_img.Where(x => x.pkID == teacher.fkImg).Single();

            if (validation())
            {
                try
                {
                    if (teacherImgFile.ContentLength > 1000 && teacherImgFile.ContentLength < 10000000)
                    {
                        var prefixName = teacherImgFile.FileName.Split('.');
                        fileName = Path.GetFileName("ti_" + teacherID.ToString() + "." + prefixName[1]);
                        var path = Path.Combine(Server.MapPath("~/AdminAssets/img/ImgTeacher"), fileName);
                        teacherImgFile.SaveAs(path);
                        tImg.address = fileName;
                    }
                    else
                    {
                        return Json(new { status = false, message = "فایل آپلود شده میبایست بین 1 کیلوبایت تا 10 مگابایت باشد..." }, JsonRequestBehavior.AllowGet);
                    }

                    teacher.firstname = firstname;
                    teacher.lastname = lastname;
                    tImg.title = imgTeacherTitle;
                    tImg.alt = teacherImgAlt;
                    context.SaveChanges();
                    return Json(new { status = true, message = "ویرایش انجام شد...", reff = tImg.address }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { status = false, message = "validation failed!!!" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult createNewTeacher(string firstname, string lastname, HttpPostedFileBase teacherImgFile, string imgTeacherTitle, string teacherImgAlt)
        {
            string fileName = "";
            Table_teacher newTeacher = new Table_teacher();
            Tbl_img newTeacherImg = new Tbl_img();

            if (validation())
            {
                try
                {
                    if (teacherImgFile.ContentLength > 1000 && teacherImgFile.ContentLength < 10000000)
                    {
                        var prefixName = teacherImgFile.FileName.Split('.');
                        fileName = Path.GetFileName("ti_" + newTeacher.pkID.ToString() + "." + prefixName[1]);
                        var path = Path.Combine(Server.MapPath("~/AdminAssets/img/ImgTeacher"), fileName);
                        teacherImgFile.SaveAs(path);
                        newTeacherImg.address = fileName;
                    }
                    else
                    {
                        return Json(new { status = false, message = "فایل آپلود شده میبایست بین 1 کیلوبایت تا 10 مگابایت باشد..." }, JsonRequestBehavior.AllowGet);
                    }

                    newTeacher.firstname = firstname;
                    newTeacher.lastname = lastname;
                    newTeacherImg.title = imgTeacherTitle;
                    newTeacherImg.alt = teacherImgAlt;

                    context.Table_teacher.Add(newTeacher);
                    context.Tbl_img.Add(newTeacherImg);
                    context.SaveChanges();

                    return Json(new { status = true, message = "استاد جدید اضافه شد...", reff = newTeacherImg.address }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { status = false, message = "validation failed!!!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult deleteTeacher(int id)
        {
            try
            {
                var deletedTeacherID = context.Table_teacher.Where(X => X.pkID == id).Single();
                var deletedTeacherImg = context.Tbl_img.Where(x => x.pkID == deletedTeacherID.fkImg).Single();
                var path = Path.Combine(Server.MapPath("~/AdminAssets/img/ImgCourse"), deletedTeacherImg.address);
                if (deletedTeacherImg.address != "defultImg.jpg")
                {
                    if (System.IO.File.Exists(path))  // Check if the file exists before deleting it.
                    {
                        System.IO.File.Delete(path);   // Delete the file from the file system.
                    }
                }

                context.Table_teacher.Remove(deletedTeacherID);
                context.Tbl_img.Remove(deletedTeacherImg);
                context.SaveChanges();
                return Json(new { status = true, m = "استاد با موفقیت حذف شد" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, m = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }




        [HttpGet]
        public ActionResult article(int id)
        {
            articleMedel np = new articleMedel();
            if (id != 0)
            {
                Tbl_article article = context.Tbl_article.Where(x => x.pkID == id).Single();
                np.pkID = article.pkID;
                np.Title = article.Title;
                np.Description = article.Description;
                np.Keywords = article.Keywords;
                np.Author = article.fkAuthor;
                np.ImgTitle = article.ImageTitle;
                np.ImgAlt = article.ImageAlt;
                np.Content = article.Contents;
                np.ImgUrl = article.ImageSrc;

            }
            else
            {
                np.pkID = 0;
            }
            np.Authors = context.Table_teacher.ToList();
            return View(np);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult article(articleMedel p)
        {
            string fileName = "";
            string stringDate = DateTime.Now.ToString();
            stringDate = stringDate.Replace("/", "_");
            stringDate = stringDate.Replace(":", "_");
            stringDate = stringDate.Replace(" ", "_");

            if (p.pkID != 0)      //it means that the article exists and we want to change it...
            {
                var existedArticle = context.Tbl_article.Where(x => x.pkID == p.pkID).Single();
                existedArticle.Title = p.Title;
                existedArticle.Description = p.Description;
                existedArticle.Keywords = p.Keywords;
                existedArticle.fkAuthor = p.Author;
                existedArticle.ImageTitle = p.ImgTitle;
                existedArticle.ImageAlt = p.ImgAlt;
                existedArticle.Contents = p.Content;
                existedArticle.DateModified = DateTime.Now.ToString();

                if (p.ImageArticle == null)
                {
                    p.ImgUrl = existedArticle.ImageSrc;
                }
                else
                {
                    
                    if (p.ImageArticle.ContentLength > 10000 && p.ImageArticle.ContentLength < 10000000)
                    {
                        var prefixName = p.ImageArticle.FileName.Split('.');
                        fileName = Path.GetFileName("ci_" + stringDate + "." + prefixName[1]);
                        var path = Path.Combine(Server.MapPath("~/AdminAssets/img/ImgArticle"), fileName);
                        p.ImageArticle.SaveAs(path);

                        existedArticle.ImageSrc = fileName;
                        p.ImgUrl = existedArticle.ImageSrc;

                    }
                    else
                    {
                        p.message = "فایل آپلود شده میبایست بین 10 کیلوبایت تا 10 مگابایت باشد...";
                        p.status = false;
                    }
                }
                p.status = true;
            }
            else
            {
                Tbl_article editp = new Tbl_article();

                editp.Title = p.Title;
                editp.Description = p.Description;
                editp.Keywords = p.Keywords;
                editp.fkAuthor=p.Author;
                editp.Contents=p.Content;
                editp.ImageAlt=p.ImgAlt;
                editp.ImageTitle=p.ImgTitle;
                editp.DateModified = DateTime.Now.ToString();

                if (p.ImageArticle != null)
                {
                    if (p.ImageArticle.ContentLength > 10000 && p.ImageArticle.ContentLength < 10000000)
                    {
                        var prefixName = p.ImageArticle.FileName.Split('.');
                        fileName = Path.GetFileName("ci_" + stringDate + "." + prefixName[1]);
                        var path = Path.Combine(Server.MapPath("~/AdminAssets/img/ImgArticle"), fileName);
                        p.ImageArticle.SaveAs(path);
                        editp.ImageSrc = fileName;
                        p.ImgUrl = editp.ImageSrc;

                        context.Tbl_article.Add(editp);
                        p.message = "مقاله با موفقیت ایجاد شد";
                        p.status=true;
                    }
                    else
                    {
                        p.message = "فایل آپلود شده میبایست بین 10 کیلوبایت تا 10 مگابایت باشد...";
                        p.status = false;
                    }
                }
                else
                {
                    p.message = "عکس انتخاب نشده است...";
                    p.status = false;
                }                
            }

            context.SaveChanges();
            p.Authors = context.Table_teacher.ToList();
            return View(p);

        }

        public ActionResult articleList()
        {
            List<articleListModel> articleList = new List<articleListModel>();
            articleList = context.Tbl_article.Select(x => new articleListModel { pkID = x.pkID, Title = x.Title }).ToList();

            return View(articleList);
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

            else
            {
                return false;
            }
        }
    }


}