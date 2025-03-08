using Application.DTOs.Request.Post;
using Application.DTOs.Response;
using Application.Interfaces.Persistence;
using Domain.Entities.Posts;
using Domain.Repository;
using MediatR;

public record CreatePostCommand(CreatePostDTO postModel) : IRequest<GeneralResponse>;

public class CreatePostHandler : IRequestHandler<CreatePostCommand, GeneralResponse>
{
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContext;

    public CreatePostHandler(
        IPostRepository postRepository,
        IUnitOfWork unitOfWork,
        IUserContextService userContext
        )
    {
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<GeneralResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetCurrentUserId();
        var post = new Post
        {
            Title = request.postModel.Title,
            Content = request.postModel.Content,
            PostCategoryId = request.postModel.CategoryId,
            UserId = userId
        };

        await _postRepository.AddAsync(post);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"Post ID: {post.Id} was created successfully.");
    }
}
