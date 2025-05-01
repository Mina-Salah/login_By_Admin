using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Services
{
    public class CourseService : ICourseService
    {
        private readonly SchoolContext _context;

        public CourseService(SchoolContext context)
        {
            _context = context;
        }

        // إحضار كل الكورسات
        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses
                                 .Include(c => c.Teacher)  // ربط المعلم مع الكورس
                                 .Where(c => !c.IsDeleted)  // استبعاد الكورسات المحذوفة
                                 .ToListAsync();
        }

        // إحضار كورس حسب الـ ID
        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                                 .Include(c => c.Teacher)  // ربط المعلم مع الكورس
                                 .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);  // إحضار الكورس غير المحذوف
        }

        // إضافة كورس
        public async Task AddCourseAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        // تعديل كورس
        public async Task UpdateCourseAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        // حذف كورس (Soft Delete)
        public async Task DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                course.IsDeleted = true;  // تعيين الكورس كـ "محذوف"
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
            }
        }
    }
}
