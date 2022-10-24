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
    public class StudentResearchPapersController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        // GET: StudentResearchPapers
        public ActionResult Index(int id)
        {
            //var studentResearchPapers = db.StudentResearchPapers.Include(s => s.Student);
            //return View(studentResearchPapers.ToList());
            string query = $"Select * from StudentResearchPaper WHERE StudentID =  { id } ";

            var StudentResearchPapers = db.Database.SqlQuery<StudentResearchPaper>(query).ToList();
            var result = (from a in StudentResearchPapers
                          select new StudentResearchPaper
                          {
                              DOI = a.DOI,
                              Title = a.Title,
                              PageNO = a.PageNO,
                              Volume = a.Volume,
                              PublicationDate = a.PublicationDate,
                              Publisher = a.Publisher,
                              Citation = a.Citation,
                              Link = a.Link
                          }).ToList();
            return View(result);
        }

        // GET: StudentResearchPapers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentResearchPaper studentResearchPaper = db.StudentResearchPapers.Find(id);
            if (studentResearchPaper == null)
            {
                return HttpNotFound();
            }
            return View(studentResearchPaper);
        }

        // GET: StudentResearchPapers/Create
        public ActionResult Create()
        {
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName");
            return View();
        }

        // POST: StudentResearchPapers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DOI,Title,StudentID,PageNO,Volume,PublicationDate,Publisher,Citation,Link")] StudentResearchPaper studentResearchPaper)
        {
            if (ModelState.IsValid)
            {
                db.StudentResearchPapers.Add(studentResearchPaper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", studentResearchPaper.StudentID);
            return View(studentResearchPaper);
        }

        // GET: StudentResearchPapers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentResearchPaper studentResearchPaper = db.StudentResearchPapers.Find(id);
            if (studentResearchPaper == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", studentResearchPaper.StudentID);
            return View(studentResearchPaper);
        }

        // POST: StudentResearchPapers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DOI,Title,StudentID,PageNO,Volume,PublicationDate,Publisher,Citation,Link")] StudentResearchPaper studentResearchPaper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentResearchPaper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", studentResearchPaper.StudentID);
            return View(studentResearchPaper);
        }

        // GET: StudentResearchPapers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentResearchPaper studentResearchPaper = db.StudentResearchPapers.Find(id);
            if (studentResearchPaper == null)
            {
                return HttpNotFound();
            }
            return View(studentResearchPaper);
        }

        // POST: StudentResearchPapers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentResearchPaper studentResearchPaper = db.StudentResearchPapers.Find(id);
            db.StudentResearchPapers.Remove(studentResearchPaper);
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
