using CertificateUserSystem.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApplicationCertificateUserSystem.Areas.Administration.Models
{
    public class CourseGridViewModel
    {
        public static Expression<Func<Course, CourseGridViewModel>> FromCourse
        {
            get
            {
                return c => new CourseGridViewModel()
                {
                    Id= c.Id,
                    IsActive = c.IsActive,
                    Title = c.Title,
                    Lecturer = c.Lecturer,
                    Description = c.Description,
                    Certificates = c.Certificates.Select(cert => cert.Title).ToList()
                };
            }

        }
        public int? Id { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 5)]
        public string Title { get; set; }
        [Required]
        [StringLength(70, MinimumLength = 5)]
        public string Lecturer { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<string> Certificates { get; set; }
        public CourseGridViewModel()
        {
            this.Certificates = new List<string>();
            this.IsActive = true;
        }
    }
}