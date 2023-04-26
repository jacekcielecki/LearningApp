using System.Net;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class AchievementControllerTests
    {
        [Fact]
        public async Task GetAllAsync_WithExistingItems_ReturnsItems()
        {
            //arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            //act
            var response = await client.GetAsync("/api/Achievement");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
