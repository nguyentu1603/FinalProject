using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcomShoes_Webshop.Models;

namespace EcomShoes_Webshop.Controllers
{
    public class productsController : Controller
    {
        private K23T3aEntities db = new K23T3aEntities();
        //
        // GET: /products/
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }
        public ViewResult product_detail(int ID = 0)
        {
            // Tra về đôi tượng với điều kiện
            Product product = db.Products.SingleOrDefault(n => n.ID == ID);
            if (product == null)
            {
                // Trả về trang báo lỗi
                Response.StatusCode = 404;
                return null;
            }
            return View(product);
        }
	}
}