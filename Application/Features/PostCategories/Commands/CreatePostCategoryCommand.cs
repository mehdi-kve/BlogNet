using Application.DTOs.Request.PostCategory;
using Application.DTOs.Response;
using Application.Interfaces.Persistence;
using Domain.Entities.Posts;
using Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;

public record CreatePostCategoryCommand(CreatePostCategoryDTO postCategoryModel) : IRequest<GeneralResponse>;

public class CreatePostCategoryHandler : IRequestHandler<CreatePostCategoryCommand, GeneralResponse>
{
    private readonly IPostCategoryRepository _postCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePostCategoryHandler(
        IPostCategoryRepository postCategoryRepository,
        IUnitOfWork unitOfWork
        )
    {
        _postCategoryRepository = postCategoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GeneralResponse> Handle(CreatePostCategoryCommand request, CancellationToken cancellationToken)
    {
        var postCategory = new PostCategory
        {
            Name = request.postCategoryModel.Name,
            Description = request.postCategoryModel.Description
        };

        await _postCategoryRepository.AddAsync(postCategory);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"postCategory ID: {postCategory.Id} was created successfully.");
    }
}
