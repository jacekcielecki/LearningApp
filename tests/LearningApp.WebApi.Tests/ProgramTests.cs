using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LearningApp.WebApi.Tests
{
    public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly List<Type> _controllerTypes;

        public ProgramTests(WebApplicationFactory<Program> factory)
        {
            _controllerTypes = typeof(Program)
                .Assembly
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(ControllerBase)))
                .ToList();

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _controllerTypes.ForEach(controllerType => services.AddScoped(controllerType));
                });
            });
        }

        [Fact]
        public void ConfigureServices_ForControllers_RegisterAllDependencies()
        {
            //arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory?.CreateScope();

            //assert
            _controllerTypes.ForEach(type =>
            {
                var controller = scope?.ServiceProvider.GetService(type);
                controller.Should().NotBeNull();
            });
        }
    }
}
