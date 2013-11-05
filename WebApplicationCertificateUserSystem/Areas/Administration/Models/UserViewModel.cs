using CertificateUserSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApplicationCertificateUserSystem.Areas.Administration.Models
{
    public class UserViewModel
    {
        public static Expression<Func<ApplicationUser, UserViewModel>> FromUser
        {
            get
            {
                return u => new UserViewModel
                {
                    FirstName=u.FirstName,
                    LastName=u.LastName,
                    Id=u.Id,
                    Role = u.Roles.FirstOrDefault().Role.Name ?? "User",
                    IsBann = u.Management.DisableSignIn
                };
                    
            }
        }
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        [Required]
        [StringLength(70, MinimumLength = 5)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(70, MinimumLength = 5)]
        public string LastName { get; set; }
        public string Role { get; set; }
        public bool IsBann { get; set; }
    }
}