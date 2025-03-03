using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Application.DTOs.Response.Account;
using Application.Interfaces;
using Domain.Entities.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Extensions;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Infrastructure.Repos
{
    public class AccountRepository
        (IMapper _mapper,
         RoleManager<IdentityRole> roleManager,
         UserManager<ApplicationUser> userManager,IConfiguration config,
         SignInManager<ApplicationUser> signInManager, ApplicationDbContext context) : IAccount
    {
        private async Task<ApplicationUser> FindUserByEmailAsync(string email)
            => await userManager.FindByEmailAsync(email);

        private async Task<IdentityRole> FindRoleByNameAsync(string roleName)
            => await roleManager.FindByNameAsync(roleName);

        /*public async Task<IEnumerable<GetRoleDTO>> GetRoleAsync()
            => (await roleManager.Roles.ToListAsync()).Adapt<IEnumerable<GetRoleDTO>>();*/

        public async Task<IEnumerable<GetRoleDTO>> GetRoleAsync()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<GetRoleDTO>>(roles);
        }

        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            try
            {
                var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
                var credential = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

                var roles = await userManager.GetRolesAsync(user);

                var userClaims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, (await userManager.GetRolesAsync(user)).FirstOrDefault().ToString()),
                    new Claim("Fullname", user.Name),
                };

                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credential
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch { return null; }

        }

        private static string CheckResponse(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(_ => _.Description);
                return string.Join(Environment.NewLine, errors);
            }
            return null;
        }

        public async Task<GeneralResponse> AssignUserToRole(ApplicationUser user, IdentityRole role)
        {
            if (user is null || role is null)
                return new GeneralResponse(false, "Model State cannot be empty");

            var roleExist = await FindRoleByNameAsync(role.Name);

            if (roleExist == null) 
            {
                var roleDto = _mapper.Map<CreateRoleDTO>(role);
                await CreateRoleAsync(roleDto);
            }

            IdentityResult result = await userManager.AddToRolesAsync(user, new List<string> {role.Name});

            string error = CheckResponse(result);

            if (!string.IsNullOrEmpty(error))
                return new GeneralResponse(false, error);
            else
                return new GeneralResponse(true, $"{user.Name} assigned to {role.Name} role.");
        } 

        public async Task<GeneralResponse> CreateAdmin()
        {
            try 
            {
                if ((await FindRoleByNameAsync(Constant.Role.Admin)) != null)
                        return new GeneralResponse(false, "setting has already set before!");

                var admin = new CreateAccountDTO()
                {
                    Name = Constant.Admin.Name,
                    Password = Constant.Admin.Password,
                    EmailAddress = Constant.Admin.Email,
                    Role = Constant.Role.Admin
                };
                await CreateAccountAsync(admin);
                return new GeneralResponse(true, "setting saved successfully");
            }
            catch (Exception ex)
            {
                return new GeneralResponse(false, ex.Message);
            }
        }

        public async Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model)
        {
            try 
            {
                if (await FindUserByEmailAsync(model.EmailAddress) != null)
                    return new GeneralResponse(false, "Sorry, user is already created");

                var user = new ApplicationUser()
                {
                    Name= model.Name,
                    UserName = model.EmailAddress,
                    Email = model.EmailAddress,
                    PasswordHash = model.Password
                };

                var result = await userManager.CreateAsync(user, model.Password);
                string error = CheckResponse(result);

                if (!string.IsNullOrEmpty(error))
                    return new GeneralResponse(false, error);

                var (flag, message) = await AssignUserToRole(user, new IdentityRole() { Name = model.Role});
                return new GeneralResponse(flag, message);
            }
            catch(Exception ex)
            {
                return new GeneralResponse(false, ex.Message);
            }
        }

        public async Task<GeneralResponse> CreateRoleAsync(CreateRoleDTO model)
        {
            var existingRole = await roleManager.FindByNameAsync(model.Name);
            if (existingRole != null)
                return new GeneralResponse(false, "Role already exists");

            var newRole = new IdentityRole { Name = model.Name };
            var result = await roleManager.CreateAsync(newRole);
            var response = CheckResponse(result);

            if (!string.IsNullOrEmpty(response))
                return new GeneralResponse(false, response);
            else
                return new GeneralResponse(true, "Role Changed");

        }

        public async Task<IEnumerable<GetUserWithRolesDTO>> GetUserWithRoleAsync()
        {
            var allUsers = await userManager.Users.ToListAsync();
            if (allUsers is null)
                return null;

            var List = new List<GetUserWithRolesDTO>();
            foreach(var user in allUsers) 
            {
                var getUserRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                var getRoleInfo = await roleManager.Roles.FirstOrDefaultAsync(r  => r.Name.ToLower() == getUserRole.ToLower());
                List.Add(new GetUserWithRolesDTO() 
                {
                    Name = user.Name,
                    Email = user.Email,
                    RoleId = getRoleInfo.Id,
                    RoleName = getRoleInfo.Name
                });
            }
            return List;
        }

        public async Task<LoginResponse> LoginAcoountAsync(LoginDto model)
        {
            try 
            {
                var user = await FindUserByEmailAsync(model.EmailAddress);
                if (user == null)
                    return new LoginResponse(false, "User Not Found");

                SignInResult result;
                try
                {
                    result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                }
                catch 
                {
                    return new LoginResponse(false, "Invalid Credential");
                }

                if (!result.Succeeded)
                    return new LoginResponse(false, "Invalid Credential");

                string jwtToken = await GenerateToken(user);
                string refreshToken = GenerateRefreshToken();
                if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken))
                    return new LoginResponse(false, "Error occured while logging in account, please contact administration");
                else 
                {

                    var saveResult = await SaveRefreshToken(user.Id, refreshToken);
                    if (saveResult.Flag)
                        return new LoginResponse(true, $"{user.Name} successfully logged in", jwtToken, refreshToken);
                    else
                        return new LoginResponse();
                }
            }
            catch(Exception ex) 

            {
                return new LoginResponse(false, ex.Message);
            }
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model)
        {
            var token = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == model.Token);
            if (token == null)
                return new LoginResponse();

            var user = await userManager.FindByIdAsync(token.UserID);
            string newToken = await GenerateToken(user);
            string newRefreshToken = GenerateRefreshToken();
            var saveResult = await SaveRefreshToken(user.Id, newToken);
            if (saveResult.Flag)
                return new LoginResponse(true, $"{user.Name} Successfully re-logged in", newToken, newRefreshToken);
            else
                return new LoginResponse();
        }

        public async Task<GeneralResponse> SaveRefreshToken(string userId, string token)
        {

            try 
            {
                var user = await context.RefreshTokens.FirstOrDefaultAsync(t => t.UserID == userId);
                if (user == null)
                    context.RefreshTokens.Add(new RefreshToken() { UserID = userId, Token = token });
                else 
                    user.Token = token;

                await context.SaveChangesAsync();
                return new GeneralResponse(true, null!);
            }
            catch(Exception ex) 
            {
                return new GeneralResponse(false, ex.Message);
            }

        }

        public async Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model)
        {
            if (await FindRoleByNameAsync(model.RoleName) is null)
                return new GeneralResponse(false, "Role not found");

            if (await FindUserByEmailAsync(model.UserEmail) is null) 
                return new GeneralResponse(false, "User not found");

            var user = await FindUserByEmailAsync(model.UserEmail);
            var previousRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var removeOldRole = await userManager.RemoveFromRoleAsync(user, previousRole);
            var error = CheckResponse(removeOldRole);
            if (!string.IsNullOrEmpty(error))
                return new GeneralResponse(false, error);

            var result = await userManager.AddToRoleAsync(user, model.RoleName);
            var response = CheckResponse(result);
            if (!string.IsNullOrEmpty(error))
                return new GeneralResponse(false, response);
            else
                return new GeneralResponse(true, "Role Changed");
        }

    }
}
