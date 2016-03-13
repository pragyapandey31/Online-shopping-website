using loginsession.Models;
using loginsession.ViewModels;
using System;
using System.Collections.Generic;
//using System.Data.Linq;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;

namespace initial2.Controllers
{
    public class ShoppingCartController : Controller
    {

        ShoppingStoreDB storeDB = new ShoppingStoreDB();

        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

       
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
                  return View(viewModel);
        }
       
       
       
        public void AddToCart(int idp)
        {
           
            var addedProduct = storeDB.Products
                .Single(p => p.ProductId == idp);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);

            
        }
        
      
      
        public ActionResult RemoveFromCart1(int id)
        {
            
            var cart = ShoppingCart.GetCart(this.HttpContext);

          
          
          string albumName = storeDB.Carts.SingleOrDefault(item => item.RecordId == id).Product.Title;

             var q = from s in storeDB.Carts
                        where s.RecordId == id
                        select s;
            
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            ViewBag.vm = viewModel;
           
            return RedirectToAction("CartView", "StoreHome",getgvl());

        }
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
        
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
        public ActionResult r(int id)
        {
            return View();
        }
        public ActionResult Buy()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            if (Request.IsAuthenticated)
            {
return View("~/Views/StoreHome/Buy.cshtml", getgvl());
            }
            else {
                return View("~/Views/Account/LogIn.cshtml", getgvl());
            }

            }
        }
}