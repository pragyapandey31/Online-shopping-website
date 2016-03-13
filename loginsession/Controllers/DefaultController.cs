using loginsession.Models;
using loginsession.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace loginsession.Controllers
{
    public class DefaultController : Controller
    {
        private ShoppingStoreDB db = new ShoppingStoreDB();

        private GroupViewModel getgvl()
        {

            GroupViewModel groupviewmodel = new GroupViewModel();
            GroupBusinessLayer gbl = new GroupBusinessLayer();
            groupviewmodel.Products = gbl.GetProducts();
            groupviewmodel.Categories = gbl.GetCategory();
            groupviewmodel.Subs = gbl.GetSub();
            groupviewmodel.SubSubs = gbl.GetSubSub();
            return groupviewmodel;

        }
        // GET: Default
        public ActionResult Index()
        {
            return View(getgvl());
        }
        public ActionResult Index1()
        {
            return View();
        }
    }
}