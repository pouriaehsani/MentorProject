using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mentorproject.Models
{
    public class pageListData
    {
        public int pkID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Action { get; set; }
        public bool? Status { get; set; }
        public string Message { get; set; }
    }
}