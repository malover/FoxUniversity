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
    public class GroupsController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Groups
        public ActionResult Index()
        {
            var groups = unitOfWork.GroupRepository.Get();
            return View(groups.ToList());
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || unitOfWork.GroupRepository == null)
            {
                return NotFound();
            }

            var group = unitOfWork.GroupRepository.GetByID(id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            PopulateCoursesDropDownList();
            return View();
        }

        // POST: Groups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("GroupName,CourseID")] Group group)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.GroupRepository.Insert(group);
                    unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again.");
            }
            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || unitOfWork.CourseRepository == null)
            {
                return NotFound();
            }

            var group = unitOfWork.GroupRepository.GetByID(id);

            if (group == null)
            {
                return NotFound();
            }

            PopulateCoursesDropDownList();
            return View(group);
        }

        // POST: Groups/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("GroupID,GroupName,CourseID")] Group group)
        {
            if (id != group.GroupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.GroupRepository.Update(group);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.GroupID))
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
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null || unitOfWork.GroupRepository == null)
            {
                return NotFound();
            }

            var group = unitOfWork.GroupRepository.GetByID(id);
            if (group == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Unable to delete group, because it's not empty";
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var group = unitOfWork.GroupRepository.GetByID(id);
            if (group == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var studentInGroup = from s in unitOfWork.StudentRepository.Get()
                                     where s.GroupID == id
                                     select s;
                if (studentInGroup.Any())
                {
                    throw new ArgumentException();
                }
                unitOfWork.GroupRepository.Delete(group);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool GroupExists(int id)
        {
            if (unitOfWork.CourseRepository.GetByID(id) == null)
            {
                return false;
            }
            else
                return true;
        }

        private void PopulateCoursesDropDownList(object selectedCourse = null)
        {
            var coursesQuery = from d in unitOfWork.CourseRepository.Get().ToList()
                               orderby d.Title
                               select d;
            ViewBag.CourseID = new SelectList(coursesQuery, "CourseID", "Title", selectedCourse);
        }

    }
}

