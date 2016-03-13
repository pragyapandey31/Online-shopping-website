using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace loginsession.Models
{
    public class SubSub
    {
        public virtual int SubSubId { get; set; }
        public virtual string Title { get; set; }
        public virtual int SubId { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}