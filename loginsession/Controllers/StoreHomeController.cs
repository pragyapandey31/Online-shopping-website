using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using loginsession.Models;
using loginsession.ViewModels;

namespace loginsession.Controllers
{
    public class StoreHomeController : Controller
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
        // GET: StoreHome
        public ActionResult error()
        {

            return View(getgvl());
        }

        public ActionResult Index()
        {
        
            return View(getgvl());
        }

        public ActionResult Index2()
        {

            return View(getgvl());
        }

        public ActionResult Re() {
            string url = this.Request.UrlReferrer.AbsolutePath;
            return Redirect(url);
        }
        public ActionResult SubView(int ids) { ViewBag.idss = ids;
            return View(getgvl());
        }


        [HttpPost]
        public ActionResult Complete(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                
               
                
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Save Order
                   db.Orders.Add(order);
                    db.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);
                var i = order.OrderId;
                orderdetailsgroupview od = new orderdetailsgroupview();
                GroupBusinessLayer g = new GroupBusinessLayer();
                od.Orderdetails = g.Getod();
                
                var o = from a in od.Orderdetails where a.OrderId.Equals(i) select a;
                var q= from a in od.Orderdetails where a.OrderId.Equals(i) select a.Quantity;
                GroupViewModel gl = getgvl();
                // int a = (int) o;
                foreach (var p in o)
                {
                    g.GetProd(p.Quantity, p.ProductId);
                } 
             //  var r = from f in gl.Products where f.ProductId.Equals(o) select f;
                
               // foreach (Product p in r)
                //{
                  //  p.Quantity = p.Quantity - ;
                //}
                
                return View("Complete",getgvl());
                       // new { id = order.OrderId });
                
            }
            catch
            {
                //Invalid - redisplay with errors
                return View("error",getgvl());
            }
        }

        public ActionResult CartView()
        {
            GroupViewModel groupviewmodel = new GroupViewModel();
            GroupBusinessLayer gbl = new GroupBusinessLayer();
            groupviewmodel.Products = gbl.GetProducts();
            groupviewmodel.Categories = gbl.GetCategory();
            groupviewmodel.Subs = gbl.GetSub();
            groupviewmodel.SubSubs = gbl.GetSubSub();

            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            ViewBag.car = "yes";
            ViewBag.vm = viewModel;
            // Return the view
          //  string separator = "/";

           // string url = this.Request.UrlReferrer.AbsolutePath;
            // string act = this.Request.UrlReferrer.AbsolutePath;
         // int index = returnUrl.LastIndexOf(separator);
           //string callerURL = returnUrl.Substring(index + separator.Length);
            // return View(callerURL, groupviewmodel);
            // return Redirect(returnUrl);
            return View(getgvl());
        }






        public ActionResult ProductView(int id1)
        {
            ViewBag.i = id1;
            GroupViewModel groupviewmodel = new GroupViewModel();
            GroupBusinessLayer gbl = new GroupBusinessLayer();
            groupviewmodel.Products = gbl.GetProducts();
            groupviewmodel.Categories = gbl.GetCategory();
            groupviewmodel.Subs = gbl.GetSub();
            groupviewmodel.SubSubs = gbl.GetSubSub();
            return View(groupviewmodel);
        }

        public ActionResult ProductPage(int idp)
        {
            ViewBag.idp = idp;
            GroupViewModel groupviewmodel = new GroupViewModel();
            GroupBusinessLayer gbl = new GroupBusinessLayer();
            groupviewmodel.Products = gbl.GetProducts();
            groupviewmodel.Categories = gbl.GetCategory();
            groupviewmodel.Subs = gbl.GetSub();
            groupviewmodel.SubSubs = gbl.GetSubSub();
            return View(groupviewmodel);
        }

        // GET: StoreManager/Details/5


        public ActionResult Disp(string search)
        {
            var gv = getgvl();
            var prods = gv.Products;
            if (!String.IsNullOrEmpty(search))
            {
                //ViewBag.products = prods.Where(s => s.Title.Contains(search))||prods.Where(s => s.Title.Contains(search));
                ViewBag.products = from pr in prods where pr.Title.ToUpper().Contains(search.ToUpper()) select pr;
                //  sub = products.Where(s => s.Title.Contains(search));
            }
            // return View(products.ToList());

            return View(gv);





        }











    }
}