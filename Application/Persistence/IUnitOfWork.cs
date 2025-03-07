using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Persistence
{
    public interface IUnitOfWork: IDisposable
    {
        IPostRepository Post { get; }
        IPostCategoryRepository PostCategory{ get; }
        ICommentRepository Comment { get; }
        ILikeRepository Like { get; }
        Task<int> SaveChangesAsync();
    }
}
