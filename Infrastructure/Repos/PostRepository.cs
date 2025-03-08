using Domain.Entities.Posts;
using Domain.Repository;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPostByIdWithDetailsAsync()
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostCategory)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Likes)
                    .ThenInclude(l => l.User).ToListAsync();
        }
    }
}
