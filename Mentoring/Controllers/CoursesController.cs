using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mentoring.Data;
using Mentoring.Models;

namespace Mentoring.Controllers
{
    public class CoursesController : Controller
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Courses
        public IActionResult Index()
        {
            var courses = unitOfWork.CourseRepository.Get();
            return View(courses.ToList());
        }

        // GET: Courses/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || unitOfWork.CourseRepository == null)
            {
                return NotFound();
            }

            var course = unitOfWork.CourseRepository.GetByID(id);

            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CourseID,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CourseRepository.Insert(course);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || unitOfWork.CourseRepository == null)
            {
                return NotFound();
            }

            var course = unitOfWork.CourseRepository.GetByID(id);

            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CourseID,Title,Credits")] Course course)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.CourseRepository.Update(course);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || unitOfWork.CourseRepository == null)
            {
                return NotFound();
            }

            var course = unitOfWork.CourseRepository.GetByID(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (unitOfWork.CourseRepository == null)
            {
                return NotFound();
            }

            var course = unitOfWork.CourseRepository.GetByID(id);
            unitOfWork.CourseRepository.Delete(course);
            unitOfWork.Save();

            if (course == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            if (unitOfWork.CourseRepository.GetByID(id) == null)
            {
                return false;
            }
            else
                return true;
        }
    }
}
