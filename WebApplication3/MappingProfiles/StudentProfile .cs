using AutoMapper;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.MappingProfiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            // من Student إلى StudentViewModel
            CreateMap<Student, StudentViewModel>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Name))
                .ForMember(dest => dest.Teachers, opt => opt.Ignore()); // Teachers دي مستخدمة بس فى Create/Edit

            // العكس: من StudentViewModel إلى Student
            CreateMap<StudentViewModel, Student>();
        }
    }
}
