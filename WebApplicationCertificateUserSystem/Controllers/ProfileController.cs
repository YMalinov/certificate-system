using CertificateUserSystem.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationCertificateUserSystem.Controllers
{
    [Authorize]
    [NoCache]
    [HandleError]
    public class ProfileController : BaseController
    {
        private HashSet<string> allowedFileExtensions =
            new HashSet<string> { "jpg", "jpeg", "png", "gif" };
        //
        // GET: /Profile/
        public ProfileController(IUowData data)
            : base(data)
        {
        }

        public ProfileController()
            : base()
        {
        }
        public ActionResult ShowProfile()
        {
            var userName = User.Identity.Name;
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == userName);
            if (string.IsNullOrEmpty(user.AvatarPath))
            {
                user.AvatarPath = "/img/Avatars/defaultAvatar.jpg";
            }
            return View(user);
        }

        [NoCache]
        [HandleError]
        public ActionResult Submit(IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                var ext = Path.GetExtension(files.First().FileName);
                if (allowedFileExtensions.Contains(ext.Substring(1).ToLower()))
                {
                    var username = User.Identity.Name;

                    var fileName = username + ext;
                    var fullPath = Server.MapPath("~/img/Avatars/") + fileName;
                    files.First().SaveAs(fullPath);
                    var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);
                    user.AvatarPath = "/img/Avatars/" + fileName;
                    this.Data.SaveChanges();
                }
                else
                {

                }
                
            }

            return RedirectToAction("ShowProfile");
        }

        public ActionResult Result()
        {
            return View();
        }


    }
}