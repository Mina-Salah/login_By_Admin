using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;


        // Navigation Property
        public ICollection<Course> Courses { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
