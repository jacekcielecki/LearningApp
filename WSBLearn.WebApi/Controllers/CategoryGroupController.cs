using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.CategoryGroup;

namespace WSBLearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryGroupController : ControllerBase
    {
        private ICategoryGroupService _categoryGroupService;

        public CategoryGroupController(ICategoryGroupService categoryGroupService)
        {
            _categoryGroupService = categoryGroupService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var entities = _categoryGroupService.GetAll();
            return Ok(entities);
        }

        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            var entity = _categoryGroupService.GetById(id);
            return Ok(entity);
        }

        [HttpPost]
        public ActionResult Post([FromBody] CreateCategoryGroupRequest request)
        {
            var id = _categoryGroupService.Create(request);
            return Ok(id);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] UpdateCategoryGroupRequest request)
        {
            var entity = _categoryGroupService.Update(id, request);
            return Ok(entity);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _categoryGroupService.Delete(id);
            return Ok();
        }

        [HttpPut("addCategory/{id:int}/{categoryId:int}")]
        public ActionResult AddCategory(int id, int categoryId)
        {
            var entity = _categoryGroupService.AddCategory(id, categoryId);
            return Ok(entity);
        }

        [HttpPut("removeCategory/{id:int}/{categoryId:int}")]
        public ActionResult RemoveCategory(int id, int categoryId)
        {
            var entity = _categoryGroupService.RemoveCategory(id, categoryId);
            return Ok(entity);
        }
    }
}
