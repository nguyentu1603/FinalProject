using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcomShoes_Webshop.Models;

namespace EcomShoes_Webshop.Controllers
{
    public class ManageOrdersController : Controller
    {
        private K23T3aEntities db = new K23T3aEntities();

        // GET: /ManageOrders/
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }
        public ActionResult Image(string id)
        {
            var path = Server.MapPath("~/App_Data"); // đường dẫn chứa tệp hình ảnh của sản phẩm
            path = System.IO.Path.Combine(path, id); // tên hình ảnh là id của mã sản phẩm
            return File(path, "image/*");
        }

        public ActionResult Edit(int? id)
        {
            var statusItems = new[]
            {
                new { Id = "0", Name = "Đang chờ xác nhận" },
                new { Id = "1", Name = "Đã nhận" },
                new { Id = "2", Name = "Đang xử lý và đóng gói "},
                new { Id = "3", Name = "Đang trên đường giao hàng" },
                new { Id = "3", Name = "Đã nhận hàng" }
            };
            ViewBag.Status = new SelectList(statusItems, "Id", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

         //POST: /ManageOrders/Edit/5
         //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
         //more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,CreatedDate,ContactName,ContactAddress,ContactPhone,ContactEmail,ContactReceiverName,ContactReceiverAddress,ContactReceiverPhone,ContactReceiverEmail,Note,OrderCode,Status,EmployeeName,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
             var statusItems = new[]
            {
                new { Id = "0", Name = "Đang chờ xác nhận" },
                new { Id = "1", Name = "Đã nhận" },
                new { Id = "2", Name = "Đang xử lý và đóng gói "},
                new { Id = "3", Name = "Đang trên đường giao hàng" },
                new { Id = "3", Name = "Đã nhận hàng" }
            };
        }

        // GET: /ManageOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: /ManageOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult OrderDetails(int id)
        {
                OrderDetail orderdetail = db.OrderDetails.Find(id);          
                return View(orderdetail);
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
