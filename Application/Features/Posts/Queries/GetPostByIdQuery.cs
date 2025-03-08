using Application.DTOs.Response.Posts;
using AutoMapper;
using Domain.Repository;
using MediatR;

public record GetPostByIdQuery(int id) : IRequest<PostDTO>;

public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostDTO>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GetPostByIdHandler(IPostRepository postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<PostDTO> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        //var post = await _postRepository.GetByIdAsync(request.id);
        var posts = await _postRepository
            .GetAllWithIncludesAsync(p => p.User, p => p.PostCategory, p => p.Comments, p => p.Likes);

        var result = _mapper.Map<PostDTO>(posts.FirstOrDefault(p => p.Id == request.id));

        return result ?? throw new Exception("Post not found");
    }
}
