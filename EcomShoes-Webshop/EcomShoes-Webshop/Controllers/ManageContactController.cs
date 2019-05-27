using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using EcomShoes_Webshop.Models;
using System.Transactions;


namespace EcomShoes_Webshop.Controllers
{
    public class ManageContactController : Controller
    {
        K23T3aEntities db = new K23T3aEntities();
        //
        // GET: /ManageContact/
        public ActionResult Index()
        {
            var model = db.ContactDetails;
            return View(model.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact tmp = db.Contacts.Find(id);
            if (tmp == null)
            {
                return HttpNotFound();
            }
            return View(tmp);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactDetail tmp = db.ContactDetails.Find(id);
            if (tmp == null)
            {
                return HttpNotFound();
            }
            return View(tmp);
        }
        [HttpPost]

        public ActionResult Edit(ContactDetail ct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(ct).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {


                }


            }
            return View(ct);
        }



    }
}
