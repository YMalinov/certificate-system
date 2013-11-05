using CertificateUserSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUserSystem.Data
{
    public interface IUowData : IDisposable
    {
        GenericRepository<Course> Courses { get; }

        GenericRepository<Certificate> Certificates { get; }

        GenericRepository<ApplicationUser> Users { get; }

        GenericRepository<CourseInstance> CourseInstances { get; }

        int SaveChanges();
    }
}
