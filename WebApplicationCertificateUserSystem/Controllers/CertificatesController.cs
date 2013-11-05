using CertificateUserSystem.Data;
using CertificateUserSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationCertificateUserSystem.Models;

namespace WebApplicationCertificateUserSystem.Controllers
{
    public class CertificatesController : BaseController
    {
        public CertificatesController(IUowData data)
            : base(data)
        {
        }

        public CertificatesController()
            : base()
        {
        }

        [HttpGet, ActionName("Index")]
        public ActionResult Index()
        {
            var certificates = this.Data.Certificates.All().Select(CertificateModel.FromCertificate).ToList();
            var currentUser = this.Data.Users.All(new string[] { "Certificates" }).FirstOrDefault(u => u.UserName == User.Identity.Name);

            foreach (var certificate in certificates)
            {
                var userCertificate = currentUser.Certificates.FirstOrDefault(c => c.Title == certificate.Title);
                if (userCertificate == null)
                {
                    certificate.ShowApply = true;
                }
            }

            return View(certificates);
        }

        [HttpGet, ActionName("Details")]
        public ActionResult Details(int id)
        {
            var certificateInDb = this.Data.Certificates.GetById(id);
            if (certificateInDb == null)
            {
                return HttpNotFound();
            }

            var certificateDetailsModel = new CertificateDetailsModel() { Title = certificateInDb.Title, Courses = certificateInDb.Courses };

            return PartialView("_Details", certificateDetailsModel);
        }

        [HttpGet, ActionName("Apply")]
        [Authorize]
        public ActionResult Apply(int id)
        {
            var user = this.Data.Users.All(new string[] { "Courses" }).FirstOrDefault(u => u.UserName == User.Identity.Name);
            var certificate = this.Data.Certificates.All(new string[] { "Courses" }).FirstOrDefault(c => c.Id == id);
            if (user == null || certificate == null)
            {
                return HttpNotFound();
            }

            bool isIssued = true;

            foreach (var course in certificate.Courses)
            {
                var userCourse = user.Courses.FirstOrDefault(x => x.Course.Title == course.Title && x.Mark >= certificate.MinimalMark);
                if (userCourse == null)
                {
                    isIssued = false;
                    break;
                }
            }

            var certificateApplication = new CertificateApplicationModel() { Title = certificate.Title, IsIssued = isIssued };

            if (isIssued)
            {
                user.Certificates.Add(certificate);
                certificate.Users.Add(user);
                this.Data.SaveChanges();
            }

            return PartialView("_Apply", certificateApplication);
        }

        [HttpGet, ActionName("ViewCertificate")]
        [Authorize]
        public ActionResult ViewCertificate(int id)
        {
            var currentUser = this.Data.Users.All(new string[] { "Certificates" }).FirstOrDefault(u => u.UserName == User.Identity.Name);
            var certificate = currentUser.Certificates.FirstOrDefault(c => c.Id == id);

            if (certificate == null)
            {
                return HttpNotFound();
            }

            var issuedCertificateModel = new IssuedCertificateModel
            {
                Title = certificate.Title,
                Mark = certificate.MinimalMark,
                IssueDate = certificate.IssueDate.Date.ToShortDateString(),
                OwnerName = currentUser.FirstName + " " + currentUser.LastName,
                Courses = certificate.Courses
            };

            return View(issuedCertificateModel);
        }
	}
}