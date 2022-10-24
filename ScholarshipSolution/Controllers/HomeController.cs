using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using ScholarshipSolution.Models;

namespace ScholarshipSolution.Controllers
{
    public class HomeController : Controller
    {
        private final_32_dbEntities db = new final_32_dbEntities();
        public ActionResult Index()
        {
                   
            var viewmodel = new Student();
            foreach (var item in db.Students.ToList())
            {
                int id = item.StudentID;

                //SystemsCount 
                int count = db.Students.Where(x => x.StudentID == id).Count();
                item.StudentID = count;
            }
            int count1 = db.Students.Count();
            ViewBag.stu = count1;

            var viewmodel1 = new Scholarship();
            foreach (var item in db.Scholarships.ToList())
            {
                int id = item.ScholarshipID;

                //SystemsCount 
                int count = db.Scholarships.Where(x => x.ScholarshipID == id).Count();
                item.ScholarshipID = count;
            }
            int count2 = db.Scholarships.Count();
            ViewBag.sch = count2;

            var viewmodel2 = new Professor();
            foreach (var item in db.Professors.ToList())
            {
                int id = item.ProfID;

                //SystemsCount 
                int count = db.Professors.Where(x => x.ProfID == id).Count();
                item.ProfID = count;
            }
            int count3 = db.Professors.Count();
            ViewBag.prof = count3;

            var viewmodel3 = new University();
            foreach (var item in db.Universities.ToList())
            {
                int id = item.UniID;

                //SystemsCount 
                int count = db.Universities.Where(x => x.UniID == id).Count();
                item.UniID = count;
            }
            int count4 = db.Universities.Count();
            ViewBag.uni = count4;

            var university = (from u in db.Universities select u).Take(3).OrderByDescending(u => u.UniRating);
            return View(university.ToList());

        }

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(TempAdmin tempAdmin)
        {
            
            
            if (ModelState.IsValid)
            {
                if(tempAdmin.AdminName.Equals("Admin") && tempAdmin.AdminPassword.Equals("12345"))
                {
                    Session["user_email"] = "Admin";
                    Session["user_id"] = "12345";
                    return RedirectToAction("Index","Universities");
                }
                else
                {
                    ViewBag.LoginFailed = "Login Failed due to wrong username or password!";
                    return View();
                }
            }
            return View();
        }

        public ActionResult AdminDashboard()
        {
            return View(); 
        }
    }
}