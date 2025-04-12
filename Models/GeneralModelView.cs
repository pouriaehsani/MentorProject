using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mentorproject.Models
{
    public class GeneralModelView
    {
        public Table_pages Pages { get; set; }
        public List<View_courses> Courses { get; set; }
        public List<View_trainers> Teachers { get; set; }
    }
}