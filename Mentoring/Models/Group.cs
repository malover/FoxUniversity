using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentoring.Models
{
    public class Group
    {
        public int GroupID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string GroupName { get; set; } = null!;


        public int CourseID { get; set; }

        public Course Course { get; set; }

        public IEnumerable<Student> Students { get; set; }
    }
}
