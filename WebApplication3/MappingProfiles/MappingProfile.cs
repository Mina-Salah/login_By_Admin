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
         
            CreateMap<CourseViewModel, Course>(); 
            CreateMap<Course, CourseViewModel>()
                  .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Name));
           
            
            CreateMap<Student, StudentViewModel>()
                           .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                           .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Name));

            CreateMap<StudentViewModel, Student>();
        }
    }
    }

