using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace loginsession.Models
{
    public class GroupBusinessLayer
    {
        ShoppingStoreDB os = new ShoppingStoreDB();
        public void GetProd(int q,int i)
        {
            //OnlineStoreDB os = new OnlineStoreDB();
            // return os.Products.ToList();
            System.Data.Entity.DbSet<loginsession.Models.Product> p = (System.Data.Entity.DbSet < loginsession.Models.Product > )from a in os.Products where a.ProductId.Equals(i) select a;
          //  var u="UPDATE os.Products "
         // List<os.Products> p=from a in os.Products where a.ProductId.Equals(i) select a;
            foreach (Product r in p)
            {
                r.Quantity = r.Quantity - q;
               
                
                

            }
            
            os.SaveChanges();

        }

        public List<Product> GetProducts()
        {
            //OnlineStoreDB os = new OnlineStoreDB();
            return os.Products.ToList();

        }
        public List<Category> GetCategory()
        {
            //OnlineStoreDB os = new OnlineStoreDB();
            return os.Categories.ToList();

        }
        public List<Sub> GetSub()
        {
            //OnlineStoreDB os = new OnlineStoreDB();
            return os.Subs.ToList();

        }
        public List<SubSub> GetSubSub()
        {
            //OnlineStoreDB os = new OnlineStoreDB();
            return os.SubSubs.ToList();

        }

        public List<OrderDetail> Getod()
        {
            //OnlineStoreDB os = new OnlineStoreDB();
            return os.OrderDetails.ToList();

        }

       
    }
}