using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يجب اختيار معلم")]
        public int TeacherId { get; set; }

        public string? TeacherName { get; set; }
        public int CourseId { get; set; }

        public string? courseName { get; set; }

        // عشان نعرض DropDownList
        public IEnumerable<SelectListItem>? Teachers { get; set; }
        public IEnumerable<SelectListItem>? Courses { get; set; }
    }
}
