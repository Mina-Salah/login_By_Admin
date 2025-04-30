using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TeacherId { get; set; }
        public List<int> CourseIds { get; set; } = new();
        // لعرض أسماء المدرسين والدورات في الفيو
        public List<SelectListItem> Teachers { get; set; }
        public List<SelectListItem> Courses { get; set; }
    }
}
