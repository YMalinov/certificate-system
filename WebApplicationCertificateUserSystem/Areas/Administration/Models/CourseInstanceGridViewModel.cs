using CertificateUserSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApplicationCertificateUserSystem.Areas.Administration.Models
{
    public class CourseInstanceGridViewModel
    {
        public static Expression<Func<CourseInstance, CourseInstanceGridViewModel>> FromCourseInstance
        {
            get
            {
                return c => new CourseInstanceGridViewModel()
                {
                    Id = c.Id,
                    IsActive = c.IsActive,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Username = c.User.UserName,
                    CourseName = c.Course.Title,
                    Mark = c.Mark
                };
            }
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string CourseName { get; set; }

        [Range(2, 6)]
        public int? Mark { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public CourseInstanceGridViewModel()
        {
            this.IsActive = true;
        }
    }
}