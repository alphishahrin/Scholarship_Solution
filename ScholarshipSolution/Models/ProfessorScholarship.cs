using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarshipSolution.Models
{
    public class ProfessorScholarship
    {
        public int ProfId { get; set; }
        public int ScholarshipID { get; set; }
        public string DegreeName { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Department { get; set; }
        public string UniName { get; set; }
        public double PercentageOfScholarship { get; set; }
        public string Session { get; set; }
        public int Seats { get; set; }
        public string MinimumGPA { get; set; }
        public string OtherRequirements { get; set; }
        public Nullable<System.DateTime> LastDate { get; set; }
    }
}