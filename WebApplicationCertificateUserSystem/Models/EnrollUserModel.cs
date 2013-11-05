using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationCertificateUserSystem.Models
{
    public class EnrollUserModel
    {
        public string Id { get; set; }

        public int CourseId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Message { get; set; }
    }
}