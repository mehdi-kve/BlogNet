﻿using Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IPostRepository: IGenericRepository<Post>
    {
        Task<List<Post>> GetPostByIdWithDetailsAsync();
    }
}
