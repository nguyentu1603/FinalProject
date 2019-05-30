using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcomShoes_Webshop.Models;
using EcomShoes_Webshop.Controllers;

namespace EcomShoes_Webshop.Controllers
{
    public class ShoppingCartController : Controller
    {
        K23T3aEntities db = new K23T3aEntities();
        //
        // GET: /ShoppingCart/
        public List<Cart> GetCart()
        {
            List<Cart> lstcart = Session["cart"] as List<Cart>;
            if (lstcart == null)
            {
                // Neu gio hang chua ton tai thi minh tien hang tao gio hang
                lstcart = new List<Cart>();
                Session["Cart"] = lstcart;
            }
            return lstcart;
        }
        // Them gio hang
        public ActionResult addCart(int iMaSP, int txtSoLuong)
        {

            Product product = db.Products.SingleOrDefault(n => n.ID == iMaSP);

            if (txtSoLuong <= 0 || txtSoLuong.ToString().Trim().Equals(null))
            {
                txtSoLuong = 1;
            }

            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // Lấy ra session giỏ hàng
            List<Cart> lstCart = GetCart();
            // Kiểm tra sản phẩm này đã tồn tại trong session[giohang] chưa
            Cart sp = lstCart.Find(n => n.iMaSP == iMaSP);

            if (sp == null)
            {

                sp = new Cart(iMaSP);
                sp.isoLuong = txtSoLuong;
                lstCart.Add(sp);
                return RedirectToAction("product_detail", "products", new { product.ID });
            }
            else
            {

                sp.isoLuong = sp.isoLuong + txtSoLuong;

                return RedirectToAction("product_detail", "products", new { product.ID });
            }


        }
        public ActionResult ShoppingCart()
        {


            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Cart> lstCart = GetCart();
            return View(lstCart);
        }
        // Cap nhat gio hang
        public ActionResult UpdateShoppingCart(int iMaSP, FormCollection f)
        {
            // Kiem tra ma san pham
            Product product = db.Products.SingleOrDefault(n => n.ID == iMaSP);
            // Neu lay sai ma san pham thi tra ve loi
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // lay gio hang ra tu session
            List<Cart> lstCart = GetCart();
            // Kiem tra san pham co ton tai tron session
            Cart sp = lstCart.SingleOrDefault(n => n.iMaSP == iMaSP);
            // Neu ton tai thi cho sua so luong
            if (sp != null)
            {
                sp.isoLuong = int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("ShoppingCart");

        }
        // Xoa gio hang
        public ActionResult DeleteShoppingCart(int iMaSP)
        {
            // Kiem tra ma san pham
            Product product = db.Products.SingleOrDefault(n => n.ID == iMaSP);
            // Neu lay sai ma san pham thi tra ve loi
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // lay gio hang ra tu session
            List<Cart> lstCart = GetCart();
            // Kiem tra san pham co ton tai tron session
            Cart sp = lstCart.SingleOrDefault(n => n.iMaSP == iMaSP);
            // Neu ton tai thi cho sua so luong
            if (sp != null)
            {
                lstCart.RemoveAll(n => n.iMaSP == iMaSP);
            }
            if (lstCart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("ShoppingCart");
        }
        // Tinh tong so luong va tong tien
        private int SumAmount()
        {
            int iSumAmount = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                iSumAmount = lstCart.Sum(n => n.isoLuong);

            }
            return iSumAmount;
        }
        // Tinh tong thanh tien
        private double SumPrice()
        {
            double dSumPrice = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                dSumPrice += lstCart.Sum(n => n.thanhTien);
            }
            return dSumPrice;
        }
        public ActionResult Index()
        {

            ViewBag.Message = "Your application description page.";
            return View();
        }
        // Tao partial gio hang
        public ActionResult PartialSoLuong()
        {
            if (SumAmount() == 0)
            {
                ViewBag.SumAmount = 0;
                ViewBag.SumPrice = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = SumAmount();
            ViewBag.TongTien = SumPrice();
            return PartialView();
        }
        // Xay dunng view cho nguoi dung chinh sua gio hang
        public ActionResult EditShoppingCart()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Cart> lstCart = GetCart();
            return View(lstCart);
        }
        public ActionResult PartialTotal()
        {
            if (SumAmount() == 0)
            {
                ViewBag.SumAmount = 0;
                ViewBag.SumPrice = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = SumAmount();
            ViewBag.TongTien = SumPrice();
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DatHang()
        {
            // Kiem tra don dat hang
            if (Session["GioHang"] == null)
            {
                RedirectToAction("index", "Home");
            }

            // Them don hang
            Order order = new Order();
            List<Cart> cart = GetCart();
            order.CreatedDate = DateTime.Now;
            db.Orders.Add(order);
            db.SaveChanges();
            // Them chi tiet don hang
            foreach (var item in cart)
            {
                OrderDetail order_Detail = new OrderDetail();
                order_Detail.OrderID = order.id;
                order_Detail.ProductID = item.iMaSP;
                order_Detail.Quantity = (int)item.isoLuong;
                db.OrderDetails.Add(order_Detail);
            }
            db.SaveChanges();
            return RedirectToAction("index", "Home");
        }
	}
}