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
        public ActionResult Search(string text)
        {
            var itemsz = db.Products.Where(x => x.ProductName.ToLower().Contains(text.ToLower())).ToList();

            if (itemsz.Count() > 0)
            {
                //ViewBag.Message = "";
            }
            else
            {
                ViewBag.Message = "No Item found";

            }
            ViewData["Item"] = itemsz;
            return View(itemsz);
        }
    }
}