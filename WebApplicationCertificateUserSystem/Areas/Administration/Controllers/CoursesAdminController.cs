using CertificateUserSystem.Data;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using WebApplicationCertificateUserSystem.Areas.Administration.Models;
using CertificateUserSystem.Models;

namespace WebApplicationCertificateUserSystem.Areas.Administration.Controllers
{
    [Authorize(Roles="Admin")]
    public class CoursesAdminController : BaseController
    {
        public CoursesAdminController()
            :base(new UowData())
        {
        }

        public CoursesAdminController(IUowData data)
            :base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var json = 
                Json(this.Data.Courses.All(new string[] { "Certificates" }).
                Select(CourseGridViewModel.FromCourse).ToList().ToDataSourceResult(request));

            return json;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, CourseGridViewModel course)
        {
            if ((course != null) && (ModelState.IsValid))
            {
                var newCourse = new Course()
                {
                    Title = course.Title,
                    Lecturer = course.Lecturer,
                    Description = course.Description,
                    IsActive = course.IsActive
                };

                this.Data.Courses.Add(newCourse);
                this.Data.SaveChanges();
                course.Certificates = new List<string>();
                course.Id = newCourse.Id;
            }

            return Json(new[] { course }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, CourseGridViewModel course)
        {

            if (course != null && ModelState.IsValid)
            {
                var target = this.Data.Courses.All(new string[] { "Certificates"}).FirstOrDefault(c => c.Id == course.Id);
                if (target != null)
                {
                    target.Title = course.Title;
                    target.Description = course.Description;
                    target.Lecturer = course.Lecturer;

                    course.Certificates = target.Certificates.Select(c => c.Title).ToList();
                    this.Data.SaveChanges();
                }
            }

            return Json(new[] { course }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetUserList(int id)
        {
            //var course = this.Data.Courses.All(new string[] { "Users", "Certificates" }).FirstOrDefault(c => c.Id == id);

            /*var course = from user in this.Data.Users.All()
                         from courseInstance in user.Courses
                         select new
                         {
                             Username = user.UserName,
                             courseInstance = courseInstance.Course.Title
                         };
            var zd = course.ToList();*/

            var courseInstances = this.Data.Courses.All(new string[] { "CourseInstances", "CourseInstances.User" }).FirstOrDefault(c => c.Id == id).CourseInstances;
            var enrolledUsers = new List<string>();
            foreach (var course in courseInstances)
            {
                if (course.IsActive)
                {
                    enrolledUsers.Add(course.User.Id);
                }
            }

            var eligibleUsers = Data.Users.All().ToList();
            eligibleUsers = eligibleUsers.Where(u => !enrolledUsers.Contains(u.Id)).ToList();

            if (eligibleUsers == null)
            {
                return View("Error");
            }

            var userList = new List<UserEnrollModel>();

            foreach (var user in eligibleUsers)
            {
                userList.Add(new UserEnrollModel() { Id = user.Id, Username = user.UserName });
            }

            var model = new EnrollmentModel();
            model.CourseId = id;
            model.Users = userList;
            model.Id = "";

            /*var context = new DataContext();
            var eligibleUsers = context.Users.ToList();*/

            return PartialView("_UserEnrollment", model);
            
        }

        public ActionResult EnrollUser(EnrollmentModel model)
        {
            if ((model.StartDate != null) && (model.EndDate != null) &&
                (model.StartDate.Value.CompareTo(model.EndDate) > 0))
            {
                ModelState.AddModelError("EndDate", "Start date must preceed end date.");
                return View("Index", model);
            }

            var course = this.Data.Courses.GetById(model.CourseId);
            var user = this.Data.Users.All().FirstOrDefault(u => u.Id == model.Id);
            var courseInstance = new CourseInstance()
            {
                User = user,
                Course = course,
                Mark = null,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };
            this.Data.CourseInstances.Add(courseInstance);
            this.Data.SaveChanges();
            model.Message = "User enrolled";

            return View("Index", model);
        }
    }
}