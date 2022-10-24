using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScholarshipSolution.Models;

namespace ScholarshipSolution.Controllers
{
    public class ProfessorResearchPapersController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        // GET: ProfessorResearchPapers
        public ActionResult Index(int id)
        {
            //var professorResearchPapers = db.ProfessorResearchPapers.Include(p => p.Professor);
            //return View(professorResearchPapers.ToList());
            string query = $"Select * from ProfessorResearchPaper WHERE ProfID =  { id } ";

            var ProfessorResearchPapers = db.Database.SqlQuery<ProfessorResearchPaper>(query).ToList();
            var result = (from a in ProfessorResearchPapers
                          select new ProfessorResearchPaper
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

        // GET: ProfessorResearchPapers/Create
        public ActionResult Create(int? id)
        {
            ProfessorResearchPaper professorResearchPaper = db.ProfessorResearchPapers.Find(id);           
            ViewBag.ProfID = new SelectList(db.Professors, "ProfID", "Name");
            return View();
        }
        

        // POST: ProfessorResearchPapers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DOI,Title,ProfID,PageNO,Volume,PublicationDate,Publisher,Citation,Link")] ProfessorResearchPaper professorResearchPaper, int id)
        {
            professorResearchPaper.ProfID = id;
            var professorResearchPapers = db.ProfessorResearchPapers.Include(p => p.Professor);
            if (ModelState.IsValid)
            {
                db.ProfessorResearchPapers.Add(professorResearchPaper);
                //db.Entry(professorResearchPaper).State = EntityState.Added;
                db.SaveChanges();               
                return RedirectToAction("Profile", "Professors");
            }

            ViewBag.ProfID = new SelectList(db.Professors, "ProfID", "Name", professorResearchPaper.ProfID);
            //return View(professorResearchPaper);
            return View(professorResearchPapers);
        }


        // GET: ProfessorResearchPapers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessorResearchPaper professorResearchPaper = db.ProfessorResearchPapers.Find(id);
            if (professorResearchPaper == null)
            {
                return HttpNotFound();
            }
            return View(professorResearchPaper);
        }


        // GET: ProfessorResearchPapers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessorResearchPaper professorResearchPaper = db.ProfessorResearchPapers.Find(id);
            if (professorResearchPaper == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfID = new SelectList(db.Professors, "ProfID", "Name", professorResearchPaper.ProfID);
            return View(professorResearchPaper);
        }

        // POST: ProfessorResearchPapers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DOI,Title,ProfID,PageNO,Volume,PublicationDate,Publisher,Citation,Link")] ProfessorResearchPaper professorResearchPaper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(professorResearchPaper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfID = new SelectList(db.Professors, "ProfID", "Name", professorResearchPaper.ProfID);
            return View(professorResearchPaper);
        }

        // GET: ProfessorResearchPapers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfessorResearchPaper professorResearchPaper = db.ProfessorResearchPapers.Find(id);
            if (professorResearchPaper == null)
            {
                return HttpNotFound();
            }
            return View(professorResearchPaper);
        }

        // POST: ProfessorResearchPapers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProfessorResearchPaper professorResearchPaper = db.ProfessorResearchPapers.Find(id);
            db.ProfessorResearchPapers.Remove(professorResearchPaper);
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
