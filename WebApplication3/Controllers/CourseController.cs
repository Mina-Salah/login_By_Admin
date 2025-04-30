using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Services;
using WebApplication3.ViewModels;
using WebApplication3.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication3.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ITeacherService _teacherService;
        private readonly IMapper _mapper;

        public CourseController(ICourseService courseService, ITeacherService teacherService, IMapper mapper)
        {
            _courseService = courseService;
            _teacherService = teacherService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync();
            var viewModels = _mapper.Map<List<CourseViewModel>>(courses);
            return View(viewModels);
        }

        public async Task<IActionResult> Create()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            ViewBag.Teachers = teachers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var teachers = await _teacherService.GetAllTeachersAsync();
                ViewBag.Teachers = teachers.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList();

                return View(model);
            }

            var course = _mapper.Map<Course>(model);
            await _courseService.AddAsync(course);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null) return NotFound();

            var viewModel = _mapper.Map<CourseViewModel>(course);
            ViewBag.Teachers = await _teacherService.GetAllTeachersAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Teachers = await _teacherService.GetAllTeachersAsync();
                return View(model);
            }

            var course = _mapper.Map<Course>(model);
            await _courseService.UpdateAsync(course);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null) return NotFound();

            var viewModel = _mapper.Map<CourseViewModel>(course);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _courseService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
