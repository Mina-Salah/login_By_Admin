using static System.Runtime.InteropServices.JavaScript.JSType;
using WebApplication3.Models;
using WebApplication3.ViewModels;
using AutoMapper;

namespace WebApplication3.MappingProfiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            // مابينج من Teacher إلى TeacherViewModel
            CreateMap<Teacher, TeacherViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            // مابينج من TeacherViewModel إلى Teacher
            CreateMap<TeacherViewModel, Teacher>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Students, opt => opt.Ignore()); // تجاهل Students لأنه مش موجود في ViewModel
        }
    }
}
