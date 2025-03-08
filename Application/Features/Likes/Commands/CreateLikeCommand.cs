using Application.DTOs.Response;
using Application.Interfaces.Persistence;
using Domain.Entities.Posts;
using Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;

public record CreateLikeCommand(int id) : IRequest<GeneralResponse>;

public class CreateLikeHandler : IRequestHandler<CreateLikeCommand, GeneralResponse>
{
    private readonly ILikeRepository _likeRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContext;

    public CreateLikeHandler(
        ILikeRepository likeRepository,
        IPostRepository postRepository,
        IUnitOfWork unitOfWork,
        IUserContextService userContext

        )
    {
        _likeRepository = likeRepository;
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<GeneralResponse> Handle(CreateLikeCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetCurrentUserId();
        var allPosts = await _postRepository.GetPostByIdWithDetailsAsync(); 

        var post = allPosts.FirstOrDefault(p => p.Id == request.id);
        if (post == null)
            return new GeneralResponse(false, "Post does not exist in database");

        var userLike = post.Likes.FirstOrDefault(l => l.UserId == userId);
        if (userLike != null)
            return new GeneralResponse(false, "You have liked this post before.");

        var like = new Like
        {
            PostId = request.id,
            UserId = userId
        };

        await _likeRepository.AddAsync(like);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"Post ID: {post.Id} was Liked successfully.");
    }
}
