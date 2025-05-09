﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new TeacherViewModel();
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null) return NotFound();

            var model = _mapper.Map<TeacherViewModel>(teacher);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteTeacherAsync(id);
            TempData["Success"] = "تم حذف المعلم";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deleted()
        {
            var deletedTeachers = await _teacherService.GetDeletedTeachersAsync();
            var model = _mapper.Map<List<TeacherViewModel>>(deletedTeachers);
            return View("Deleted", model); // View مخصص للـ Deleted Teachers
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            await _teacherService.RestoreTeacherAsync(id);
            TempData["Success"] = "تم استرجاع المعلم بنجاح";
            return RedirectToAction(nameof(Deleted));
        }
    }
}
