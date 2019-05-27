using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using EcomShoes_Webshop.Models;
using EcomShoes_Webshop.Tests.Controllers;
using EcomShoes_Webshop.Tests;
using System.Web.Mvc;
using EcomShoes_Webshop.Controllers;
using System.Collections.Generic;
using System.Transactions;

namespace EcomShoes_Webshop.Tests.Controllers
{
    [TestClass]
    public class ManageProductTest
    {
        [TestMethod]
        public void TestIndex()
        {
            var db = new K23T3aEntities();
            var controller = new ManageProductsController();
            var result = controller.Index();
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.Model as List<Product>;
            Assert.IsNotNull(model);
            Assert.AreEqual(db.Products.Count(), model.Count);
        }
        [TestMethod]
        public void TestCreateG()
        {
            var controller = new ManageProductsController();
            var result = controller.Create() as ViewResult;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestCreateP()
        {
            var model = new Product
            {
                //Name = "Socola",
                //Topping = "Tran chau",
                //Price = 0

            };
            var db = new ManageProductsController();
            var controller = new ManageProductsController();


            using (var scope = new TransactionScope())
            {
                var result = controller.Create(model);
                var view = result as ViewResult;
                Assert.IsNotNull(view);
                Assert.IsInstanceOfType(view.Model, typeof(Product));


                //    model.Price = 26000;
                //    controller = new ManageProductsController();

                //    result = controller.Create(model);
                //    var redirect = result as RedirectToRouteResult;

                //    Assert.IsNotNull(redirect);
                //    Assert.AreEqual("Index", redirect.RouteValues["action"]);
                //    var item = db.
                //    Assert.IsNotNull(item);
                //    Assert.AreEqual(model.Name, item.Name);
                //    Assert.AreEqual(model.Price, item.Price);
                //    Assert.AreEqual(model.Topping, item.Topping);
                //}
            }
        }
    }
}
