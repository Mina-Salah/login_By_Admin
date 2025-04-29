using WebApplication3.Models;
using WebApplication3.Repositories;

namespace WebApplication3.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IGenericRepository<Teacher> _teacherRepository;

        public TeacherService(IGenericRepository<Teacher> teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _teacherRepository.GetAllAsync();
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            return await _teacherRepository.GetByIdAsync(id);
        }

        public async Task AddTeacherAsync(Teacher teacher)
        {
            await _teacherRepository.AddAsync(teacher);
            await _teacherRepository.SaveAsync();
        }

        public async Task UpdateTeacherAsync(Teacher teacher)
        {
            _teacherRepository.Update(teacher);
            await _teacherRepository.SaveAsync();
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            if (teacher != null)
            {
                teacher.IsDeleted = true;
                _teacherRepository.Update(teacher);
                await _teacherRepository.SaveAsync();
            }
        }
    }
}
