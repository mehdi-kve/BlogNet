using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Post
{
    public class UpdatePostDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(50, ErrorMessage = "Role name must be between 2 and 50 characters", MinimumLength = 2)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [StringLength(200, ErrorMessage = "Role name must be between 2 and 50 characters", MinimumLength = 2)]
        public string Content { get; set; }
    }
}
