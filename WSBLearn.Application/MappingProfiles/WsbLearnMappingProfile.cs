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
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<CreateQuestionRequest, Question>();
        }
    }
}
