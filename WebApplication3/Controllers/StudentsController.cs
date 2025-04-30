using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication3.Models;
using WebApplication3.Repositories;
using WebApplication3.Services;
using WebApplication3.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IGenericRepository<Teacher> _teacherRepository;
        private readonly IMapper _mapper;

        public StudentsController(
            IStudentService studentService,
            IGenericRepository<Teacher> teacherRepository,
            IMapper mapper)
        {
            _studentService = studentService;
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllStudentsAsync(
                includes: query => query.Include(s => s.Teacher)
                .Include(s => s.Course)
            );

            var studentViewModels = _mapper.Map<List<StudentViewModel>>(students);
            return View(studentViewModels);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new StudentViewModel
            {
                Teachers = await GetTeachersSelectListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                var student = _mapper.Map<Student>(studentViewModel);
                await _studentService.AddStudentAsync(student);
                return RedirectToAction(nameof(Index));
            }

            studentViewModel.Teachers = await GetTeachersSelectListAsync();
            return View(studentViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentService.GetStudentByIdAsync(
                id.Value,
                includes: query => query.Include(s => s.Teacher)
            );

            if (student == null)
                return NotFound();

            var model = _mapper.Map<StudentViewModel>(student);
            model.Teachers = await GetTeachersSelectListAsync();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentViewModel studentViewModel)
        {
            if (id != studentViewModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var student = _mapper.Map<Student>(studentViewModel);
                await _studentService.UpdateStudentAsync(student);
                return RedirectToAction(nameof(Index));
            }

            studentViewModel.Teachers = await GetTeachersSelectListAsync();
            return View(studentViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _studentService.GetStudentByIdAsync(
                id.Value,
                includes: query => query.Include(s => s.Teacher)
            );

            if (student == null)
                return NotFound();

            var studentViewModel = _mapper.Map<StudentViewModel>(student);
            return View(studentViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetTeachersSelectListAsync()
        {
            var teachers = await _teacherRepository.GetAllAsync();
            return teachers.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            });
        }
    }
}
