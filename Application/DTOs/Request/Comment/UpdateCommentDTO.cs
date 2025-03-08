using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Comment
{
    public class UpdateCommentDTO
    {
        [Required(ErrorMessage = "Content is required")]
        [StringLength(200, ErrorMessage = "Content must be between 2 and 50 characters", MinimumLength = 2)]
        public string Content { get; set; }
    }
}
