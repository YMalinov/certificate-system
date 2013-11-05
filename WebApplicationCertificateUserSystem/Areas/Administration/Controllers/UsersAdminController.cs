using CertificateUserSystem.Data;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using WebApplicationCertificateUserSystem.Areas.Administration.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplicationCertificateUserSystem.Areas.Administration.Controllers
{
    [Authorize(Roles="Admin")]
    public class UsersAdminController : BaseController
    {
        public UsersAdminController()
            : this(new UowData())
        {

        }
        public UsersAdminController(IUowData data)
            : base(data)
        {
        }
        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            var users = this.Data.Users.All().Select(UserViewModel.FromUser).ToList();
            return View(users);
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var users = this.Data.Users.All().Select(UserViewModel.FromUser).ToList();
            return Json(users.ToDataSourceResult(request));
        }

        public ActionResult EditingPopup_Update([DataSourceRequest] DataSourceRequest request, UserViewModel user)
        {

            if (user != null && ModelState.IsValid)
            {
                var userDb = this.Data.Users.All().FirstOrDefault(u=>u.Id==user.Id);
                if (userDb != null)
                {
                    userDb.FirstName = user.FirstName;
                    userDb.LastName = user.LastName;
                    userDb.Management.DisableSignIn = user.IsBann;
                    userDb.Management.DisableSignIn = user.IsBann;
                    if (user.Role=="Admin" && !userDb.Roles.Any(r=>r.Role.Name=="Admin"))
                    {
                        //(userDb.Roles.FirstOrDefault(r=>r.RoleId=="1")
                        userDb.Roles.Add(new UserRole() 
                        { 
                            RoleId="1",
                            User=userDb
                        });
                    }
                    else if(user.Role=="User" && !userDb.Roles.Any(r=>r.Role.Name=="User"))
                    {
                        userDb.Roles.Add(new UserRole()
                        {
                            RoleId = "2",
                            User = userDb
                        });
                    }
                }
                this.Data.SaveChanges();
                //context.Entry(userDb).State = EntityState.Modified;
                //context.SaveChanges();

            }
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();
                ViewBag.ErrorMassage = errorList;
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState),JsonRequestBehavior.AllowGet);
        }

	}
}