using WebApplication3.Models;

namespace WebApplication3.Services
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teacher>> GetAllTeachersAsync();
        Task<Teacher> GetTeacherByIdAsync(int id);
        Task AddTeacherAsync(Teacher teacher);
        Task UpdateTeacherAsync(Teacher teacher);
        Task DeleteTeacherAsync(int id);

        Task<IEnumerable<Teacher>> GetDeletedTeachersAsync();
        Task RestoreTeacherAsync(int id);

    }
}
