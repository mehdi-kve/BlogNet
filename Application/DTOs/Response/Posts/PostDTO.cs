using Application.DTOs.Response.Comment;
using Application.DTOs.Response.Like;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response.Posts
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }

        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
        public List<LikeDTO> Likes { get; set; } = new List<LikeDTO>();
    }
}
