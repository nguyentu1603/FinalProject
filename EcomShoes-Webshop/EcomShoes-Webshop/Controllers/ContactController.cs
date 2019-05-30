
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcomShoes_Webshop.Models;
using System.Transactions;

namespace EcomShoes_Webshop.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        K23T3aEntities db = new K23T3aEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(Contact contact, string message, string Feedback_Detail)
        {
            if (ModelState.IsValid)
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        Convert.ToInt32(contact.Phone);
                    }
                    catch (Exception)
                    {

                        return View(contact);
                    }
                    try
                    {
                       
                        contact.Status = 0;
                        db.Contacts.Add(contact);
                        db.SaveChanges();

                        var detail = new ContactDetail();
                        detail.ContactID = contact.ContactID;
                        detail.CreatedDate = DateTime.Now;
                        detail.Message = message;
                        detail.EmployeeName = "";
                  

                        db.ContactDetails.Add(detail);
                        db.SaveChanges();
                        
                        scope.Complete();
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {   


                    }

                }

            return View(contact);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}