using CertificateUserSystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUserSystem.Data
{
    public class DataContext : IdentityDbContextWithCustomUser<ApplicationUser>
    {
        public IDbSet<Course> Courses { get; set; }
        public IDbSet<Certificate> Certificates { get; set; }

        public IDbSet<CourseInstance> CourseInstances { get; set; }
    }
}
