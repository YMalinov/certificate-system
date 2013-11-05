using CertificateUserSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationCertificateUserSystem.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
            :base(new UowData())
        {
        }

        public HomeController(IUowData data)
            :base(data)
        {
        }
        public ActionResult Index()
        {
            //this.Data.Courses.All(new string[] { "Users", "Certificates" } );
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
          var x =  this.Data.Users.All(new string[] { "Certificates" });
          foreach (var user in x)
          {
              var y = user;
          }

            return View();
        }
    }
}