using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarshipSolution.Models
{
    public class EnrollStudent
    {
        public int StudentScholarshipId { get; set; }
        public int ScholarshipId { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string status { get; set; }
    }
}