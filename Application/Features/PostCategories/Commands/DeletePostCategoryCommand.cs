using MediatR;
using Application.DTOs.Response;
using Domain.Repository;
using Application.Interfaces.Persistence;

public record DeletePostCategoryCommand(int id) : IRequest<GeneralResponse>;

public class DeletePostCategoryHandler : IRequestHandler<DeletePostCategoryCommand, GeneralResponse>
{
    private readonly IPostCategoryRepository _postCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;


    public DeletePostCategoryHandler(
        IPostCategoryRepository postCategoryRepository,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _postCategoryRepository = postCategoryRepository;
    }

    public async Task<GeneralResponse> Handle(DeletePostCategoryCommand request, CancellationToken cancellationToken)
    {
        var postCategory = await _postCategoryRepository.GetByIdAsync(request.id);

        if (postCategory == null)
            return new GeneralResponse(false, "Post Category does not exist in database.");

        _postCategoryRepository.SoftDelete(postCategory);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"Post ID: {postCategory.Id} was deleted successfully.");
    }
}
