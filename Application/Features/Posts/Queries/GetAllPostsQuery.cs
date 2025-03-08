using Application.DTOs.Response.Posts;
using AutoMapper;
using Domain.Repository;
using MediatR;

public record GetAllPostsQuery : IRequest<List<PostDTO>>;

public class GetAllPostsHandler : IRequestHandler<GetAllPostsQuery, List<PostDTO>>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GetAllPostsHandler(IPostRepository postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<List<PostDTO>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetPostByIdWithDetailsAsync();
        var result = _mapper.Map<List<PostDTO>>(posts);
        return result;
    }
}
