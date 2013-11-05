using CertificateUserSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApplicationCertificateUserSystem.Models
{
    public class CertificateModel
    {
        public static Expression<Func<Certificate,CertificateModel>> FromCertificate
        {
            get
            {
                return x => new CertificateModel() { Id = x.Id, Title = x.Title, MinimalMark = x.MinimalMark, ShowApply = false };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public bool ShowApply { get; set; }

        [Range(2, 6)]
        public int MinimalMark { get; set; }
    }

    public class CertificateDetailsModel
    {
        public string Title { get; set; }
        public ICollection<Course> Courses { get; set; }
    }

    public class CertificateApplicationModel
    {
        public string Title { get; set; }
        public bool IsIssued { get; set; }
    }

    public class IssuedCertificateModel
    {
        public string Title { get; set; }
        public string IssueDate { get; set; }
        public int Mark { get; set; }
        public string OwnerName { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}