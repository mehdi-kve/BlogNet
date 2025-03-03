using Application.DTOs.Request.Account;
using Application.DTOs.Response.Account;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Application.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IdentityRole, CreateRoleDTO>();
            CreateMap<IdentityRole, GetRoleDTO>();
        }
    }
}
