using Mentoring.Data;
using Mentoring.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Mentoring.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public IActionResult Index(int? id, int? groupID)
        {

            var viewModel = new CourseIndexData();
            viewModel.Courses = unitOfWork.CourseRepository.Get();
            /*            viewModel.Groups = unitOfWork.GroupRepository.Get();
                        viewModel.Students = unitOfWork.StudentRepository.Get();*/

            if (id != null)
            {
                ViewData["CourseID"] = id.Value;
                viewModel.Groups = unitOfWork.GroupRepository.Get().Where(
                    g => g.CourseID == id);
            }

            if (groupID != null)
            {
                ViewData["GroupID"] = groupID.Value;
                viewModel.Students = unitOfWork.StudentRepository.Get().Where(
                    s => s.GroupID == groupID);
            }

            return View(viewModel);
            /*var viewModel = new CourseIndexData();
            viewModel.Courses = await _context.Courses
                .Include(c => c.Groups)
                .ThenInclude(g => g.Students)
                .AsNoTracking()
                .OrderBy(c => c.Title)
                .ToListAsync();

            if (id != null)
            {
                ViewData["CourseID"] = id.Value;
                Course course = viewModel.Courses.Where(
                    c => c.CourseID == id.Value).Single();
                viewModel.Groups = course.Groups;
            }
            if (groupID != null)
            {
                ViewData["GroupID"] = groupID.Value;
                viewModel.Students = viewModel.Groups.Where(
                    x => x.GroupID == groupID
                    ).Single().Students;
            }
            return View(viewModel);*/
        }

        public IActionResult Privacy()
        {
            return View();
        } 
    }
}