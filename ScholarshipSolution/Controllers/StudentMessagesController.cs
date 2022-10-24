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
    public class StudentMessagesController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        // GET: StudentMessages
        public ActionResult Index()
        {
            var studentMessages = db.StudentMessages.Include(s => s.Professor).Include(s => s.Student);
            return View(studentMessages.ToList());
        }

        // GET: StudentMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentMessage studentMessage = db.StudentMessages.Find(id);
            if (studentMessage == null)
            {
                return HttpNotFound();
            }
            return View(studentMessage);
        }

        // GET: StudentMessages/Create
        public ActionResult Create()
        {
            ViewBag.ProfId = new SelectList(db.Professors, "ProfID", "Name");
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName");
            return View();
        }

        // POST: StudentMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,StudentId,TextMessage,ProfId")] StudentMessage studentMessage)
        {
            if (ModelState.IsValid)
            {
                db.StudentMessages.Add(studentMessage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProfId = new SelectList(db.Professors, "ProfID", "Name", studentMessage.ProfId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentMessage.StudentId);
            return View(studentMessage);
        }

        // GET: StudentMessages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentMessage studentMessage = db.StudentMessages.Find(id);
            if (studentMessage == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfId = new SelectList(db.Professors, "ProfID", "Name", studentMessage.ProfId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentMessage.StudentId);
            return View(studentMessage);
        }

        // POST: StudentMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,StudentId,TextMessage,ProfId")] StudentMessage studentMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfId = new SelectList(db.Professors, "ProfID", "Name", studentMessage.ProfId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentMessage.StudentId);
            return View(studentMessage);
        }

        // GET: StudentMessages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentMessage studentMessage = db.StudentMessages.Find(id);
            if (studentMessage == null)
            {
                return HttpNotFound();
            }
            return View(studentMessage);
        }

        // POST: StudentMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentMessage studentMessage = db.StudentMessages.Find(id);
            db.StudentMessages.Remove(studentMessage);
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
