﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; } 
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
    }

}
