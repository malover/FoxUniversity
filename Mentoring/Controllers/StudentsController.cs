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
    public class StudentsController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Students
        public IActionResult Index()
        {
            var students = unitOfWork.StudentRepository.Get();
            return View(students);
        }

        // GET: Students/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || unitOfWork.StudentRepository == null)
            {
                return NotFound();
            }

            var student = unitOfWork.StudentRepository.GetByID(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            PopulateGroupsDropDownList();
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("LastName,FirstMidName,GroupID")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.StudentRepository.Insert(student);
                    unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again.");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || unitOfWork.StudentRepository == null)
            {
                return NotFound();
            }

            var student = unitOfWork.StudentRepository.GetByID(id);
            if (student == null)
            {
                return NotFound();
            }
            PopulateGroupsDropDownList();
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ID,LastName,FirstMidName,GroupID")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.StudentRepository.Update(student);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public IActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null || unitOfWork.StudentRepository == null)
            {
                return NotFound();
            }

            var student = unitOfWork.StudentRepository.GetByID(id);
            if (student == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Unable to delete group, because it's not empty";
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = unitOfWork.StudentRepository.GetByID(id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                unitOfWork.StudentRepository.Delete(student);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool StudentExists(int id)
        {
            if (unitOfWork.StudentRepository.GetByID(id) == null)
            {
                return false;
            }
            else
                return true;
        }

        private void PopulateGroupsDropDownList(object selectedGroup = null)
        {
            var groupsQuery =  from g in unitOfWork.GroupRepository.Get().ToList()
                               orderby g.GroupName
                               select g;
            ViewBag.GroupID = new SelectList(groupsQuery, "GroupID", "GroupName", selectedGroup);
        }
    }
}
