using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using loginsession.Models;

namespace loginsession.ViewModels
{
    public class orderdetailsgroupview
    {
        public IEnumerable<OrderDetail> Orderdetails { get; set; }
       // public IEnumerable<Category> Categories { get; set; }
    }
}