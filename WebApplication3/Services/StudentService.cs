using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using System.Linq;

namespace WebApplication3.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolContext _context;

        public StudentService(SchoolContext context)
        {
            _context = context;
        }

        // الحصول على كل الطلاب مع المدرسين والدورات
        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.Teacher)
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }

        // الحصول على طالب واحد بالمعرف مع المدرس والدورات
        public async Task<Student> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Teacher)
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        // إضافة طالب جديد مع ربطه بالدورات
        public async Task AddAsync(Student student, List<int> courseIds)
        {
            _context.Students.Add(student);

            foreach (var courseId in courseIds)
            {
                var studentCourse = new StudentCourse
                {
                    StudentId = student.Id,
                    CourseId = courseId
                };
                _context.StudentCourses.Add(studentCourse);
            }

            await _context.SaveChangesAsync();
        }

        // تحديث طالب مع ربطه بالدورات
        public async Task UpdateAsync(Student student, List<int> courseIds)
        {
            _context.Students.Update(student);

            // حذف الدورات القديمة المربوطة بالطالب
            var existingStudentCourses = _context.StudentCourses
                .Where(sc => sc.StudentId == student.Id)
                .ToList();
            _context.StudentCourses.RemoveRange(existingStudentCourses);

            // إضافة الدورات الجديدة
            foreach (var courseId in courseIds)
            {
                var studentCourse = new StudentCourse
                {
                    StudentId = student.Id,
                    CourseId = courseId
                };
                _context.StudentCourses.Add(studentCourse);
            }

            await _context.SaveChangesAsync();
        }

        // حذف طالب
        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                student.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
