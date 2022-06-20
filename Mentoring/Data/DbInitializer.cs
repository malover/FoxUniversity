using Mentoring.Models;

namespace Mentoring.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            // Look for any students.
            if (context.Courses.Any())
            {
                return;   // DB has been seeded
            }

            var courses = new Course[]
            {
                new Course{CourseID=1050,Title="Chemistry",Credits=3},
                new Course{CourseID=3141,Title="Trigonometry",Credits=4},
                new Course{CourseID=2042,Title="Literature",Credits=4}
            };
            context.Courses.AddRange(courses);
            context.SaveChanges();

            var groups = new Group[]
            {
                new Group{GroupName = "121A", CourseID = 1050 },
                new Group{GroupName = "072B", CourseID = 1050 },
                new Group{GroupName = "113B", CourseID = 3141 },
                new Group{GroupName = "002A", CourseID = 3141 },
                new Group{GroupName = "161B", CourseID = 2042 },
                new Group{GroupName = "121B", CourseID = 2042 },
            };
            context.Groups.AddRange(groups);
            context.SaveChanges();

            var students = new Student[]
            {
                new Student{FirstMidName="Carson",  LastName="Alexander", GroupID = groups.Single(g => g.GroupName == "121A").GroupID},
                new Student{FirstMidName="Meredith",LastName="Alonso",    GroupID = groups.Single(g => g.GroupName == "121A").GroupID},
                new Student{FirstMidName="Arturo",  LastName="Anand",     GroupID = groups.Single(g => g.GroupName == "072B").GroupID},
                new Student{FirstMidName="Gytis",   LastName="Barzdukas", GroupID = groups.Single(g => g.GroupName == "002A").GroupID},
                new Student{FirstMidName="Yan",     LastName="Li",        GroupID = groups.Single(g => g.GroupName == "161B").GroupID},
                new Student{FirstMidName="Peggy",   LastName="Justice",   GroupID = groups.Single(g => g.GroupName == "121B").GroupID},
                new Student{FirstMidName="Laura",   LastName="Norman",    GroupID = groups.Single(g => g.GroupName == "113B").GroupID},
                new Student{FirstMidName="Nino",    LastName="Olivetto",  GroupID = groups.Single(g => g.GroupName == "002A").GroupID}
            };
            context.Students.AddRange(students);
            context.SaveChanges();           
        }
    }
}

