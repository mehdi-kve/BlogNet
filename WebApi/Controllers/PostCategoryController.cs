using Application.DTOs.Request.Post;
using Application.DTOs.Request.PostCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCategoryController: ControllerBase
    {
        private readonly IMediator _mediator;

        public PostCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPostCategories()
        {
            var postCategories = await _mediator.Send(new GetAllPostCategoriesQuery());
            return postCategories == null ? NotFound() : Ok(postCategories);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePostCategory([FromBody] CreatePostCategoryDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");

            var command = new CreatePostCategoryCommand(model);
            var response = await _mediator.Send(command);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdatePostCategory(int id, [FromBody] UpdatePostCategoryDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");

            var command = new UpdatePostCategoryCommand(id, model);
            var response = await _mediator.Send(command);
            if (!response.Flag)
                return NotFound(response);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePostCategory(int id)
        {
            var command = new DeletePostCategoryCommand(id);
            var response = await _mediator.Send(command);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
