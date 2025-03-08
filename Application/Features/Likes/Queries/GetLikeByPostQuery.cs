using Application.DTOs.Response.Like;
using AutoMapper;
using Domain.Repository;
using MediatR;

public record GetLikeByPostQuery(int id) : IRequest<List<LikeDTO>>;

public class GetLikeByPostHandler : IRequestHandler<GetLikeByPostQuery, List<LikeDTO>>
{
    private readonly IPostRepository _postRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IMapper _mapper;

    public GetLikeByPostHandler(
        IPostRepository postRepository,
        IMapper mapper,
        ILikeRepository likeRepository)
    {
        _postRepository = postRepository;
        _likeRepository = likeRepository;
        _mapper = mapper;
    }

    public async Task<List<LikeDTO>> Handle(GetLikeByPostQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.id);
        if (post == null)
            throw new Exception("Post not found");

        var likes = await _likeRepository.GetAllAsync(l => l.PostId == request.id, c => c.User);
        return _mapper.Map<List<LikeDTO>>(likes);
    }
}
