using WebApplication3.ViewModels;

namespace WebApplication3.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentViewModel>> GetAllStudentsAsync();
        Task<StudentViewModel?> GetStudentByIdAsync(int id);
        Task AddStudentAsync(StudentViewModel studentViewModel);
        Task UpdateStudentAsync(StudentViewModel studentViewModel);
        Task DeleteStudentAsync(int id);
    }
}