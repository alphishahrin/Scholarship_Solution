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
    public class ScholarshipsController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        // GET: Scholarships
        public ActionResult Index(int id)
        {
            //var scholarships = db.Scholarships.Include(s => s.Professor).Include(s => s.University);
            //return View(scholarships.ToList());
            string query = $"select Scholarship.* ,  Professor.*, University.*  from Scholarship FULL JOIN Professor ON Scholarship.ProfId = Professor.ProfId FULL JOIN University ON University.UniID = Scholarship.UniID WHERE Scholarship.ProfID = { id } ";

            var scholarships = db.Database.SqlQuery<Scholarship>(query).ToList();
            var result = (from a in scholarships
                          join b in db.Professors on a.ProfID equals b.ProfID
                          join c in db.Universities on a.UniID equals c.UniID
                          select new ProfessorScholarship
                          {
                              ScholarshipID = a.ScholarshipID,
                              ProfId = b.ProfID,
                              DegreeName = a.DegreeName,
                              Subject = a.Subject,
                              Name = b.Name,
                              Email = b.Email,
                              Department = b.Department,
                              Picture = b.Picture,
                              UniName = c.UniName,
                              PercentageOfScholarship = a.PercentageOfScholarship,
                              Session = a.Session,
                              Seats = a.Seats,
                              MinimumGPA = a.MinimumGPA,
                              OtherRequirements = a.OtherRequirements,
                              LastDate = a.LastDate
                          }).ToList();
            return View(result);
        }
        public ActionResult Indexone(int id)
        {
            //var scholarships = db.Scholarships.Include(s => s.Professor).Include(s => s.University);
            //return View(scholarships.ToList());
            string query = $"select Scholarship.* ,  Professor.*, University.*  from Scholarship FULL JOIN Professor ON Scholarship.ProfId = Professor.ProfId FULL JOIN University ON University.UniID = Scholarship.UniID WHERE Scholarship.ProfID = { id } ";

            var scholarships = db.Database.SqlQuery<Scholarship>(query).ToList();
            var result = (from a in scholarships
                          join b in db.Professors on a.ProfID equals b.ProfID
                          join c in db.Universities on a.UniID equals c.UniID
                          select new ProfessorScholarship
                          {
                              ScholarshipID = a.ScholarshipID,
                              ProfId = b.ProfID,
                              DegreeName = a.DegreeName,
                              Subject = a.Subject,
                              Name = b.Name,
                              Email = b.Email,
                              Department = b.Department,
                              Picture = b.Picture,
                              UniName = c.UniName,
                              PercentageOfScholarship = a.PercentageOfScholarship,
                              Session = a.Session,
                              Seats = a.Seats,
                              MinimumGPA = a.MinimumGPA,
                              OtherRequirements = a.OtherRequirements,
                              LastDate = a.LastDate
                          }).ToList();
            return View(result);
        }

        // GET: Scholarships/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scholarship scholarship = db.Scholarships.Find(id);
            if (scholarship == null)
            {
                return HttpNotFound();
            }
            return View(scholarship);
        }

        // GET: Scholarships/Create
        public ActionResult Create(int? id)
        {
            Scholarship scholarship = db.Scholarships.Find(id);
            ViewBag.ProfID = new SelectList(db.Professors, "ProfID", "Name");
            ViewBag.UniID = new SelectList(db.Universities, "UniID", "UniName");
            return View();
        }

        // POST: Scholarships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScholarshipID,DegreeName,UniID,ProfID,Subject,PercentageOfScholarship,Session,Seats,MinimumGPA,OtherRequirements,LastDate")] Scholarship scholarship, int id)
        {
            scholarship.ProfID = id;
            var scholarships = db.Scholarships.Include(p => p.Professor);
            if (ModelState.IsValid)
            {
                db.Scholarships.Add(scholarship);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            ViewBag.ProfID = new SelectList(db.Professors, "ProfID", "Name", scholarship.ProfID);
            ViewBag.UniID = new SelectList(db.Universities, "UniID", "UniName", scholarship.UniID);
            return View(scholarship);
        }

        // GET: Scholarships/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scholarship scholarship = db.Scholarships.Find(id);
            if (scholarship == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfID = new SelectList(db.Professors, "ProfID", "Name", scholarship.ProfID);
            ViewBag.UniID = new SelectList(db.Universities, "UniID", "UniName", scholarship.UniID);
            return View(scholarship);
        }

        // POST: Scholarships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScholarshipID,DegreeName,UniID,ProfID,Subject,PercentageOfScholarship,Session,Seats,MinimumGPA,OtherRequirements,LastDate")] Scholarship scholarship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scholarship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfID = new SelectList(db.Professors, "ProfID", "Name", scholarship.ProfID);
            ViewBag.UniID = new SelectList(db.Universities, "UniID", "UniName", scholarship.UniID);
            return View(scholarship);
        }

        // GET: Scholarships/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scholarship scholarship = db.Scholarships.Find(id);
            if (scholarship == null)
            {
                return HttpNotFound();
            }
            return View(scholarship);
        }

        // POST: Scholarships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Scholarship scholarship = db.Scholarships.Find(id);
            db.Scholarships.Remove(scholarship);
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
