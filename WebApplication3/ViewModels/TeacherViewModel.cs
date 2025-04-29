using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class TeacherViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        public string Name { get; set; }
    }
}
