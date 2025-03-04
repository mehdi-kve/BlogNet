using Domain.Entities.Authentication;
using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Posts
{
    [Table("Posts")]
    public class Post: BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsPublished { get; set; } = false;

        [Column("AuthorId")]
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? PostCategoryId { get; set; }
        public PostCategory PostCategory { get; set; }

        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public Post()
        {
            LikesCount = 0;
            CommentsCount = 0;
        }
    }
}
