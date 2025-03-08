using MediatR;
using Application.DTOs.Response;
using Domain.Repository;
using Application.Interfaces.Persistence;

public record DeleteLikeCommand(int id) : IRequest<GeneralResponse>;

public class DeleteLikeHandler : IRequestHandler<DeleteLikeCommand, GeneralResponse>
{
    private readonly IPostRepository _postRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public DeleteLikeHandler(
        ILikeRepository likeRepository,
        IUnitOfWork unitOfWork,
        IUserContextService userContextService,
        IPostRepository postRepository)
    {
        _likeRepository = likeRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
        _postRepository = postRepository;
    }

    public async Task<GeneralResponse> Handle(DeleteLikeCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.GetCurrentUserId();
        var allPosts = await _postRepository
            .GetAllWithIncludesAsync(p => p.Likes);

        var post = allPosts.FirstOrDefault(p => p.Id == request.id);
        if (post == null)
            return new GeneralResponse(false, "Post does not exist in database");

        var userLike = post.Likes.FirstOrDefault(l => l.UserId == userId);
        if (userLike == null)
            return new GeneralResponse(false, "You have not liked this post before.");

        _likeRepository.SoftDelete(userLike);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"You unlike post {post.Id} successfully.");
    }
}
