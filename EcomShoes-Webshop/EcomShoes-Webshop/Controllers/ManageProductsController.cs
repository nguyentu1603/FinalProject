﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcomShoes_Webshop.Models;
using System.Transactions;

namespace EcomShoes_Webshop.Controllers
{
    public class ManageProductsController : Controller
    {
        private K23T3aEntities db = new K23T3aEntities();
        // GET: /ManageProducts/
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }
        public ActionResult Image(string id)
        {
            var path = Server.MapPath("~/Content/Images"); // đường dẫn chứa tệp hình ảnh của sản phẩm
            path = System.IO.Path.Combine(path, id); // tên hình ảnh là id của mã sản phẩm
            return File(path, "image/*");
        }
        //Kiểm tra mã sản phẩm đã tồn tại trong database: Nhung, Tú
        private void checkProductCodeCreate(Product model) {
            // x.ProductCode: giá trị trong database; model.ProductCode: giá trị trong view
            var isProductCodeExist = db.Products.Any(x => x.ProductCode == model.ProductCode);//kiểm tra sự tồn tại
            if (isProductCodeExist) {
                ModelState.AddModelError("ProductCode", RangBuoc.codeProduct_Exist);
            }
        }
        private void checkProductCodeEdit(Product model)
        {
            var isProductCodeExist = db.Products.Any(x => x.ProductCode == model.ProductCode && x.ID != model.ID);
            if (isProductCodeExist)
            {
                ModelState.AddModelError("ProductCode", RangBuoc.codeProduct_Exist);
            }
        }
        private void checkQuantityAndStatus(Product model) {
            if (model.Quantity>0 && model.Status=="DEACTIVE") {
                ModelState.AddModelError("Status", RangBuoc.deactive_Product);

            }
            else if (model.Quantity <= 0 && model.Status == "ACTIVE") {
                ModelState.AddModelError("Status",RangBuoc.active_Product);
            }
        }
        private void checkProductCode(Product model)
        {
            if (model.ProductCode.Length < 2 && model.ProductCode.Length > 15)
            {
                ModelState.AddModelError("Status","Vui lòng nhập lại đúng định dạng");
            }

            
        }
        // GET: /ManageProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            
            if (product == null)
            {
                return HttpNotFound();
            }
 
            return View(product);
        }

        // GET: /ManageProducts/Create
        public ActionResult Create()
        {
     
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "NameCategory");
            //Tạo viewbag hiển thị giá trị mặc định cho Status
            var statusItems = new[]
            {
                new { Id = "DEACTIVE", Name = "Hết hàng" },
                new { Id = "ACTIVE", Name = "Còn hàng" },
            };
            ViewBag.Status = new SelectList(statusItems, "Id", "Name");
            return View();
        }

        // POST: /ManageProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            checkProductCode(product);
            checkPrice(product); //Kiểm tra giá trị giá gốc và giá bán phải lớn hơn 0.
            checkProductCodeCreate(product); //Kiểm tra mã sản phẩm đã tồn tại hay chưa.
            checkQuantityAndStatus(product); //Kiểm tra mốc số lượng và tình trạng
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    //thêm model vào database
                    product.CreatedDate = DateTime.Now;
                    product.UpdateDate = DateTime.Now;
                    db.Products.Add(product);
                    db.SaveChanges();

                    //gán hình vào file app_data

                    var path = Server.MapPath("~/Content/Images");
                    path = System.IO.Path.Combine(path, product.ID.ToString()); //file ảnh sản phẩm sẽ có tên là id của sẩn phẩm.
                    Request.Files["Image"].SaveAs(path);
                    product.ImageURL = path; //ImageURL sẽ lưu đường dẫn tới file ảnh.

                    //All done successfully       
                    db.SaveChanges();
                    scope.Complete();
                    return RedirectToAction("Index");

                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "NameCategory", product.CategoryID);
            var statusItems = new[]
            {
                new { Id = "DEACTIVE", Name = "Hết hàng" },
                new { Id = "ACTIVE", Name = "Còn hàng" },
            };
            ViewBag.Status = new SelectList(statusItems, "Id", "Name");

            return View(product);
        }
        private void checkPrice(Product model)
        {
            if (model.SalePrice <= 0 && model.OriginalPrice <= 0)
            {
                ModelState.AddModelError("SalePrice", RangBuoc.price_Less_0);
                ModelState.AddModelError("OriginalPrice", RangBuoc.price_Less_0);
            }
        }

        // GET: /ManageProducts/Edit/5
        public ActionResult Edit(int id)
        {
            var statusItems = new[]
            {
                new { Id = "DEACTIVE", Name = "Hết hàng" },
                new { Id = "ACTIVE", Name = "Còn hàng" },
            };
            ViewBag.Status = new SelectList(statusItems, "Id", "Name");
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "NameCategory", product.CategoryID);
            return View(product);
        }

        // POST: /ManageProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            checkPrice(product);
            checkProductCodeEdit(product);
            checkQuantityAndStatus(product);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    //save edit of product
                    product.UpdateDate = DateTime.Now;
                    db.Entry(product).State = EntityState.Modified;

                    db.SaveChanges();
                    //add file to app_data
                    var path = Server.MapPath("~/Content/Images");
                    path = System.IO.Path.Combine(path, product.ID.ToString());
                    if (Request.Files["Image"].ContentLength != 0)
                    {
                        Request.Files["Image"].SaveAs(path);
                    }
                    product.ImageURL = path; //gán đường dẫn vào ImageURL
                    //All done successfully
                    db.SaveChanges(); // save lại đường dẫn
                    scope.Complete();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "NameCategory", product.CategoryID);
            var statusItems = new[]
            {
                new { Id = "DEACTIVE", Name = "Hết hàng" },
                new { Id = "ACTIVE", Name = "Còn hàng" },
            };
            ViewBag.Status = new SelectList(statusItems, "Id", "Name");
            return View(product);
        }

        // GET: /ManageProducts/Delete/5
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: /ManageProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            var inOrder = db.OrderDetails.Where(x => x.ProductID == id);
            if (inOrder.Count() != 0)
            {
                TempData["msg"] = "<script>alert('Sản phẩm hiện tại đang tồn tại trong giỏ hàng!!! Vui lòng thực hiện sau khi sản phẩm này không còn có trong đơn hàng nào.');</script>";
               
            }
            else
            {
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
