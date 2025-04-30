using AutoMapper;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Teacher, TeacherViewModel>().ReverseMap();
            CreateMap<Course, CourseViewModel>().ReverseMap();

            CreateMap<Student, StudentViewModel>()
                .ForMember(dest => dest.CourseIds, opt => opt.MapFrom(
                    src => src.StudentCourses.Select(sc => sc.CourseId)))
                .ReverseMap()
                .ForMember(dest => dest.StudentCourses, opt => opt.MapFrom(
                    src => src.CourseIds.Select(id => new StudentCourse { CourseId = id })));
        }
    }
}
