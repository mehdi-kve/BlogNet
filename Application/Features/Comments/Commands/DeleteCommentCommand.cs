using MediatR;
using Application.DTOs.Response;
using Domain.Repository;
using Application.Interfaces.Persistence;

public record DeleteCommentCommand(int id) : IRequest<GeneralResponse>;

public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, GeneralResponse>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public DeleteCommentHandler(
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<GeneralResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.GetCurrentUserId();
        var isAdmin = _userContextService.IsAdmin();
        var comment = await _commentRepository.GetByIdAsync(request.id);

        if (comment == null)
            return new GeneralResponse(false, "Comment does not exist in database.");

        if (comment.UserId != userId && !isAdmin)
            return new GeneralResponse(false, "Not Allowed To delete this post");

        _commentRepository.SoftDelete(comment);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"Comment {comment.Id} was deleted successfully.");
    }
}
