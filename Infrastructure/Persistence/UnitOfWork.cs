using Application.Interfaces;
using Application.Interfaces.Persistence;
using Domain.Entities.Posts;
using Domain.Repository;
using Infrastructure.DataContext;
using Infrastructure.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IPostRepository Post { get; private set;}
        public IPostCategoryRepository PostCategory { get; private set;}
        public ICommentRepository Comment { get; private set;}
        public ILikeRepository Like { get; private set;}

        public UnitOfWork(ApplicationDbContext context, IAccount account)
        {
            _context = context;
            Post = new PostRepository(context);
            PostCategory = new PostCategoryRepository(context);
            Comment = new CommentRepository(context);
            Like = new LikeRepository(context);
        }

        public async Task<int> SaveChangesAsync() 
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose() 
        {
            _context.Dispose();
        }
    }
}
