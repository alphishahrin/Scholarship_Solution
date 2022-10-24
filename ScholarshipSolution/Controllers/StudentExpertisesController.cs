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
    public class StudentExpertisesController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        // GET: StudentExpertises
        public ActionResult Index()
        {
            var studentExpertises = db.StudentExpertises.Include(s => s.Expertise).Include(s => s.Student);
            return View(studentExpertises.ToList());
        }

        // GET: StudentExpertises/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentExpertise studentExpertise = db.StudentExpertises.Find(id);
            if (studentExpertise == null)
            {
                return HttpNotFound();
            }
            return View(studentExpertise);
        }

        // GET: StudentExpertises/Create
        public ActionResult Create()
        {
            ViewBag.ExpertiseID = new SelectList(db.Expertises, "ExpertiseID", "TopicName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName");
            return View();
        }

        // POST: StudentExpertises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentExpertiseID,StudentId,ExpertiseID,Certificate,Institution")] StudentExpertise studentExpertise, int id)
        {
            
            if (ModelState.IsValid)
            {
                studentExpertise.StudentId = id;
                db.StudentExpertises.Add(studentExpertise);
                db.SaveChanges();
                return RedirectToAction("Dashboard","Students");
            }

            ViewBag.ExpertiseID = new SelectList(db.Expertises, "ExpertiseID", "TopicName", studentExpertise.ExpertiseID);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentExpertise.StudentId);
            return View(studentExpertise);
        }

        // GET: StudentExpertises/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentExpertise studentExpertise = db.StudentExpertises.Find(id);
            if (studentExpertise == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExpertiseID = new SelectList(db.Expertises, "ExpertiseID", "TopicName", studentExpertise.ExpertiseID);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentExpertise.StudentId);
            return View(studentExpertise);
        }

        // POST: StudentExpertises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentExpertiseID,StudentId,ExpertiseID,Certificate,Institution")] StudentExpertise studentExpertise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentExpertise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExpertiseID = new SelectList(db.Expertises, "ExpertiseID", "TopicName", studentExpertise.ExpertiseID);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentExpertise.StudentId);
            return View(studentExpertise);
        }

        // GET: StudentExpertises/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentExpertise studentExpertise = db.StudentExpertises.Find(id);
            if (studentExpertise == null)
            {
                return HttpNotFound();
            }
            return View(studentExpertise);
        }

        // POST: StudentExpertises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentExpertise studentExpertise = db.StudentExpertises.Find(id);
            db.StudentExpertises.Remove(studentExpertise);
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
