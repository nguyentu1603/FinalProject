using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcomShoes_Webshop.Models;
namespace EcomShoes_Webshop.Controllers
{
    public class HomeController : Controller
    {
        private K23T3aEntities db = new K23T3aEntities();
        public ActionResult Index()
        {
            return View(db.Products.ToList());

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}