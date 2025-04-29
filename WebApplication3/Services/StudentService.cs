using WebApplication3.Models;
using WebApplication3.Repositories;
using WebApplication3.Services;

public class StudentService : IStudentService
{
    private readonly IGenericRepository<Student> _studentRepository;

    public StudentService(IGenericRepository<Student> studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync(Func<IQueryable<Student>, IQueryable<Student>>? includes = null)
    {
        return await _studentRepository.GetAllAsync(includes);
    }

    public async Task<Student?> GetStudentByIdAsync(int id, Func<IQueryable<Student>, IQueryable<Student>>? includes = null)
    {
        return await _studentRepository.GetByIdAsync(id, includes);
    }

    public async Task AddStudentAsync(Student student)
    {
        await _studentRepository.AddAsync(student);
        await _studentRepository.SaveAsync();
    }

    public async Task UpdateStudentAsync(Student student)
    {
        _studentRepository.Update(student);
        await _studentRepository.SaveAsync();
    }

    public async Task DeleteStudentAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student != null)
        {
            _studentRepository.Delete(student);
            await _studentRepository.SaveAsync();
        }
    }
}
