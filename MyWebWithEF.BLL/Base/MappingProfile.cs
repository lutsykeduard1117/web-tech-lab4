using AutoMapper;
using MyWebWithEF.BLL.Components.CategoryComponent.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Мапінг для Post
        CreateMap<Post, PostDto>().ReverseMap();

        // Мапінг для Category
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}
