using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign Key
        public int TeacherId { get; set; }

        // Navigation Property
        public Teacher Teacher { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }

    }
}
