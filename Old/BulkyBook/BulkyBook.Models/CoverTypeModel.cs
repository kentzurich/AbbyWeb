﻿using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class CoverTypeModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Cover Type")]
        public string Name { get; set; }
    }
}
