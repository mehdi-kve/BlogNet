using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccount account) : ControllerBase
    {
        [HttpPost("identify/create")]
        public async Task<ActionResult<GeneralResponse>> CreateAccount(CreateAccountDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");
            var response = await account.CreateAccountAsync(model);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("identify/login")]
        public async Task<ActionResult<GeneralResponse>> LoginAccount(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");
            var response = await account.LoginAcoountAsync(model);

            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("identify/refresh-token")]
        public async Task<ActionResult<GeneralResponse>> RefreshToken(RefreshTokenDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");
            var response = await account.RefreshTokenAsync(model);

            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("identify/role/create")]
        public async Task<ActionResult<GeneralResponse>> CreateRole(CreateRoleDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");
            var response = await account.CreateRoleAsync(model);

            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("identify/role/list")]
        public async Task<ActionResult<IEnumerable<GetRoleDTO>>> GetRoles()
        {
            return Ok(await account.GetRoleAsync());
        }

        [HttpPost("/setting")]
        public async Task<ActionResult> CreateAdmin()
        {
            var response = await account.CreateAdmin();
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("identify/users-with-roles")]
        public async Task<ActionResult<IEnumerable<GetUserWithRolesDTO>>> GetUserWithRoles()
        {
            return Ok(await account.GetUserWithRoleAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("identity/change-role")]
        public async Task<ActionResult<GeneralResponse>> ChangeUserRole(ChangeUserRoleRequestDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model Cannot be null");
            var response = await account.ChangeUserRoleAsync(model);

            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
