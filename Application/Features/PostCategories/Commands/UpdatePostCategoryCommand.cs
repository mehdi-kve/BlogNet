using Application.DTOs.Request.PostCategory;
using Application.DTOs.Response;
using Application.Interfaces.Persistence;
using Domain.Repository;
using MediatR;

public record UpdatePostCategoryCommand(int id, UpdatePostCategoryDTO updatePostCatModel) : IRequest<GeneralResponse>;

public class UpdatePostCategoryHandler : IRequestHandler<UpdatePostCategoryCommand, GeneralResponse>
{
    private readonly IPostCategoryRepository _postCategory;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePostCategoryHandler(
        IPostCategoryRepository postCategory,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _postCategory = postCategory;
    }

    public async Task<GeneralResponse> Handle(UpdatePostCategoryCommand request, CancellationToken cancellationToken)
    {

        var postCategory = await _postCategory.GetByIdAsync(request.id);

        if (postCategory == null)
            return new GeneralResponse(false, "Post Category does not exist in database.");

        postCategory.Name = request.updatePostCatModel.Name;
        postCategory.Description = request.updatePostCatModel.Description;

        _postCategory.Update(postCategory);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"Post ID: {postCategory.Id} was updated successfully."); ;
    }
}
