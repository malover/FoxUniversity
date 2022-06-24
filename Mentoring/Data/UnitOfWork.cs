using Mentoring.Models;
using Microsoft.EntityFrameworkCore;

namespace Mentoring.Data
{
    public class UnitOfWork : IDisposable
    {       
        private SchoolContext _context;
        private GenericRepository<Student> studentRepository;
        private GenericRepository<Group> groupRepository;
        private GenericRepository<Course> courseRepository;

        public UnitOfWork()
        {
            var contextOptions = new DbContextOptionsBuilder<SchoolContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Mentoring.Data;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;
            _context = new SchoolContext(contextOptions);
        }
        public GenericRepository<Student> StudentRepository
        {
            get
            {
                if (this.studentRepository == null)
                {
                    this.studentRepository = new GenericRepository<Student>(_context);
                }
                return studentRepository;
            }
        }

        public GenericRepository<Group> GroupRepository
        {
            get
            {
                if (this.groupRepository == null)
                {
                    this.groupRepository = new GenericRepository<Group>(_context);
                }
                return groupRepository;
            }
        }

        public GenericRepository<Course> CourseRepository
        {
            get
            {

                if (this.courseRepository == null)
                {
                    this.courseRepository = new GenericRepository<Course>(_context);
                }
                return courseRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
