//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mentorproject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class View_Article
    {
        public int pkID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int fkAuthor { get; set; }
        public string Keywords { get; set; }
        public string ImageTitle { get; set; }
        public string ImageAlt { get; set; }
        public string Contents { get; set; }
        public string ImageSrc { get; set; }
        public string DateModified { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address { get; set; }
    }
}
