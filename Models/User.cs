﻿using System.ComponentModel.DataAnnotations;

namespace Skinalyze.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string password { get; set; }
       
    }   
}
