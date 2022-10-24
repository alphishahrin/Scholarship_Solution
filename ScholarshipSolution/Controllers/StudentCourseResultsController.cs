
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
    public class StudentCourseResultsController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        // GET: StudentCourseResults
        public ActionResult Index(int id)
        {
            string query = $"select StudentCourseResult.* ,  Course.*, Student.*  from StudentCourseResult FULL JOIN Student ON StudentCourseResult.StudentId = Student.StudentID FULL JOIN Course ON Course.CourseID = StudentCourseResult.CourseID WHERE StudentCourseResult.StudentId =  { id } ";
                        
            var studentCourseResults = db.Database.SqlQuery<StudentCourseResult>(query).ToList();
            var result = (from a in studentCourseResults
                          join b in db.Students on a.StudentId equals b.StudentID
                         join c in db.Courses on a.CourseID equals c.CourseID
                         select new StudentCourseInfo
                         {
                             Grade = a.Grade,
                             FirstName = b.FirstName,
                             CourseName = c.CourseName
                         }).ToList();
            return View(result);
        }

        // GET: StudentCourseResults/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCourseResult studentCourseResult = db.StudentCourseResults.Find(id);
            if (studentCourseResult == null)
            {
                return HttpNotFound();
            }
            return View(studentCourseResult);
        }

        // GET: StudentCourseResults/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName");
            return View();
        }

        // POST: StudentCourseResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentCourseResultID,CourseID,StudentId,Grade")] StudentCourseResult studentCourseResult)
        {
            if (ModelState.IsValid)
            {
                db.StudentCourseResults.Add(studentCourseResult);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", studentCourseResult.CourseID);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentCourseResult.StudentId);
            return View(studentCourseResult);
        }

        // GET: StudentCourseResults/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCourseResult studentCourseResult = db.StudentCourseResults.Find(id);
            if (studentCourseResult == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", studentCourseResult.CourseID);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentCourseResult.StudentId);
            return View(studentCourseResult);
        }

        // POST: StudentCourseResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentCourseResultID,CourseID,StudentId,Grade")] StudentCourseResult studentCourseResult)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentCourseResult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", studentCourseResult.CourseID);
            ViewBag.StudentId = new SelectList(db.Students, "StudentID", "FirstName", studentCourseResult.StudentId);
            return View(studentCourseResult);
        }

        // GET: StudentCourseResults/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCourseResult studentCourseResult = db.StudentCourseResults.Find(id);
            if (studentCourseResult == null)
            {
                return HttpNotFound();
            }
            return View(studentCourseResult);
        }

        // POST: StudentCourseResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentCourseResult studentCourseResult = db.StudentCourseResults.Find(id);
            db.StudentCourseResults.Remove(studentCourseResult);
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
