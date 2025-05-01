using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        [Display(Name = "Student Name")]
        public string Name { get; set; }

        [Display(Name = "Is Deleted?")]
        public bool IsDeleted { get; set; }

        // Course Info
        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Display(Name = "Course Name")]
        public string? CourseName { get; set; }

        // Teacher Info
        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }

        [Display(Name = "Teacher Name")]
        public string? TeacherName { get; set; }

        // For dropdown lists
        public List<CourseViewModel>? AvailableCourses { get; set; }
        public List<TeacherViewModel>? AvailableTeachers { get; set; }
    }
}
