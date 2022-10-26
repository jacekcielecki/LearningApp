using AutoMapper;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.MappingProfiles
{
    public class WsbLearnMappingProfile : Profile
    {
        public WsbLearnMappingProfile()
        {
            //Category
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryRequest, Category>();

            //Question
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<CreateQuestionRequest, Question>();

            //User
            CreateMap<User, UserDto>();
        }
    }
}
