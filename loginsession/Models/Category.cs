using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace loginsession.Models
{
    public class Category
    {

        public virtual int CategoryId { get; set; }
        //    public virtual int SubId { get; set; }
        public virtual string Title { get; set; }
        public virtual List<Sub> Subs { get; set; }
    }
}