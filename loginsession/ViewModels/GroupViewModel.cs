using loginsession.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace loginsession.ViewModels
{
    public class GroupViewModel
    {

        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Sub> Subs { get; set; }
        public IEnumerable<SubSub> SubSubs { get; set; }

    }
}