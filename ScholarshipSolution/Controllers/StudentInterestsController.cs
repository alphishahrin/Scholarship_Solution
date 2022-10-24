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
    public class StudentInterestsController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        // GET: StudentInterests
        public ActionResult Index()
        {
            var studentInterests = db.StudentInterests.Include(s => s.Interest).Include(s => s.Student);
            return View(studentInterests.ToList());
        }

        // GET: StudentInterests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentInterest studentInterest = db.StudentInterests.Find(id);
            if (studentInterest == null)
            {
                return HttpNotFound();
            }
            return View(studentInterest);
        }

        // GET: StudentInterests/Create
        public ActionResult Create()
        {
            ViewBag.InterestID = new SelectList(db.Interests, "InterestID", "TopicName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName");
            return View();
        }

        // POST: StudentInterests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentInterestID,StudentId,InterestID,Reason")] StudentInterest studentInterest, int id)
        {
            if (ModelState.IsValid)
            {
                studentInterest.StudentId = id; 
                db.StudentInterests.Add(studentInterest);
                db.SaveChanges();
                return RedirectToAction("Dashboard","Students");
            }

            ViewBag.InterestID = new SelectList(db.Interests, "InterestID", "TopicName", studentInterest.InterestID);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentInterest.StudentId);
            return View(studentInterest);
        }

        // GET: StudentInterests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentInterest studentInterest = db.StudentInterests.Find(id);
            if (studentInterest == null)
            {
                return HttpNotFound();
            }
            ViewBag.InterestID = new SelectList(db.Interests, "InterestID", "TopicName", studentInterest.InterestID);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentInterest.StudentId);
            return View(studentInterest);
        }

        // POST: StudentInterests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentInterestID,StudentId,InterestID,Reason")] StudentInterest studentInterest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentInterest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InterestID = new SelectList(db.Interests, "InterestID", "TopicName", studentInterest.InterestID);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentInterest.StudentId);
            return View(studentInterest);
        }

        // GET: StudentInterests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentInterest studentInterest = db.StudentInterests.Find(id);
            if (studentInterest == null)
            {
                return HttpNotFound();
            }
            return View(studentInterest);
        }

        // POST: StudentInterests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentInterest studentInterest = db.StudentInterests.Find(id);
            db.StudentInterests.Remove(studentInterest);
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
