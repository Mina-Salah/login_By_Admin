using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Admin")] // فقط للمستخدمين الذين لديهم دور "Admin"
    public class DeletedDataController : Controller
    {
        private readonly IDeletedDataService _deletedDataService;

        public DeletedDataController(IDeletedDataService deletedDataService)
        {
            _deletedDataService = deletedDataService;
        }

        // عرض البيانات المحذوفة (مثال: الدورات أو المعلمون أو الطلاب)
        public async Task<IActionResult> Index()
        {
            var deletedCourses = await _deletedDataService.GetDeletedDataAsync<Course>();
            var deletedTeachers = await _deletedDataService.GetDeletedDataAsync<Teacher>();
            var deletedStudents = await _deletedDataService.GetDeletedDataAsync<Student>();

            var model = new
            {
                Courses = deletedCourses,
                Teachers = deletedTeachers,
                Students = deletedStudents
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePermanently(int id, string modelName)
        {
            try
            {
                if (modelName == "Course")
                {
                    await _deletedDataService.DeletePermanentlyAsync<Course>(id);
                }
                else if (modelName == "Teacher")
                {
                    await _deletedDataService.DeletePermanentlyAsync<Teacher>(id);
                }
                else if (modelName == "Student")
                {
                    await _deletedDataService.DeletePermanentlyAsync<Student>(id);
                }

                TempData["Success"] = "تم حذف العنصر نهائيًا";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message; // عرض رسالة الخطأ
            }

            return RedirectToAction(nameof(Index));
        }


        // استعادة السجل المحذوف (مثال: دورة أو معلم أو طالب)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id, string modelName)
        {
            try
            {
                if (modelName == "Course")
                {
                    await _deletedDataService.RestoreAsync<Course>(id);
                }
                else if (modelName == "Teacher")
                {
                    await _deletedDataService.RestoreAsync<Teacher>(id);
                }
                else if (modelName == "Student")
                {
                    await _deletedDataService.RestoreAsync<Student>(id);
                }

                TempData["Success"] = "تم استعادة العنصر بنجاح";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message; // عرض رسالة الخطأ
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
