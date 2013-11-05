using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CertificateUserSystem.Models;
using CertificateUserSystem.Data;
using WebApplicationCertificateUserSystem.Models;

namespace WebApplicationCertificateUserSystem.Controllers
{
    public class CoursesController : BaseController
    {
         public CoursesController()
            : this(new UowData())
        {
        }
         public CoursesController(IUowData data)
            : base(data)
        {
        }

        // GET: /Course/
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PublicCourses", "Courses");
            }

            ViewBag.Title = "All Courses";

            var activeCourses = this.Data.Courses
                .All(new string[] { "Users" })
                .Where(c => c.IsActive == true)
                .Select(PublicCourseModel.FromCourse)
                .ToList();

            for (int i = 0; i < activeCourses.Count; i++)
            {
                activeCourses[i].EnrolledCount = this.Data.CourseInstances
                    .All(new string[] { "Course" })
                    .ToList()
                    .Where(c => c.Course.Id == activeCourses[i].Id)
                    .Count();
            }

            var loggedUserId = (from loggedUser in this.Data.Users.All().ToList()
                                where loggedUser.UserName.ToLower() == User.Identity.Name.ToLower()
                                select loggedUser.Id).FirstOrDefault();

            if (loggedUserId == null)
            {
                throw new ArgumentException("Logged user id not found.");
            }

            for (int i = 0; i < activeCourses.Count; i++)
            {
                var courseInstances = this.Data.CourseInstances
                    .All(new string[] { "User", "Course" })
                    .ToList()
                    .Where(c => c.Course.Id == activeCourses[i].Id)
                    .ToList();

                for (int j = 0; j < courseInstances.Count; j++)
                {
                    if (courseInstances[j].User.Id == loggedUserId)
                    {
                        activeCourses[i].Enrolled = true;
                    }
                }
            }

            return View(activeCourses);
        }


        public ActionResult PublicCourses()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Courses");
            }

            ViewBag.Title = "All Courses";

            var activeCourses = this.Data.Courses.All(new string[] { "Users" })
                .Where(c => c.IsActive == true)
                .Select(PublicCourseModel.FromCourse);

            return View(activeCourses);
        }

        // GET: /Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = this.Data.Courses.All().Where(c => c.Id == id).FirstOrDefault();

            if (course == null)
            {
                return HttpNotFound();
            }

            PublicCourseModel courseModel = new PublicCourseModel()
            {
                Description = course.Description,
                Id = course.Id,
                LecturerName = course.Lecturer,
                Title = course.Title,
                EnrolledCount = this.Data.CourseInstances
                                    .All(new string[] { "Course" })
                                    .Where(c => c.Course.Id == id)
                                    .Count()
            };
            
            string loggedUserId = GetLoggedUserId();

            if (loggedUserId != null)
            {
                courseModel.Enrolled = this.Data.CourseInstances
                                        .All(new string[] { "User", "Course" })
                                        .Where(c => c.User.Id == loggedUserId)
                                        .Where(c => c.Course.Id == course.Id)
                                        .Count() == 1;
            }

            return View(courseModel);
        }

        public ActionResult Enroll(int id)
        {

            var course = this.Data.Courses.GetById(id);
            
            var user = this.Data.Users.All().FirstOrDefault(u=>u.UserName == User.Identity.Name);
            var courseInstance = new CourseInstance()
            {
                User = user,
                Course = course,
                Mark = null,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10)
            };

            this.Data.CourseInstances.Add(courseInstance);
            this.Data.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult GetAutocompleteData(string text)
        {
            var selectedBooks = this.Data.Courses.All()
                .Where(x => x.Title.ToLower().Contains(text.ToLower()))
                .Select(PublicCourseModel.FromCourse);

            return Json(selectedBooks, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string searchedText)
        {
            var selectedBooks = this.Data.Courses.All()
               .Where(x => x.Title.ToLower().Contains(searchedText.ToLower()))
               .Select(PublicCourseModel.FromCourse);

            return View(selectedBooks);
        }

        public ActionResult ShowCourseDetails(string bookName,int id)
        {
            var course = this.Data.Courses.GetById(id);
            PublicCourseModel courseModel = new PublicCourseModel()
            {
                Description = course.Description,
                Id = course.Id,
                LecturerName = course.Lecturer,
                Title = course.Title
            };

            return View(courseModel);
        }

        [NonAction]
        private string GetLoggedUserId()
        {
            var loggedUserId = (from loggedUser in this.Data.Users.All().ToList()
                                where loggedUser.UserName.ToLower() == User.Identity.Name.ToLower()
                                select loggedUser.Id).FirstOrDefault();

            return loggedUserId;
        }
    }
}
