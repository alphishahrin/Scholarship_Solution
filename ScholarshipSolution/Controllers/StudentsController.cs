using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using ScholarshipSolution.Models;
using System.Web.Security;

namespace ScholarshipSolution.Controllers
{
    public class StudentsController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();

        public ActionResult Index(string searching, string sortBy)
        {
            ViewBag.SortGpa = string.IsNullOrEmpty(sortBy) ? "Gpa_desc" : "";
            var student = db.Students.AsQueryable();

            //Include(i => i.Modules.Select(s => s.Chapters) && i.Lab)

            student = db.Students.Where(x => x.FirstName.Contains(searching) || x.Department.Contains(searching)  || searching == null);
            switch (sortBy)
            {
                case "Gpa_desc":
                    student = student.OrderByDescending(x => x.AverageGrade);
                    break;
                default:
                    student = student.OrderBy(x => x.AverageGrade);
                    break;
            }
            return View(student.ToList());



            //string query = "Select Student.*, Interest.*, Expertise.* from Student Inner join StudentInterest on Student.StudentID = StudentInterest.StudentId"
            //    + " Inner join StudentExpertise on student.StudentID = StudentExpertise.StudentId"+ " Inner join Interest on Interest.InterestID = StudentInterest.InterestID"
            //    + " Inner join Expertise on Expertise.ExpertiseID = StudentExpertise.ExpertiseID";

            //var student = db.Database.SqlQuery<Student>(query).ToList();
            //var result = (from a in student
            //              join b in db.StudentInterests on a.StudentID equals b.StudentId
            //              join c in db.StudentExpertises on a.StudentID equals c.StudentId
            //              join d in db.Interests on b.InterestID equals d.InterestID
            //              join e in db.Expertises on c.ExpertiseID equals e.ExpertiseID
            //              select new StudentInterestExperties
            //              {
            //                  Name = a.FirstName,
            //                  DeptName = a.Department,
            //                  Grade = a.AverageGrade,
            //                  Interest = d.TopicName,
            //                  Expertise = e.TopicName,
            //                  StudentId = a.StudentID
            //              });

            //ViewBag.SortGpa = string.IsNullOrEmpty(sortBy) ? "Gpa_desc" : "";

            //switch (sortBy)
            //{
            //    case "Gpa_desc":
            //        result = result.OrderByDescending(x => x.Grade);
            //        break;
            //    default:
            //        result = result.OrderBy(x => x.Grade);
            //        break;
            //}

            //return View(result);
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Student student, Student imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.StudentPicture = "~/Content/img/StudentPicture/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Content/img/StudentPicture/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                //db.Students.Add(student);
                db.Students.Add(imageModel);
                db.SaveChanges();

                Response.Write("<script>alert('SignUp Successfull');</script>");
                return RedirectToAction("Login");

            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(TemoStudent tempStudent)
        {
            if (ModelState.IsValid)
            {
                var student = db.Students.Where(u => u.Email.Equals(tempStudent.Email)
                && u.Password.Equals(tempStudent.Password)).FirstOrDefault();

                if (student != null)
                {

                    Session["user_email"] = student.Email;
                    Session["user_id"] = student.StudentID;
                    return RedirectToAction("DashBoard");
                }
                else
                {
                    ViewBag.LoginFailed = "User not found";
                    return View();
                }

            }
            return View();

        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }





        public ActionResult IndexGrade()
        {
            return View(db.Students.ToList());
        }

        public ActionResult search(string searching)
        {

            return View(db.Universities.Where(x => x.UniName.Contains(searching) || x.City.Contains(searching) || x.Country.Contains(searching) || searching == null).ToList());


        }
        public ActionResult Professor(string searching)
        {
            
            return View(db.Professors.Include(p => p.Expertise).Include(p => p.Interest).Where(p => p.Interest.TopicName.Contains(searching) || p.Expertise.TopicName.Contains(searching) || searching == null).ToList());
        }

        public ActionResult StudentScholarship(string search, string sortOrder, string confirm)
        {

            if(!string.IsNullOrEmpty(confirm))
            {
                Response.Write("<script>alert('Enroll Successfull');</script>");
            }
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.GpaSortParm = String.IsNullOrEmpty(sortOrder)  ? "Gpa_desc" : "";
            

            var studentScholarship = db.Scholarships.AsQueryable();
            studentScholarship = studentScholarship.Include(p => p.Professor).Include(p => p.University).Where(x => x.DegreeName.Contains(search) || x.Subject.Contains(search) || x.Session.Contains(search) || x.University.UniName.Contains(search) || x.Professor.Name.Contains(search) || search == null);

            switch (sortOrder)
            {
                case "date_desc":
                    studentScholarship = studentScholarship.OrderByDescending(s => s.LastDate);
                    break;

                case "Gpa_desc":
                    studentScholarship = studentScholarship.OrderByDescending(s => s.MinimumGPA);
                    break;

                //case "Gpa":
                //    studentScholarship = studentScholarship.OrderBy(s => s.MinimumGPA);
                //    break;

                default:
                    studentScholarship = studentScholarship.OrderBy(s => s.LastDate);
                    studentScholarship = studentScholarship.OrderBy(s => s.MinimumGPA);
                    break;


            }

            return View(studentScholarship.ToList());
            //return View(db.Scholarships.Include(p => p.Professor).Include(p => p.University).Where(x => x.DegreeName.Contains(search) || x.Subject.Contains(search) || x.Session.Contains(search) || x.University.UniName.Contains(search) || x.Professor.Name.Contains(search) || search == null).ToList());
        }

       



        // GET: Students/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Student student = db.Students.Find(id);
        //    if (student == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(student);
        //}
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dynamic dy = new ExpandoObject();
            dy.StudentCourseList = getStudentCourseResults(id);
            dy.StudentExpertiseList = getStudentExpertise(id);
            dy.StudentInterestList = getStudentInterest(id);
            dy.Student = getStudent(id); 
            return View(dy);
        }
        public ActionResult Dashboard(string email)
        {

            int id = Convert.ToInt32(Session["user_id"]);
            dynamic dy1 = new ExpandoObject();
            dy1.StudentCourseList = getStudentCourseResults(id);
            dy1.StudentExpertiseList = getStudentExpertise(id);
            dy1.StudentInterestList = getStudentInterest(id);
            dy1.Student = getStudent(id);
            return View(dy1);
 
        }
        public List<StudentCourseInfo> getStudentCourseResults(int? id)
        {
            final_32_dbEntities db = new final_32_dbEntities();
            string query = $"select StudentCourseResult.* ,  Course.* from StudentCourseResult FULL JOIN Course ON Course.CourseID = StudentCourseResult.CourseID WHERE StudentCourseResult.StudentId =  { id } ";
            List<StudentCourseResult> LStudentCourseResult = db.Database.SqlQuery<StudentCourseResult>(query).ToList();
            List<StudentCourseInfo> result = (from a in LStudentCourseResult
                          join c in db.Courses on a.CourseID equals c.CourseID
                          select new StudentCourseInfo
                          {
                              Grade = a.Grade,
                              CourseName = c.CourseName
                          }).ToList();
            return result;
        }
        public List<StudentInterestInfo> getStudentInterest(int? id)
        {
            final_32_dbEntities db = new final_32_dbEntities();
            string query = $"select StudentInterest.* ,  Interest.* from StudentInterest FULL JOIN Interest ON StudentInterest.InterestID = Interest.InterestID WHERE StudentInterest.StudentId = { id } ";
            List<StudentInterest> LStudentInterest = db.Database.SqlQuery<StudentInterest>(query).ToList();
            List<StudentInterestInfo> result = (from a in LStudentInterest
                                                join c in db.Interests on a.InterestID equals c.InterestID
                                              select new StudentInterestInfo
                                              {
                                                  TopicName = c.TopicName,
                                                  Reason = a.Reason
                                              }).ToList();
            return result;
        }
        public List<StudentExpertiseInfo> getStudentExpertise(int? id)
        {
            final_32_dbEntities db = new final_32_dbEntities();
            string query = $"select StudentExpertise.* ,  Expertise.* from StudentExpertise FULL JOIN Expertise ON StudentExpertise.ExpertiseID = Expertise.ExpertiseID WHERE StudentExpertise.StudentId =  { id } ";
            List<StudentExpertise> LStudentExpertise = db.Database.SqlQuery<StudentExpertise>(query).ToList();
            List<StudentExpertiseInfo> result = (from a in LStudentExpertise
                        join c in db.Expertises on a.ExpertiseID equals c.ExpertiseID
                        select new StudentExpertiseInfo
                        {
                            TopicName = c.TopicName,
                            Institution = a.Institution
                        }).ToList();
            return result;
        }

        public Student getStudent(int? id)
        {
            final_32_dbEntities db = new final_32_dbEntities();
            Student student = db.Students.Find(id);
            return student;
        }







        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,FirstName,LastName,StudentPicture,Department,Session,Year,Semester,Email,AverageGrade,CompletedCredit,ExtracurricularActivities,Blog,Reference,Password")] Student student, Student imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.StudentPicture = "~/Content/img/StudentPicture/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Content/img/StudentPicture/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                db.Entry(imageModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View(student);
        }

        


        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
