using Application.DTOs.Request.Comment;
using Application.DTOs.Request.Post;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetByPost/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(int postId)
        {
            var comments = await _mediator.Send(new GetCommentByPostQuery(postId));
            return comments == null ? NotFound() : Ok(comments);
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");

            var command = new CreateCommentCommand(model);
            var response = await _mediator.Send(command);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");

            var command = new UpdateCommentCommand(id, model);
            var response = await _mediator.Send(command);
            if (!response.Flag)
                return NotFound(response);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var command = new DeleteCommentCommand(id);
            var response = await _mediator.Send(command);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
