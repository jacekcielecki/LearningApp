using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Extensions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.Question;

namespace WSBLearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("{categoryId}")]
        public ActionResult<IEnumerable<QuestionDto>> GetAllByCategory(int categoryId)
        {
            var dtos = _questionService.GetAllByCategory(categoryId);

            return Ok(dtos);
        }

        [HttpGet("{categoryId}/{level}")]
        [Authorize(Roles = "Admin, User")]
        public ActionResult<IEnumerable<QuestionDto>> GetQuiz(int categoryId, [FromRoute] int level)
        {
            var userId = HttpContext.GetUserId();
            var dtos = _questionService.GetQuiz(categoryId, level, userId);

            return Ok(dtos);
        }

        [HttpPost("{categoryId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([FromBody] CreateQuestionRequest questionRequest, int categoryId)
        {
            var questionId = _questionService.Create(questionRequest, categoryId);

            return Created("Success", string.Format(CrudMessages.CreateEntitySuccess, "Question", questionId));
        }



        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<QuestionDto> Update(int id, [FromBody] UpdateQuestionRequest updateQuestionRequest)
        {
            QuestionDto questionDto = _questionService.Update(id, updateQuestionRequest);

            return Ok(questionDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _questionService.Delete(id);

            return Ok(string.Format(CrudMessages.DeleteEntitySuccess, "Question"));
        }
    }
}