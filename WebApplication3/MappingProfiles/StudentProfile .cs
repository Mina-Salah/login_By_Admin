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
                .ForMember(dest => dest.courseName, opt => opt.MapFrom(src => src.Course.Name)) // ✅ أضف هذا السطر
                .ForMember(dest => dest.Teachers, opt => opt.Ignore())
                .ForMember(dest => dest.Courses, opt => opt.Ignore()); // إذا هتستخدمها في Create/Edit

            // العكس: من StudentViewModel إلى Student
            CreateMap<StudentViewModel, Student>();
        }
    }
}
