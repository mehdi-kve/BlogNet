﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Post
{
    public class CreatePostDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(50, ErrorMessage = "Title must be between 2 and 50 characters", MinimumLength = 2)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [StringLength(200, ErrorMessage = "Content must be between 2 and 50 characters", MinimumLength = 2)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Category Id is required")]
        public int CategoryId { get; set; }
    }
}
