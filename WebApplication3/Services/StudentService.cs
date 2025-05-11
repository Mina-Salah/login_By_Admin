using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApplication3.Repositories;
using WebApplication3.ViewModels;
using WebApplication3.Models;
using WebApplication3.fillspec;

namespace WebApplication3.Services
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IGenericRepository<Student> studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentViewModel>> GetAllStudentsAsync()
        {
            var spec = new StudentWithCourseAndTeacherSpecification();
            var students = await _studentRepository.GetAllWithSpecAsync(spec);
            return _mapper.Map<IEnumerable<StudentViewModel>>(students);
        }

        public async Task<StudentViewModel?> GetStudentByIdAsync(int id)
        {
            var spec = new StudentWithCourseAndTeacherSpecification(id);
            var student = await _studentRepository.GetBySpecAsync(spec);
            return student == null ? null : _mapper.Map<StudentViewModel>(student);
        }


        public async Task AddStudentAsync(StudentViewModel studentViewModel)
        {
            var student = _mapper.Map<Student>(studentViewModel);
            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveAsync();
        }

        public async Task UpdateStudentAsync(StudentViewModel studentViewModel)
        {
            var existingStudent = await _studentRepository.GetByIdAsync(studentViewModel.Id);
            if (existingStudent == null)
                throw new Exception("Student not found");

            _mapper.Map(studentViewModel, existingStudent);
            _studentRepository.Update(existingStudent);
            await _studentRepository.SaveAsync();
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                throw new Exception("Student not found");

            // استخدام Soft Delete
            student.IsDeleted = true;
            _studentRepository.Update(student);
            await _studentRepository.SaveAsync();
        }
    }
}