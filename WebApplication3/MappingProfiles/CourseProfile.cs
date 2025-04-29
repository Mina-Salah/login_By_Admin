using AutoMapper;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseViewModel>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Name))
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src =>
                    src.StudentCourses.Select(sc => sc.Student.Name).ToList()));

            CreateMap<CourseViewModel, Course>();
        }
    }
}
