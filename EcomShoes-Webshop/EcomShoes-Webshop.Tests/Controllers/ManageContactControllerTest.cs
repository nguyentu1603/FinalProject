    
using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcomShoes_Webshop.Controllers;
using EcomShoes_Webshop.Models;
using System.Collections.Generic;
using System.Transactions;


namespace EcomShoes_Webshop.Tests.Controllers
{
    [TestClass]
    public class ManageContactControllerTest
    {
        [TestMethod]
        public void TestContactIndex()
        {
            var db = new K23T3aEntities();
            var controller = new ManageContactController();

            var result = controller.Index();
            var view = result as ViewResult;
            Assert.IsNotNull(view);
            var model = view.Model as List<ContactDetail>;
            Assert.IsNotNull(model);
            Assert.AreEqual(db.ContactDetails.Count(), model.Count);
        }
    }
}
