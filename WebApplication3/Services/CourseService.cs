using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using WebApplication3.Repositories;
using WebApplication3.Services;

public class CourseService : ICourseService
{
    private readonly IGenericRepository<Course> _courseRepository;
    private readonly IGenericRepository<Teacher> _teacherRepository;

    public CourseService(IGenericRepository<Course> courseRepository, IGenericRepository<Teacher> teacherRepository)
    {
        _courseRepository = courseRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _courseRepository.GetAllAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _courseRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Course course)
    {
        await _courseRepository.AddAsync(course);
        await _courseRepository.SaveAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        _courseRepository.Update(course);
        await _courseRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course != null)
        {
            _courseRepository.Delete(course);
            await _courseRepository.SaveAsync();
        }
    }

    public async Task<IEnumerable<Course>> GetCoursesWithTeacherAsync()
    {
        return await _courseRepository.GetAllAsync(query => query.Include(c => c.Teacher));
    }

    public async Task<Course?> GetCourseWithStudentsAsync(int id)
    {
        return await _courseRepository.GetByIdAsync(id, query =>
            query
                .Include(c => c.Teacher)
                .Include(c => c.StudentCourses)
                    .ThenInclude(sc => sc.Student));
    }

    public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
    {
        return await _teacherRepository.GetAllAsync();
    }


}
