using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الكورس مطلوب")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يجب اختيار المدرس")]
        [Display(Name = "المعلم")]
        public int TeacherId { get; set; }

        public string? TeacherName { get; set; } // للعرض فقط
    }
}
