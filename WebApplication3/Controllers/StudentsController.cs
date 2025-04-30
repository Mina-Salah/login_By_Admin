using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication3.Services;
using WebApplication3.ViewModels;
using WebApplication3.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication3.Controllers
{
    // Restricting access to the entire controller for users with "Admin" role
    [Authorize(Roles = "Admin")] 
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public StudentsController(
            IStudentService studentService,
            ITeacherService teacherService,
            ICourseService courseService,
            IMapper mapper)
        {
            _studentService = studentService;
            _teacherService = teacherService;
            _courseService = courseService;
            _mapper = mapper;
        }

        // Action to display all students (accessible by any logged-in user)
        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllAsync();
            var viewModels = _mapper.Map<List<StudentViewModel>>(students);
            return View(viewModels);
        }
        // Action to show the student creation page (only accessible by "Admin" users)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            var courses = await _courseService.GetAllAsync();

            ViewBag.Teachers = teachers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();

            ViewBag.Courses = courses.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View();
        }

        // Action to handle the creation of a new student (only accessible by "Admin" users)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(StudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var teachers = await _teacherService.GetAllTeachersAsync();
                var courses = await _courseService.GetAllAsync();

                // Re-populate the teachers and courses in case of input errors
                ViewBag.Teachers = teachers.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList();

                ViewBag.Courses = courses.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                return View(model);
            }

            var student = _mapper.Map<Student>(model);
            await _studentService.AddAsync(student, model.CourseIds); // Add the student with associated courses
            return RedirectToAction(nameof(Index));
        }

        // Action to show the student edit page (only accessible by "Admin" users)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound();

            var viewModel = _mapper.Map<StudentViewModel>(student);
            var teachers = await _teacherService.GetAllTeachersAsync();
            var courses = await _courseService.GetAllAsync();

            ViewBag.Teachers = teachers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();

            ViewBag.Courses = courses.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(viewModel);
        }

        // Action to handle student updates (only accessible by "Admin" users)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, StudentViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                var teachers = await _teacherService.GetAllTeachersAsync();
                var courses = await _courseService.GetAllAsync();

                // Re-populate the teachers and courses in case of input errors
                ViewBag.Teachers = teachers.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList();

                ViewBag.Courses = courses.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                return View(model);
            }

            var student = _mapper.Map<Student>(model);
            await _studentService.UpdateAsync(student, model.CourseIds); // Update the student and associated courses
            return RedirectToAction(nameof(Index));
        }

        // Action to show the delete confirmation page (only accessible by "Admin" users)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound();

            var viewModel = _mapper.Map<StudentViewModel>(student);
            return View(viewModel);
        }

        // Action to confirm the deletion of a student (only accessible by "Admin" users)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _studentService.DeleteAsync(id); // Delete the student
            return RedirectToAction(nameof(Index));
        }
    }
}
