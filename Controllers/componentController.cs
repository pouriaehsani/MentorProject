using mentorproject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace mentorproject.Controllers
{
    public class componentController : Controller
    {
        educationDb context = new educationDb();

        [ChildActionOnly]
        public ActionResult _menu(string pn)
        {
            MenuModel model = new MenuModel();
            model.Languages = context.Tbl_language.Where(x => x.status == true).ToList();

            if (Request.Cookies["langID"] == null || Request.Cookies["langID"].Value == "")
            {

                Response.Cookies["langID"].Value = "1";
            }

            int langID = int.Parse(Request.Cookies["langID"].Value);    //Request.Cookies["langID"].Value
            ViewBag.page = pn;

            model.Pages = context.Table_pages.Where(x => x.fkLangID == langID).ToList();



            return PartialView(model);
        }

        public ActionResult _Banners(int id)
        {
            Tbl_Banners banner = new Tbl_Banners();
            banner = context.Tbl_Banners.Where(x => x.pkID == id).Single();
            return PartialView(banner);
        }

        public ActionResult _About(int id)
        {
            Tble_About about = new Tble_About();
            about = context.Tble_About.Where(x => x.pkID == id).Single();
            return PartialView(about);
        }

        public ActionResult _Courses() 
        {
            GeneralModelView model = new GeneralModelView();
            model.Courses=context.View_courses.ToList();
        
            return PartialView(model);
        }
        public ActionResult _Teachers()
        {
            GeneralModelView model = new GeneralModelView();
            model.Teachers = context.View_trainers.ToList();

            return PartialView(model);
        }
        public ActionResult _Articles()
        {
            ArticleHomeViewModel model = new ArticleHomeViewModel();
            model.article=context.View_Article.ToList();

            return PartialView(model);
        }
    }


}