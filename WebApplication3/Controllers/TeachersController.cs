using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TeachersController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly IMapper _mapper;

        public TeachersController(ITeacherService teacherService, IMapper mapper)
        {
            _teacherService = teacherService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            var teacherViewModels = _mapper.Map<List<TeacherViewModel>>(teachers);
            return View(teacherViewModels);
        }
        public IActionResult Create()
        {
            var model = new TeacherViewModel();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var teacher = _mapper.Map<Teacher>(model);
            await _teacherService.AddTeacherAsync(teacher);
            TempData["Success"] = "تم إضافة المعلم بنجاح";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null) return NotFound();

            var model = _mapper.Map<TeacherViewModel>(teacher);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeacherViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var teacher = _mapper.Map<Teacher>(model);
            await _teacherService.UpdateTeacherAsync(teacher);
            TempData["Success"] = "تم تعديل بيانات المعلم";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteTeacherAsync(id);
            TempData["Success"] = "تم حذف المعلم";
            return RedirectToAction(nameof(Index));
        }



    }
}
