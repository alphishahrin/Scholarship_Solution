using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScholarshipSolution.Models;

namespace ScholarshipSolution.Controllers
{
    public class UniversitiesController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        // GET: Universities
        public ActionResult Index()
        {
            return View(db.Universities.ToList());
        }

        public ActionResult ShowAll()
        {
            return View(db.Universities.ToList());
        }

        public ActionResult SearchUni(string searching)
        {
            return View(db.Universities.Where(x => x.UniName.Contains(searching) || x.City.Contains(searching) || x.Country.Contains(searching) || searching == null).ToList()); 
        }

        // GET: Universities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            University university = db.Universities.Find(id);
            if (university == null)
            {
                return HttpNotFound();
            }
            return View(university);
        }

        // GET: Universities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Universities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UniID,UniName,UniRating,UniPicture,City,Country")] University university, University imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.UniPicture = "~/Content/UniversityPicture/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Content/UniversityPicture/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                //db.Universities.Add(university);
                db.Universities.Add(imageModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(university);
        }

        // GET: Universities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            University university = db.Universities.Find(id);
            if (university == null)
            {
                return HttpNotFound();
            }
            return View(university);
        }

        // POST: Universities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UniID,UniName,UniRating,UniPicture,City,Country")] University university, University imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.UniPicture = "~/Content/UniversityPicture/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Content/UniversityPicture/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                //db.Entry(university).State = EntityState.Modified;
                db.Entry(imageModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(university);
        }

        // GET: Universities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            University university = db.Universities.Find(id);
            if (university == null)
            {
                return HttpNotFound();
            }
            return View(university);
        }

        // POST: Universities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            University university = db.Universities.Find(id);
            db.Universities.Remove(university);
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
