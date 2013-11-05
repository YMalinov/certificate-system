using CertificateUserSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApplicationCertificateUserSystem.Areas.Administration.Models
{
    public class CertificateViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        
        public string Title { get; set; }

        public int MinimalMark { get; set; }

        public string Courses { get; set; }
    }
}