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
            var path = Server.MapPath("~/App_Data");
            path = System.IO.Path.Combine(path, id);
            return File(path, "image/*");
        }
        private void checkPrice(Product model)
        {
            if (model.OriginalPrice <= 0 && model.SalePrice <= 0)
            {
                ModelState.AddModelError("SalePrice", RangBuoc.price_Less_0);
                ModelState.AddModelError("OriginalPrice", RangBuoc.price_Less_0);
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
            checkPrice(product);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    //add model to database
                    product.CreatedDate = DateTime.Now;
                    product.UpdateDate = DateTime.Now;
                    db.Products.Add(product);
                    db.SaveChanges();
                    //add file to app_data
                    var path = Server.MapPath("~/App_Data");
                    path = System.IO.Path.Combine(path, product.ID.ToString());
                    Request.Files["Image"].SaveAs(path);
                    //All done successfully
                    scope.Complete();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "NameCategory", product.CategoryID);
            return View(product);
        }


        // GET: /ManageProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    product.UpdateDate = DateTime.Now;
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    var path = Server.MapPath("~/App_Data");
                    path = System.IO.Path.Combine(path, product.ID.ToString());
                    Request.Files["Image"].SaveAs(path);
                    //All done successfully
                    scope.Complete();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "NameCategory", product.CategoryID);
            return View(product);
        }

        // GET: /ManageProducts/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: /ManageProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
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
