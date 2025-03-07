using Application.DTOs.Request.Account;
using Application.DTOs.Response.Account;
using Application.DTOs.Response.Comment;
using Application.DTOs.Response.Like;
using Application.DTOs.Response.Posts;
using AutoMapper;
using Domain.Entities.Posts;
using Microsoft.AspNetCore.Identity;

namespace Application.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IdentityRole, CreateRoleDTO>();
            CreateMap<IdentityRole, GetRoleDTO>();

            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.PostCategory.Name))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes));

            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User.Name));

            CreateMap<Like, LikeDTO>()
                .ForMember(dest => dest.LikedBy, opt => opt.MapFrom(src => src.User.Name));
        }
    }
}
