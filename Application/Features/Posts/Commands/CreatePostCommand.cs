using Application.DTOs.Request.Post;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Persistence;
using Domain.Entities.Authentication;
using Domain.Entities.Posts;
using Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;

public record CreatePostCommand(CreatePostDTO postModel) : IRequest<GeneralResponse>;

public class CreatePostHandler : IRequestHandler<CreatePostCommand, GeneralResponse>
{
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContext;

    public CreatePostHandler(
        IPostRepository postRepository,
        IUnitOfWork unitOfWork,
        IUserContextService userContext
        )
    {
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<GeneralResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var user = _userContext.GetCurrentUserId();
        if (user == null)
            return new GeneralResponse(false, "user does not exist in database.");

        var post = new Post
        {
            Title = request.postModel.Title,
            Content = request.postModel.Content,
            PostCategoryId = request.postModel.CategoryId,
            UserId = user
        };

        await _postRepository.AddAsync(post);
        await _unitOfWork.SaveChangesAsync();

        return new GeneralResponse(true, $"Post {post.Id} was created successfully.");
    }
}
