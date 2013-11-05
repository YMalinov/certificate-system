using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUserSystem.Models
{
    public class ApplicationUser : User
    {
        [Required]
        [StringLength(70,MinimumLength=5)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(70, MinimumLength = 5)]
        public string LastName { get; set; }
        public virtual ICollection<CourseInstance> Courses { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public ApplicationUser()
        {
            this.Courses = new HashSet<CourseInstance>();
            this.Certificates = new HashSet<Certificate>();
        }

        public string AvatarPath { get; set; }
    }
}
