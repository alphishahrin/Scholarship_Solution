using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScholarshipSolution.Models;

namespace ScholarshipSolution.Controllers
{
    public class StudentScholarshipsController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        // GET: StudentScholarships
        public ActionResult Index(int id)
        {
            string query = $"Select Scholarship.*, StudentScholarship.*, University.*, Professor.* from Scholarship inner join StudentScholarship on StudentScholarship.ScholarshipID = Scholarship.ScholarshipID inner join University on University.UniID = Scholarship.UniID inner join Professor on Professor.ProfID = Scholarship.ProfID where StudentScholarship.StudentID = { id } ";

            var StudentScholarships = db.Database.SqlQuery<StudentScholarship>(query).ToList();
            var result = (from a in StudentScholarships
                          join b in db.Scholarships on a.ScholarshipID equals b.ScholarshipID
                          join c in db.Universities on b.UniID equals c.UniID
                          join d in db.Professors on b.ProfID equals d.ProfID
                          select new StudentScholarshipInfo
                          {
                              StudentScholarshipID = a.StudentScholarshipID,
                              ScholarshipID = (int)a.ScholarshipID,
                              DegreeName = b.DegreeName,
                              Subject = b.Subject,
                              PercentageOfScholarship = b.PercentageOfScholarship,
                              Session = b.Session,
                              MinimumGPA = b.MinimumGPA,
                              UniName = c.UniName,
                              ProfName = d.Name,
                              Status = a.Status
                          }).ToList();
            return View(result);
          
        }

        // GET: StudentScholarships/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentScholarship studentScholarship = db.StudentScholarships.Find(id);
            if (studentScholarship == null)
            {
                return HttpNotFound();
            }
            return View(studentScholarship);
        }

        // GET: StudentScholarships/Create
        public ActionResult Create()
        {
            ViewBag.ScholarshipID = new SelectList(db.Scholarships, "ScholarshipID", "DegreeName");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName");
            return View();
        }

        // POST: StudentScholarships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentScholarshipID,ScholarshipID,StudentID,Status")] StudentScholarship studentScholarship, int id)
        {
            int student_id = Convert.ToInt32(Session["user_id"]);
            studentScholarship.StudentID = student_id;
            studentScholarship.ScholarshipID = id;
            studentScholarship.Status = "Waiting";
            //Scholarship scholarship = new Scholarship();
            Scholarship scholarship = db.Scholarships.Find(id);


            string CurrentDate = DateTime.Now.ToString("yyyy/MM/dd");

            if (ModelState.IsValid)
            {
               
                var studentSch = db.Scholarships.Where(u => u.LastDate.Equals(DateTime.Now));
                var studentEnr = db.StudentScholarships.Where(u => u.StudentID.Equals(student_id) && u.ScholarshipID.Equals(id));
                DateTime s = (DateTime)scholarship.LastDate;
                string t = s.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                if ((CurrentDate.CompareTo(t) == 1))
                {
                    Response.Write("<script>alert('Date Already Expired');</script>");
                }
                else if(studentEnr != null)
                {
                    Response.Write("<script>alert('Already enrolled');</script>");
                }
                else
                {
                    db.StudentScholarships.Add(studentScholarship);
                    db.SaveChanges();
                    return RedirectToAction("StudentScholarship", "Students", new { search = "", sortOrder = "", confirm = "hi" });
                }
            }

            ViewBag.ScholarshipID = new SelectList(db.Scholarships, "ScholarshipID", "DegreeName", studentScholarship.ScholarshipID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", studentScholarship.StudentID);
            return View(studentScholarship);
        }



        public ActionResult EnrolledStudent(int? id)
        {
            string query = $"Select Student.*, StudentScholarship.* From Student Inner join StudentScholarship on Student.StudentID = StudentScholarship.StudentID WHERE  StudentScholarship.ScholarshipID =  { id } ";

            var studentScholarships = db.Database.SqlQuery<StudentScholarship>(query).ToList();
            var result = (from a in studentScholarships
                          join b in db.Students on a.StudentID equals b.StudentID                         
                          select new EnrollStudent
                          {
                              StudentScholarshipId = a.StudentScholarshipID,
                              ScholarshipId = (int)a.ScholarshipID,
                              Name = b.FirstName,
                              Email = b.Email,
                              status = a.Status,
                              StudentId = b.StudentID
                          }).ToList();
            return View(result);
        }

        // GET: StudentScholarships/Edit/5
        public ActionResult Edit(int? stuid, int?schid)
        {
            if (stuid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentScholarship studentScholarship = db.StudentScholarships.Find(stuid);
            if (studentScholarship == null)
            {
                return HttpNotFound();
            }
            ViewBag.ScholarshipID = new SelectList(db.Scholarships, "ScholarshipID", "DegreeName", studentScholarship.ScholarshipID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", studentScholarship.StudentID);
            return View(studentScholarship);
        }

        // POST: StudentScholarships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentScholarshipID,ScholarshipID,StudentID,Status")] StudentScholarship studentScholarship, int stuid, int schid)
        {
            int student_id = Convert.ToInt32(Session["user_id"]);
            studentScholarship.StudentID = stuid;
            studentScholarship.ScholarshipID = schid;
            //studentScholarship.Status = "Waiting";
            if (ModelState.IsValid)
            {
                db.Entry(studentScholarship).State = EntityState.Modified;
                db.SaveChanges();
                return View();
            }
            ViewBag.ScholarshipID = new SelectList(db.Scholarships, "ScholarshipID", "DegreeName", studentScholarship.ScholarshipID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", studentScholarship.StudentID);
            return View(studentScholarship);
        }
        public ActionResult Accept(int? id, int? stuid, int? schid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentScholarship studentScholarship = db.StudentScholarships.Find(id);
            studentScholarship.StudentID = stuid;
            studentScholarship.ScholarshipID = schid;
            if (studentScholarship == null)
            {
                return HttpNotFound();
            }
            ViewBag.ScholarshipID = new SelectList(db.Scholarships, "ScholarshipID", "DegreeName", studentScholarship.ScholarshipID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", studentScholarship.StudentID);
            return View(studentScholarship);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Accept([Bind(Include = "StudentScholarshipID,ScholarshipID,StudentID,Status")] StudentScholarship studentScholarship, int? stuid, int? schid)
        {
            int prof_id = Convert.ToInt32(Session["prof_id"]);
            if (ModelState.IsValid)
            {
                studentScholarship.StudentID = stuid;
                studentScholarship.ScholarshipID = schid;
                db.Entry(studentScholarship).State = EntityState.Modified;
                db.SaveChanges();
                Response.Write("<script>alert('Done');</script>");
                //return View();
                //return RedirectToAction("ËnrolledStudent", "StudentScholarships", new { id = prof_id });
                return RedirectToAction("Accept", "StudentScholarships", new { id = studentScholarship.StudentScholarshipID });
            }
            ViewBag.ScholarshipID = new SelectList(db.Scholarships, "ScholarshipID", "DegreeName", studentScholarship.ScholarshipID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FirstName", studentScholarship.StudentID);
            return View(studentScholarship);
            //return View();
        }
        

        // GET: StudentScholarships/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentScholarship studentScholarship = db.StudentScholarships.Find(id);
            if (studentScholarship == null)
            {
                return HttpNotFound();
            }
            return View(studentScholarship);
        }

        // POST: StudentScholarships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentScholarship studentScholarship = db.StudentScholarships.Find(id);
            db.StudentScholarships.Remove(studentScholarship);
            db.SaveChanges();

            int stuid = (int)Session["user_id"];
            return RedirectToAction("Index", "StudentScholarships", new { id = stuid});
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
