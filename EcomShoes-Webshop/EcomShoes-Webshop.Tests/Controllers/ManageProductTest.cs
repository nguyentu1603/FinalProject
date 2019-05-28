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
           
        }
        
        [TestMethod]
        public void TestEditG()
        {
            var db = new K23T3aEntities();
            var item = db.Products.First();
            var controller = new ManageProductsController();

            var result = controller.Edit(0);
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));

            result = controller.Edit(item.ID);
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.Model as Product;
            Assert.IsNotNull(model);
            Assert.AreEqual(item.ID, model.ID);
            Assert.AreEqual(item.CategoryID, model.CategoryID);
            Assert.AreEqual(item.CreatedDate, model.CreatedDate);
            Assert.AreEqual(item.OriginalPrice, model.OriginalPrice);
            Assert.AreEqual(item.SalePrice ,model.SalePrice);
            Assert.AreEqual(item.Description, model.Description);
            Assert.AreEqual(item.Size, model.Size);
            Assert.AreEqual(item.ProductCode, model.ProductCode);
            Assert.AreEqual(item.Status, model.Status);
        }
      
        [TestMethod]
        public void TestDelete()
        {
            ManageProductsController controller = new ManageProductsController();
            ViewResult result = controller.Delete(0) as ViewResult;   
            Assert.IsNull(result);
        }
        [TestMethod]
        public void TestDetails()
        {
            var db = new K23T3aEntities();
            var item = db.Products.First();
            var controller = new ManageProductsController();

            var result = controller.Details(item.ID);
            var view = result as ViewResult;
            Assert.IsNotNull(view);

            var model = view.Model as Product;
            Assert.IsNotNull(model);
            Assert.AreEqual(item.ID, model.ID);

            result = controller.Details(0);
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }
    }
    
}
