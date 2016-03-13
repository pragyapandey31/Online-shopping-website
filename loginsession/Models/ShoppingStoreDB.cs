using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace loginsession.Models
{
    public class ShoppingStoreDB : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ShoppingStoreDB() : base("name=ShoppingStoreDB")
        {
        }

        public System.Data.Entity.DbSet<loginsession.Models.Product> Products { get; set; }
        public System.Data.Entity.DbSet<loginsession.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<loginsession.Models.Sub> Subs { get; set; }

        public System.Data.Entity.DbSet<loginsession.Models.SubSub> SubSubs { get; set; }

        public DbSet<Cart>
Carts
        { get; set; }
        public DbSet<Order> Orders
        { get; set; }
        public DbSet<OrderDetail>
OrderDetails
        { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();



        }




    }
}
