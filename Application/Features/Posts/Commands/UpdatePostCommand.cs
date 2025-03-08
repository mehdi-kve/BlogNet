using Application.DTOs.Request.Post;
using Application.DTOs.Response;
using Application.Interfaces.Persistence;
using Domain.Entities.Posts;
using Domain.Repository;
using MediatR;
using static Application.Extensions.Constant;

public record UpdatePostCommand(int id, UpdatePostDTO updatePostModel) : IRequest<GeneralResponse>;

public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, GeneralResponse>
{
    private readonly IPostRepository _postRepository;
    private readonly IPostCategoryRepository _postCategory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public UpdatePostHandler(
        IPostRepository postRepository, 
        IPostCategoryRepository postCategory,
        IUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
        _postCategory = postCategory;
        _userContextService = userContextService;
    }

    public async Task<GeneralResponse> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.GetCurrentUserId();
        var isAdmin = _userContextService.IsAdmin();
        var post = await _postRepository.GetByIdAsync(request.id);

        if (post == null)
            return new GeneralResponse(false, "Post does not exist in database.");

        if (post.UserId != userId && !isAdmin)
            return new GeneralResponse(false, "Not Allowed To update this post");

        post.Title = request.updatePostModel.Title;
        post.Content = request.updatePostModel.Content;

        _postRepository.Update(post);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"Post ID: {post.Id} was updated successfully."); ;
    }
}
