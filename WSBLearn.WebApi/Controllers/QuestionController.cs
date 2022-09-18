using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Services;

namespace WSBLearn.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly IQuestionService _questionService;

        public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService)
        {
            _logger = logger;
            _questionService = questionService;
        }

        [HttpGet(Name = "GetQuestions")]
        public IActionResult GetAll()
        {
            _logger.LogWarning("This is a test of logger!");
            return Ok(_questionService.GetQuestions());
        }
    }
}
