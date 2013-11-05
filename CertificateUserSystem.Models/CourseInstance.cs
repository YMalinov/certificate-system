using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUserSystem.Models
{
    public class CourseInstance
    {
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Course Course { get; set; }

        [Range(2, 6)]
        public int? Mark { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public CourseInstance()
        {
            this.IsActive = true;
        }
    }
}
