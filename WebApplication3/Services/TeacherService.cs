using WebApplication3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication3.Data;

namespace WebApplication3.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly SchoolContext _context;

        public TeacherService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            return await _context.Teachers
                .Where(t => !t.IsDeleted) // جلب المعلمين الذين لم يتم حذفهم
                .ToListAsync();
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            return await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted); // جلب معلم غير محذوف
        }

        public async Task AddTeacherAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTeacherAsync(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                teacher.IsDeleted = true; // قم بتغيير حالة المعلم إلى محذوف
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Teacher>> GetDeletedTeachersAsync()
        {
            return await _context.Teachers
                .Where(t => t.IsDeleted) // جلب المعلمين الذين تم حذفهم
                .ToListAsync();
        }

        public async Task RestoreTeacherAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                teacher.IsDeleted = false; // استرجاع المعلم بتغيير حالة الحذف
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteTeacherPermanentlyAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<string> DeleteTeacherPermanentlyWithMessageAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                // تحقق من ارتباط المدرس بكورسات
                var courses = await _context.Courses
                    .Where(c => c.TeacherId == teacher.Id)
                    .ToListAsync();

                if (courses.Any())
                {
                    // إذا كان مرتبطًا بكورسات، إرجاع رسالة
                    return "المدرس مرتبط بكورسات ولا يمكن حذفه نهائيًا.";
                }

                // إذا لم يكن هناك ارتباط بكورسات، يتم الحذف النهائي
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
                return "تم حذف المعلم نهائيًا بنجاح";
            }

            return "المدرس غير موجود.";
        }
    }
}
