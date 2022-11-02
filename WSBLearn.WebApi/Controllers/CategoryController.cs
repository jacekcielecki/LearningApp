﻿using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Constants;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests;

namespace WSBLearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // POST api/<CategoryController>
        [HttpPost]
        public ActionResult Post([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            var result = _categoryService.Create(createCategoryRequest);

            return Created("Success", string.Format(CrudMessages.CreateEntitySuccess, "Category", result));
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDto>> Get()
        {
            IEnumerable<CategoryDto>? categories =  _categoryService.GetAll();

            return Ok(categories);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public ActionResult<CategoryDto> GetById(int id)
        {
            CategoryDto category = _categoryService.GetById(id);

            return Ok(category);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public ActionResult<CategoryDto> Put(int id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            CategoryDto category = _categoryService.Update(id, updateCategoryRequest);

            return Ok(category);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _categoryService.Delete(id);

            return Ok(string.Format(CrudMessages.DeleteEntitySuccess, "Category"));
        }
    }
}