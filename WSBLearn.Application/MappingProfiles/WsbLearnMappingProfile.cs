using AutoMapper;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests.Achievement;
using WSBLearn.Application.Requests.Category;
using WSBLearn.Application.Requests.Question;
using WSBLearn.Application.Responses;
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

            CreateMap<User, UserRankingResponse>()
                .ForMember(u => u.ExperiencePoints,
                    opt => opt.MapFrom(e => e.UserProgress.ExperiencePoints))
                .ForMember(u => u.Level,
                    opt => opt.MapFrom(e => e.UserProgress.Level));

            CreateMap<Achievement, AchievementDto>().ReverseMap();
            CreateMap<CreateAchievementRequest, Achievement>();
        }
    }
}
