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
    [Table("Comments")]
    public class Comment : BaseEntity
    {
        public string Content { get; set; } = string.Empty;

        [Column("AuthorId")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
    }
}
