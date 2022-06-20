namespace Mentoring.Models
{
    public class CourseIndexData
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<Student> Students { get; set; }
    }
}
