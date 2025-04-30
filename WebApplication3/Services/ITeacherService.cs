using WebApplication3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication3.Services
{
    public interface ITeacherService
    {
        Task<List<Teacher>> GetAllTeachersAsync();
        Task<Teacher> GetTeacherByIdAsync(int id);
        Task AddTeacherAsync(Teacher teacher);
        Task UpdateTeacherAsync(Teacher teacher);
        Task DeleteTeacherAsync(int id);
        Task<List<Teacher>> GetDeletedTeachersAsync(); // إضافة الطريقة لجلب المعلمين المحذوفين
        Task RestoreTeacherAsync(int id); // إضافة الطريقة لاسترجاع المعلم
    }
}
