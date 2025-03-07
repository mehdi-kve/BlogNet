using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response.Like
{
    public class LikeDTO
    {
        public string LikedBy { get; set; }
        public DateTime LikedAt { get; set; }
    }
}
