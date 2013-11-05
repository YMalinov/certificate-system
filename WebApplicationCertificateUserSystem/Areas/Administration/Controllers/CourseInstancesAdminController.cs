using CertificateUserSystem.Data;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationCertificateUserSystem.Areas.Administration.Models;
using Kendo.Mvc.Extensions;

namespace WebApplicationCertificateUserSystem.Areas.Administration.Controllers
{
    public class CourseInstancesAdminController : BaseController
    {
        public CourseInstancesAdminController()
            :base(new UowData())
        {
        }

        public CourseInstancesAdminController(IUowData data)
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
                Json(this.Data.CourseInstances.All(new string[] { "User", "Course" }).
                Select(CourseInstanceGridViewModel.FromCourseInstance).ToList().ToDataSourceResult(request));

            return json;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, CourseInstanceGridViewModel course)
        {

            if (course != null && ModelState.IsValid)
            {
                var target = this.Data.CourseInstances.All(new string[] { "User", "Course" }).FirstOrDefault(c => c.Id == course.Id);
                if (target != null)
                {
                    target.Mark = course.Mark;
                    target.StartDate = course.StartDate;
                    target.EndDate = course.EndDate;
                    this.Data.SaveChanges();
                }
            }

            return Json(new[] { course }.ToDataSourceResult(request, ModelState));
        }
    }
}