﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User";
    }

}
