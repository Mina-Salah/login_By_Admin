using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    [Authorize] // حماية الوصول لجميع الأفعال للمستخدمين المصادق عليهم
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public CourseController(ICourseService courseService, SchoolContext context, IMapper mapper)
        {
            _courseService = courseService;
            _context = context;
            _mapper = mapper;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            var courseViewModels = _mapper.Map<List<CourseViewModel>>(courses);

            // إضافة رسالة نجاح أو فشل إذا كانت موجودة في TempData
            if (TempData["Success"] != null)
            {
                ViewBag.SuccessMessage = TempData["Success"];
            }

            if (TempData["Error"] != null)
            {
                ViewBag.ErrorMessage = TempData["Error"];
            }

            return View(courseViewModels);
        }

        // GET: Course/Create
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var teachers = await _context.Teachers
                .Where(t => !t.IsDeleted)
                .Select(t => new TeacherViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();

            ViewBag.Teachers = new SelectList(teachers, "Id", "Name");

            var model = new CourseViewModel();
            return View(model);
        }

        // POST: Course/Create
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var course = new Course
                {
                    Name = model.Name,
                    TeacherId = model.TeacherId
                };

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Course added successfully!";
                return RedirectToAction(nameof(Index));
            }

            var teachers = await _context.Teachers
                .Where(t => !t.IsDeleted)
                .ToListAsync();

            ViewBag.Teachers = new SelectList(teachers, "Id", "Name");
            return View(model);
        }

        // GET: Course/Edit/5
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();

            var vm = _mapper.Map<CourseViewModel>(course);
            var teachers = await _context.Teachers
                .Where(t => !t.IsDeleted)
                .Select(t => new TeacherViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();

            ViewBag.Teachers = new SelectList(teachers, "Id", "Name", vm.TeacherId);
            return View(vm);
        }

        // POST: Course/Edit/5
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(vm);
                await _courseService.UpdateCourseAsync(course);

                TempData["Success"] = "Course updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            var teachers = await _context.Teachers
                .Where(t => !t.IsDeleted)
                .ToListAsync();

            ViewBag.Teachers = new SelectList(teachers, "Id", "Name", vm.TeacherId);
            return View(vm);
        }

        // GET: Course/Delete/5
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();

            var vm = _mapper.Map<CourseViewModel>(course);
            return View(vm);
        }

        // POST: Course/Delete/5
        [Authorize(Roles = "Admin")] // فقط للمستخدمين من دور "Admin"
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _courseService.DeleteCourseAsync(id);
            TempData["Success"] = "Course deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
