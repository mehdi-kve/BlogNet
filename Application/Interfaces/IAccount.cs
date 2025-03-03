using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccount
    {
        Task<GeneralResponse> CreateAdmin();

        Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model);

        Task<LoginResponse> LoginAcoountAsync(LoginDto model);

        Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model);

        Task<GeneralResponse> CreateRoleAsync(CreateRoleDTO model);

        Task<IEnumerable<GetRoleDTO>> GetRoleAsync();

        Task<IEnumerable<GetUserWithRolesDTO>> GetUserWithRoleAsync();

        Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model);
    }
}
