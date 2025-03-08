using Application.DTOs.Request.Comment;
using Application.DTOs.Response;
using Application.Interfaces.Persistence;
using Domain.Entities.Posts;
using Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;

public record CreateCommentCommand(CreateCommentDTO commentModel) : IRequest<GeneralResponse>;

public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, GeneralResponse>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContext;

    public CreateCommentHandler(
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork,
        IUserContextService userContext,
        IPostRepository postRepository
        )
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _postRepository = postRepository;
    }

    public async Task<GeneralResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetCurrentUserId();
        var post = await _postRepository.GetByIdAsync(request.commentModel.PostId);

        if (post == null)
            return new GeneralResponse(false, "Post does not exist in database");

        var comment = new Comment
        {
            Content = request.commentModel.Content,
            PostId = post.Id,
            UserId = userId
        };

        await _commentRepository.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"Comment {comment.Id} was created successfully.");
    }
}
