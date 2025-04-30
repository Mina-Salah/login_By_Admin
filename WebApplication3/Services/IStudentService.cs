using WebApplication3.Models;

namespace WebApplication3.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task AddAsync(Student student, List<int> courseIds); // تم إضافة List<int> courseIds
        Task UpdateAsync(Student student, List<int> courseIds); // تم إضافة List<int> courseIds
        Task DeleteAsync(int id);
    }
}
