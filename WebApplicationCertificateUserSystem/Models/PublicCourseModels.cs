using CertificateUserSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApplicationCertificateUserSystem.Models
{
    public class PublicCourseModel
    {
        public static Expression<Func<Course, PublicCourseModel>> FromCourse
        {
            get
            {
                return course => new PublicCourseModel
                {
                    Description = course.Description,
                    Id = course.Id,
                    LecturerName = course.Lecturer,
                    Title = course.Title,
                    Enrolled = false,
                    EnrolledCount = 0
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string LecturerName { get; set; }

        public string Description { get; set; }

        public bool Enrolled { get; set; }

        public int EnrolledCount { get; set; }
    }

    //public class UserCourseModel
    //{
    //    public static Expression<Func<Course, UserCourseModel>> FromCourse
    //    {
    //        get 
    //        {
    //            return course => new UserCourseModel
    //            {
    //                Description = course.Description,
    //                Id = course.Id,
    //                LecturerName = course.Lecturer,
    //                Title = course.Title,
    //                Enrolled = 0
    //            };
    //        }
    //    }
    //    public int Id { get; set; }

    //    public string Title { get; set; }

    //    public string LecturerName { get; set; }

    //    public string Description { get; set; }

    //    public int Enrolled { get; set; }
    //}
}