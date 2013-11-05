using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUserSystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 5)]
        public string Title { get; set; }
        [Required]
        [StringLength(70, MinimumLength = 5)]
        public string Lecturer { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }

        public virtual ICollection<CourseInstance> CourseInstances { get; set; }
        public Course()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Certificates = new HashSet<Certificate>();
            this.IsActive = true;
        }
    }
}
