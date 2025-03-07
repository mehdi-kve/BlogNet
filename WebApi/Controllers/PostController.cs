using Application.DTOs.Request.Post;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPost()
        {
            var posts = await _mediator.Send(new GetAllPostsQuery());
            return posts == null ? NotFound() : Ok(posts);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _mediator.Send(new GetPostByIdQuery(id));
            return post == null ? NotFound() : Ok(post);
        }

        [Authorize(Roles = "Author, Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");

            var command = new CreatePostCommand(model);
            var response = await _mediator.Send(command);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize(Roles = "Author,Admin")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");

            var command = new UpdatePostCommand(id, model);
            var response = await _mediator.Send(command);
            if (!response.Flag)
                return NotFound(response);

            return Ok(response);
        }

        [Authorize(Roles = "Author,Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var command = new DeletePostCommand(id);
            var response = await _mediator.Send(command); 
            if (!response.Flag)
                return BadRequest(response);
            
            return Ok(response);
        }
    }
}
