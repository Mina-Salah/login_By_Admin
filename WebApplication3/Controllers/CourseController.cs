using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    [Authorize] // السماح فقط للمستخدمين المسجلين بالدخول
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public CourseController(ICourseService courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetCoursesWithTeacherAsync();
            var viewModel = _mapper.Map<IEnumerable<CourseViewModel>>(courses);
            return View(viewModel);
        }

        private async Task<IEnumerable<SelectListItem>> GetTeachersSelectListAsync()
        {
            var teachers = await _courseService.GetAllTeachersAsync();
            return teachers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name ?? "غير محدد"
            });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var vm = new CourseViewModel
            {
                TeachersList = await GetTeachersSelectListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(vm);
                await _courseService.AddAsync(course);
                return RedirectToAction(nameof(Index));
            }

            vm.TeachersList = await GetTeachersSelectListAsync();
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            var vm = _mapper.Map<CourseViewModel>(course);
            vm.TeachersList = await GetTeachersSelectListAsync();
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseViewModel vm)
        {
            if (id != vm.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(vm);
                await _courseService.UpdateAsync(course);
                return RedirectToAction(nameof(Index));
            }

            vm.TeachersList = await GetTeachersSelectListAsync();
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
