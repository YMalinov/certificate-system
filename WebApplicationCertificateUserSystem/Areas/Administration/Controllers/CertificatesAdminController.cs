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
using System.Text;

namespace WebApplicationCertificateUserSystem.Areas.Administration.Controllers
{
    public class CertificatesAdminController : BaseController
    {
        public CertificatesAdminController()
            : this(new UowData())
        {

        }
        public CertificatesAdminController(IUowData data)
            : base(data)
        {
        }

        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            var certificates = this.Data.Certificates.All().ToList();
            var models = new List<CertificateViewModel>();
            foreach (var certificate in certificates)
            {
                var model = this.FromCertificate(certificate);
                models.Add(model);
            }

            return View(models);
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var certificates = this.Data.Certificates.All().ToList();
            var models = new List<CertificateViewModel>();
            foreach (var certificate in certificates)
            {
                var model = this.FromCertificate(certificate);
                models.Add(model);
            }

            return Json(models.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, CertificateViewModel model)
        {
            CertificateViewModel newModel = model;

            if (model != null && ModelState.IsValid)
            {
                var certificateInDb = this.Data.Certificates.GetById(model.Id);
                string[] newCertificateCourses = model.Courses.Split(',');
                certificateInDb.MinimalMark = model.MinimalMark;
                certificateInDb.Title = model.Title;

                if (certificateInDb.Courses.Count > newCertificateCourses.Length)
                {
                    certificateInDb.Courses = new HashSet<Course>();
                    foreach (var item in newCertificateCourses)
                    {
                        var course = certificateInDb.Courses.FirstOrDefault(c => c.Title.Equals(item, StringComparison.InvariantCultureIgnoreCase));
                        if (course == null)
                        {
                            var courseToAdd = this.Data.Courses.All().FirstOrDefault(c => c.Title.Equals(item, StringComparison.InvariantCultureIgnoreCase));
                            certificateInDb.Courses.Add(courseToAdd);
                        }
                    }
                }
                else
                {
                    foreach (var item in newCertificateCourses)
                    {
                        var course = certificateInDb.Courses.FirstOrDefault(c => c.Title.Equals(item, StringComparison.InvariantCultureIgnoreCase));
                        if (course == null)
                        {
                            var courseToAdd = this.Data.Courses.All().FirstOrDefault(c => c.Title.Equals(item, StringComparison.InvariantCultureIgnoreCase));
                            certificateInDb.Courses.Add(courseToAdd);
                        }
                    }
                }


                this.Data.SaveChanges();

                newModel = this.FromCertificate(certificateInDb);
            }

            return Json(new[] { newModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, CertificateViewModel model)
        {
            var newModel = model;
            if (model != null && ModelState.IsValid)
            {
                var certificateInDb = this.Data.Certificates.GetById(model.Id);

                if (certificateInDb == null)
                {
                    return HttpNotFound();
                }

                this.Data.Certificates.Delete(certificateInDb);
                this.Data.SaveChanges();
            }

            return Json(new[] { newModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, CertificateViewModel model)
        {
            var newModel = model;
            if (model != null && ModelState.IsValid)
            {
                var certificate = new Certificate()
                {
                    Title = model.Title,
                    IssueDate = DateTime.Now,
                    MinimalMark = model.MinimalMark
                };

                string[] newCertificateCourses = model.Courses.Split(',');

                if (newCertificateCourses.Length > 1)
                {
                    foreach (var item in newCertificateCourses)
                    {
                        var certificateCourse = this.Data.Courses.All().FirstOrDefault(c => c.Title.Equals(item,StringComparison.InvariantCultureIgnoreCase));
                        certificate.Courses.Add(certificateCourse);
                    }
                }
                else
                {
                    var courseTitle = newCertificateCourses[0];
                    var certificateCourse = this.Data.Courses.All().FirstOrDefault(c => c.Title.Equals(courseTitle,StringComparison.InvariantCultureIgnoreCase));
                    certificate.Courses.Add(certificateCourse);
                }

                this.Data.Certificates.Add(certificate);
                this.Data.SaveChanges();

                newModel = this.FromCertificate(certificate);
            }

            return Json(new[] { newModel }.ToDataSourceResult(request, ModelState));
        }

        private CertificateViewModel FromCertificate(Certificate certificate)
        {
            var model = new CertificateViewModel();
            model.Id = certificate.Id;
            model.Title = certificate.Title;
            model.MinimalMark = certificate.MinimalMark;

            StringBuilder sb = new StringBuilder();
            foreach (var item in certificate.Courses)
            {
                sb.Append(item.Title + ",");
            }
            if (sb.Length > 0)
            {
                sb.Length--;
            }

            model.Courses = sb.ToString();

            return model;
        }
	}
}