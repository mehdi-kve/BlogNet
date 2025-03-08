using Application.DTOs.Response.PostCategory;
using Application.DTOs.Response.Posts;
using AutoMapper;
using Domain.Repository;
using MediatR;

public record GetAllPostCategoriesQuery : IRequest<List<PostCategoryDTO>>;

public class GetAllPostCategoriesHandler : IRequestHandler<GetAllPostCategoriesQuery, List<PostCategoryDTO>>
{
    private readonly IPostCategoryRepository _postCategoryRepository;
    private readonly IMapper _mapper;

    public GetAllPostCategoriesHandler(
        IPostCategoryRepository postCategoryRepository, 
        IMapper mapper)
    {
        _postCategoryRepository = postCategoryRepository;
        _mapper = mapper;
    }

    public async Task<List<PostCategoryDTO>> Handle(GetAllPostCategoriesQuery request, CancellationToken cancellationToken)
    {
        var postCategories = await _postCategoryRepository.GetAllAsync();
        var result = _mapper.Map<List<PostCategoryDTO>>(postCategories);
        return result;
    }
}
