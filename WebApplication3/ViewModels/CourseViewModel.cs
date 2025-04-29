using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication3.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }

        public List<string>? Students { get; set; }
        public IEnumerable<SelectListItem>? TeachersList { get; set; }

    }

}
