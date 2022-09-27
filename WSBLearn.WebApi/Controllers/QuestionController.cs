using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests;

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

        // POST api/<QuestionController>
        [HttpPost]
        public ActionResult Post([FromBody] CreateQuestionRequest questionRequest)
        {
            var result = _questionService.Create(questionRequest);

            return Created("Success", string.Format(CrudMessages.CreateEntitySuccess, "Category", result));
        }

        // GET: api/<QuestionController>
        [HttpGet]
        public ActionResult<IEnumerable<QuestionDto>> Get()
        {
            IEnumerable<QuestionDto>? questionDtos = _questionService.GetAll();

            return Ok(questionDtos);
        }

        // GET api/<QuestionController>/5
        [HttpGet("{id}")]
        public ActionResult<QuestionDto> GetById(int id)
        {
            QuestionDto questionDto = _questionService.GetById(id);

            return Ok(questionDto);
        }

        // PUT api/<QuestionController>/5
        [HttpPut("{id}")]
        public ActionResult<QuestionDto> Put(int id, [FromBody] UpdateQuestionRequest updateQuestionRequest)
        {
            QuestionDto questionDto = _questionService.Update(id, updateQuestionRequest);

            return Ok(questionDto);
        }

        // DELETE api/<QuestionController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _questionService.Delete(id);

            return Ok(string.Format(CrudMessages.DeleteEntitySuccess, "Question"));
        }
    }
}