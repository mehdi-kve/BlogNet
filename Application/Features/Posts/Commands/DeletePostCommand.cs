using MediatR;
using Application.DTOs.Response;
using Domain.Repository;
using Application.Interfaces.Persistence;

public record DeletePostCommand(int id) : IRequest<GeneralResponse>;

public class DeletePostHandler : IRequestHandler<DeletePostCommand, GeneralResponse>
{
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;


    public DeletePostHandler(
        IPostRepository postRepository, 
        IUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<GeneralResponse> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.GetCurrentUserId();
        var isAdmin = _userContextService.IsAdmin();
        var post = await _postRepository.GetByIdAsync(request.id);

        if (post == null)
            return new GeneralResponse(false, "Post does not exist in database.");

        if (post.UserId != userId && !isAdmin)
            return new GeneralResponse(false, "Not Allowed To delete this post");

        _postRepository.SoftDelete(post);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"Post ID: {post.Id} was deleted successfully.");
    }
}
