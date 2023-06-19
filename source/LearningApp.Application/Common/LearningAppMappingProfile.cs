using AutoMapper;
using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Achievement;
using LearningApp.Application.Requests.Category;
using LearningApp.Application.Requests.CategoryGroup;
using LearningApp.Application.Requests.Question;
using LearningApp.Domain.Entities;

namespace LearningApp.Application.Common
{
    public class LearningAppMappingProfile : Profile
    {
        public LearningAppMappingProfile()
        {
            //Achievement
            CreateMap<Achievement, AchievementDto>().ReverseMap();
            CreateMap<CreateAchievementRequest, Achievement>();

            //Category
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryRequest, Category>();

            //CategoryGroup
            CreateMap<CategoryGroup, CategoryGroupDto>().ReverseMap();
            CreateMap<CreateCategoryGroupRequest, CategoryGroup>();

            //CategoryProgress
            CreateMap<CategoryProgress, CategoryProgressDto>().ReverseMap();

            //LevelProgress
            CreateMap<LevelProgress, LevelProgressDto>().ReverseMap();

            //User
            CreateMap<User, UserDto>();

            CreateMap<User, UserRankingDto>()
                .ForMember(u => u.ExperiencePoints,
                    opt => opt.MapFrom(e => e.UserProgress.ExperiencePoints))
                .ForMember(u => u.Level,
                    opt => opt.MapFrom(e => e.UserProgress.Level));

            //UserProgress
            CreateMap<UserProgress, UserProgressDto>().ReverseMap();

            //Question
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<CreateQuestionRequest, Question>();

            //Role
            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
