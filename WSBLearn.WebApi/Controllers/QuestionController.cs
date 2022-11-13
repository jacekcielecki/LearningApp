using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.Question;
using WSBLearn.Domain.Entities;

namespace WSBLearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly AuthorizationHandlerContext _context;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(IQuestionService questionService, AuthorizationHandlerContext context, ILogger<QuestionController> logger)
        {
            _questionService = questionService;
            _context = context;
            _logger = logger;
        }

        // POST api/<QuestionController>
        [HttpPost("{categoryId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([FromBody] CreateQuestionRequest questionRequest, int categoryId)
        {
            var questionId = _questionService.Create(questionRequest, categoryId);

            return Created("Success", string.Format(CrudMessages.CreateEntitySuccess, "Question", questionId));
        }

        // GET: api/<QuestionController>
        [HttpGet("{categoryId}")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<QuestionDto>> GetAllByCategory(int categoryId)
        {
            IEnumerable<QuestionDto> questionDtos = _questionService.GetAllByCategory(categoryId);

            return Ok(questionDtos);
        }

        // GET: api/<QuestionController>
        [HttpGet("{categoryId}/{level}")] 
        [AllowAnonymous]
        public ActionResult<string?> GetQuiz(int categoryId, [FromRoute] int level)
        {
            IEnumerable<QuestionDto> questionDtos = _questionService.GetQuiz(categoryId, level);
            var userId = _context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return Ok(userId);
        }

        // PUT api/<QuestionController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<QuestionDto> Update(int id, [FromBody] UpdateQuestionRequest updateQuestionRequest)
        {
            QuestionDto questionDto = _questionService.Update(id, updateQuestionRequest);

            return Ok(questionDto);
        }

        // DELETE api/<QuestionController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _questionService.Delete(id);

            return Ok(string.Format(CrudMessages.DeleteEntitySuccess, "Question"));
        }
    }
}