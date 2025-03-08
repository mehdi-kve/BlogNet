using Application.DTOs.Request.Comment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("GetByPost/{postId}")]
        public async Task<IActionResult> GetLikesByPostId(int postId)
        {
            var comments = await _mediator.Send(new GetLikeByPostQuery(postId));
            return comments == null ? NotFound() : Ok(comments);
        }

        [Authorize]
        [HttpPost("Create/{postid}")]
        public async Task<IActionResult> CreateLike(int postid)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");

            var command = new CreateLikeCommand(postid);
            var response = await _mediator.Send(command);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("Delete/{postid}")]
        public async Task<IActionResult> DeleteComment(int postid)
        {
            var command = new DeleteLikeCommand(postid);
            var response = await _mediator.Send(command);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
