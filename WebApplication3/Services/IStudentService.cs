using WebApplication3.Models;
    namespace WebApplication3.Services
    {
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync(Func<IQueryable<Student>, IQueryable<Student>>? includes = null);
        Task<Student?> GetStudentByIdAsync(int id, Func<IQueryable<Student>, IQueryable<Student>>? includes = null);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);
    }

}