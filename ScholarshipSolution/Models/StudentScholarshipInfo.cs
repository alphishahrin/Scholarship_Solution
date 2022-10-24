using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarshipSolution.Models
{
    public class StudentScholarshipInfo
    {
        public int StudentScholarshipID { get; set; }
        public int ScholarshipID { get; set; }
        public string DegreeName { get; set; }       
        public string Subject { get; set; }
        public double PercentageOfScholarship { get; set; }
        public string Session { get; set; }       
        public string MinimumGPA { get; set; }
        public string UniName { get; set; }
        public string ProfName { get; set; }
        public string Status { get; set; }
    }
}