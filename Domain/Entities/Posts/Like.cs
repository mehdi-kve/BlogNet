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
    [Table("Likes")]
    public class Like: BaseEntity
    {
        public  Guid UserId { get; set; }
        public  ApplicationUser User { get; set; } = null!;

        public  int PostId { get; set; }
        public  Post Post { get; set; } = null!;
    }
}
