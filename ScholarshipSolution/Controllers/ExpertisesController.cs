using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScholarshipSolution.Models;

namespace ScholarshipSolution.Controllers
{
    public class ExpertisesController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        // GET: Expertises
        public ActionResult Index()
        {
            return View(db.Expertises.ToList());
        }

        // GET: Expertises/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expertise expertise = db.Expertises.Find(id);
            if (expertise == null)
            {
                return HttpNotFound();
            }
            return View(expertise);
        }

        // GET: Expertises/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Expertises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpertiseID,TopicName")] Expertise expertise)
        {
            if (ModelState.IsValid)
            {
                db.Expertises.Add(expertise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expertise);
        }

        // GET: Expertises/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expertise expertise = db.Expertises.Find(id);
            if (expertise == null)
            {
                return HttpNotFound();
            }
            return View(expertise);
        }

        // POST: Expertises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExpertiseID,TopicName")] Expertise expertise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expertise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expertise);
        }

        // GET: Expertises/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expertise expertise = db.Expertises.Find(id);
            if (expertise == null)
            {
                return HttpNotFound();
            }
            return View(expertise);
        }

        // POST: Expertises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expertise expertise = db.Expertises.Find(id);
            db.Expertises.Remove(expertise);
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
