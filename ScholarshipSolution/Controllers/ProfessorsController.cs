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
    public class ProfessorsController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        // GET: Professors
        //public ActionResult Index()
        //{
        //    var professors = db.Professors.Include(p => p.Expertise).Include(p => p.Interest).Include(p => p.University);
        //    return View(professors.ToList());
        //}


        //// GET: Professors/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Professor professor = db.Professors.Find(id);
        //    if (professor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(professor);
        //}

        // GET: Professors/Create
        public ActionResult Create()
        {
            ViewBag.ExpertiseID = new SelectList(db.Expertises, "ExpertiseID", "TopicName");
            ViewBag.InterestID = new SelectList(db.Interests, "InterestID", "TopicName");
            ViewBag.UniID = new SelectList(db.Universities, "UniID", "UniName");
            return View();
        }

        // POST: Professors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProfID,Name,Email,EducationalBackground,NoOfStudents,Funding,Password,Picture,Department,UniID,InterestID,ExpertiseID")] Professor professor, Professor imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.Picture = "~/Content/ProfessorPicture/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Content/ProfessorPicture/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                //db.Professors.Add(professor);
                db.Professors.Add(imageModel);
                db.SaveChanges();
                return Content("Sign up successful!");
            }

            ViewBag.ExpertiseID = new SelectList(db.Expertises, "ExpertiseID", "TopicName", professor.ExpertiseID);
            ViewBag.InterestID = new SelectList(db.Interests, "InterestID", "TopicName", professor.InterestID);
            ViewBag.UniID = new SelectList(db.Universities, "UniID", "UniName", professor.UniID);
            return View(professor);
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(TempProfessor tempProfessor)
        {
            if (ModelState.IsValid)
            {
                var user = db.Professors.Where(u => u.Name.Equals(tempProfessor.Name)
                && u.Password.Equals(tempProfessor.Password) && u.Email.Equals(tempProfessor.Email)).FirstOrDefault();

                if (user != null)
                {
                    Session["user_name"] = user.Name;
                    Session["prof_id"] = user.ProfID;
                    return RedirectToAction("Profile","Professors");
                    //return Content("Login Successful");
                }
                else
                {
                    ViewBag.LoginFailed = "User Not Found or Password Mismatched";
                    //return Content("Login Failed");
                }
            }
            return View();
        }

        //public ActionResult DashBoard()
        //{
        //    string name = Convert.ToString(Session["user_name"]);
        //    var user = db.Professors.Where(u => u.Name.Equals(name)).FirstOrDefault();

        //    return View(user);
        //}

        public ActionResult ProfileFromStudent(int? id)
        {
            var user = db.Professors.Where(u => u.ProfID.Equals((int)id)).FirstOrDefault();

            return View(user);
        }

        public ActionResult Profile()
        {
            string name = Convert.ToString(Session["user_name"]);
            var user = db.Professors.Where(u => u.Name.Equals(name)).FirstOrDefault();

            return View(user);
        }


        // GET: Professors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExpertiseID = new SelectList(db.Expertises, "ExpertiseID", "TopicName", professor.ExpertiseID);
            ViewBag.InterestID = new SelectList(db.Interests, "InterestID", "TopicName", professor.InterestID);
            ViewBag.UniID = new SelectList(db.Universities, "UniID", "UniName", professor.UniID);
            return View(professor);
        }

        // POST: Professors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProfID,Name,Email,EducationalBackground,NoOfStudents,Funding,Password,Picture,Department,UniID,InterestID,ExpertiseID")] Professor professor, Professor imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.Picture = "~/Content/ProfessorPicture/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Content/ProfessorPicture/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                db.Entry(imageModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            ViewBag.ExpertiseID = new SelectList(db.Expertises, "ExpertiseID", "TopicName", professor.ExpertiseID);
            ViewBag.InterestID = new SelectList(db.Interests, "InterestID", "TopicName", professor.InterestID);
            ViewBag.UniID = new SelectList(db.Universities, "UniID", "UniName", professor.UniID);
            return View(professor);
        }

        // GET: Professors/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Professor professor = db.Professors.Find(id);
        //    if (professor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(professor);
        //}

        //// POST: Professors/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Professor professor = db.Professors.Find(id);
        //    db.Professors.Remove(professor);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
