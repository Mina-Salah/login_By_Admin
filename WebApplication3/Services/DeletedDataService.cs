using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Services
{
    public class DeletedDataService : IDeletedDataService
    {
        private readonly SchoolContext _context;

        public DeletedDataService(SchoolContext context)
        {
            _context = context;
        }

        // الحصول على البيانات المحذوفة لكل موديل
        public async Task<List<T>> GetDeletedDataAsync<T>() where T : class
        {
            if (typeof(T) == typeof(Course))
            {
                return await _context.Courses.Where(c => c.IsDeleted).ToListAsync() as List<T>;
            }
            else if (typeof(T) == typeof(Teacher))
            {
                return await _context.Teachers.Where(t => t.IsDeleted).ToListAsync() as List<T>;
            }
            else if (typeof(T) == typeof(Student))
            {
                return await _context.Students.Where(s => s.IsDeleted).ToListAsync() as List<T>;
            }
            return null;
        }

        // حذف السجلات المحذوفة نهائيًا
        public async Task DeletePermanentlyAsync<T>(int id) where T : class
        {
            if (typeof(T) == typeof(Course))
            {
                var course = await _context.Courses.FindAsync(id);
                if (course != null)
                {
                    // تحقق إذا كان هناك أي طلاب مرتبطين بهذا الكورس
                    var students = await _context.Students.Where(s => s.CourseId == course.Id).ToListAsync();
                    if (students.Any())
                    {
                        throw new InvalidOperationException($"لا يمكن حذف الكورس لأنه يحتوي على {students.Count} طالب/طلاب مرتبطين به.");
                    }

                    _context.Courses.Remove(course);
                    await _context.SaveChangesAsync();
                }
            }
            else if (typeof(T) == typeof(Teacher))
            {
                var teacher = await _context.Teachers.FindAsync(id);
                if (teacher != null)
                {
                    // تحقق إذا كان هناك أي كورسات مرتبطه بهذا المدرس
                    var courses = await _context.Courses.Where(c => c.TeacherId == teacher.Id).ToListAsync();
                    if (courses.Any())
                    {
                        throw new InvalidOperationException($"لا يمكن حذف المعلم لأنه مرتبط بكورس/كورسات.");
                    }

                    _context.Teachers.Remove(teacher);
                    await _context.SaveChangesAsync();
                }
            }
            else if (typeof(T) == typeof(Student))
            {
                var student = await _context.Students.FindAsync(id);
                if (student != null)
                {
                    // تحقق إذا كان الطالب مرتبط بكورس
                    var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == student.CourseId);
                    if (course != null)
                    {
                        throw new InvalidOperationException($"لا يمكن حذف الطالب لأنه مرتبط بكورس {course.Name}.");
                    }

                    _context.Students.Remove(student);
                    await _context.SaveChangesAsync();
                }
            }
        }


        // استعادة السجلات المحذوفة
        public async Task RestoreAsync<T>(int id) where T : class
        {
            if (typeof(T) == typeof(Course))
            {
                var course = await _context.Courses.FindAsync(id);
                if (course != null)
                {
                    course.IsDeleted = false;
                    _context.Courses.Update(course);
                    await _context.SaveChangesAsync();
                }
            }
            else if (typeof(T) == typeof(Teacher))
            {
                var teacher = await _context.Teachers.FindAsync(id);
                if (teacher != null)
                {
                    teacher.IsDeleted = false;
                    _context.Teachers.Update(teacher);
                    await _context.SaveChangesAsync();
                }
            }
            else if (typeof(T) == typeof(Student))
            {
                var student = await _context.Students.FindAsync(id);
                if (student != null)
                {
                    student.IsDeleted = false;
                    _context.Students.Update(student);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
