using Application.DTOs.Response.Comment;
using AutoMapper;
using Domain.Repository;
using MediatR;

public record GetCommentByPostQuery(int id) : IRequest<List<CommentDTO>>;

public class GetCommentByPostHandler : IRequestHandler<GetCommentByPostQuery, List<CommentDTO>>
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public GetCommentByPostHandler(
        IPostRepository postRepository, 
        IMapper mapper,
        ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _mapper = mapper;
        _commentRepository = commentRepository;
    }

    public async Task<List<CommentDTO>> Handle(GetCommentByPostQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(request.id);
        if(post == null)
            throw new Exception("Post not found");

        var comments = await _commentRepository.GetAllAsync(c => c.PostId == request.id, c => c.User);
        return _mapper.Map<List<CommentDTO>>(comments);
    }
}
