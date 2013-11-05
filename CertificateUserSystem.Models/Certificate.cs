using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUserSystem.Models
{
    public class Certificate
    {

        public int Id { get; set; }
        [Required]
        //[Range(typeof(DateTime), DateTime.Now.AddDays(-2).ToString(), DateTime.Now.AddYears(10).ToString())]
        public DateTime IssueDate { get; set; }
        [Required]
        [StringLength(70, MinimumLength = 5)]
        public string Title { get; set; }

        [Range(2, 6)]
        public int MinimalMark { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public Certificate()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Courses = new HashSet<Course>();
        }
    }
}
