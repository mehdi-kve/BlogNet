using Application.DTOs.Request.Comment;
using Application.DTOs.Response;
using Application.Interfaces.Persistence;
using Domain.Repository;
using MediatR;

public record UpdateCommentCommand(int id, UpdateCommentDTO updateCommentModel) : IRequest<GeneralResponse>;

public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, GeneralResponse>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public UpdateCommentHandler(
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<GeneralResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.GetCurrentUserId();
        var isAdmin = _userContextService.IsAdmin();
        var comment = await _commentRepository.GetByIdAsync(request.id);

        if (comment == null)
            return new GeneralResponse(false, "Comment does not exist in database.");

        if (comment.UserId != userId && !isAdmin)
            return new GeneralResponse(false, "Not Allowed To update this post");

        comment.Content = request.updateCommentModel.Content;

        _commentRepository.Update(comment);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"Comment {comment.Id} was updated successfully."); ;
    }
}
