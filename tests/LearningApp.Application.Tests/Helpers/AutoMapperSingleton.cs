using AutoMapper;
using LearningApp.Application.Common;

namespace LearningApp.Application.Tests.Helpers
{
    public class AutoMapperSingleton
    {
        private static IMapper? _mapper;
        public static IMapper? Mapper
        {
            get
            {
                if (_mapper != null) return _mapper;
                var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new WsbLearnMappingProfile()); });
                var mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
                return _mapper;
            }
        }
    }
}
