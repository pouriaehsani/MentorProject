using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mentorproject.Models
{
    public class articleMedel
    {
        public int pkID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Table_teacher> Authors { get; set; }
        public int Author { get; set; }
        public HttpPostedFileBase ImageArticle { get; set; }
        public string Keywords { get; set; }
        public string ImgTitle { get; set; }
        public string ImgAlt { get; set; }
        public string Content { get; set; }
        public  string message { get; set; }
        public bool? status { get; set; }
        public string ImgUrl { get; set; }
    }
}