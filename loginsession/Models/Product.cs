using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace loginsession.Models
{
    public class Product
    {
        public virtual int ProductId { get; set; }
        // public virtual int CategoryId { get; set; }
        public virtual int SubSubId { get; set; }
        //public virtual int SubId { get; set; }
        public virtual string Title { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string ProductArtUrl { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string Description { get; set; }

        // public virtual Category Category{ get; set; }
        //public virtual SubSub SubSub { get; set; }
        //public virtual Sub Sub { get; set; }

    }
}