﻿using Microsoft.AspNetCore.Mvc;
using ProductCategoryAPI.DTO;
using ProductCategoryAPI.models;
using ProductCategoryAPI.Services;

namespace ProductCategoryAPI.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ICategoryService categoryService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
        }
        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ActionResult> Get()
        {
            return Ok(await _productService.Get());
        }

        [HttpGet()]
        [Route("api/[controller]/{id}")]
        public async Task<ActionResult> Get(string id)
        {
            try
            {
                var product = await _productService.Get(id);

                if (product == null)
                {
                    return NotFound($"Product not found by id:{id}!");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ActionResult> Create(ProductDTO productDto)
        {
            Product product;
            Category category = null;
            if (productDto != null)
            {
                if (productDto.CategoryId != null)
                {
                    category = await _categoryService.Get(productDto.CategoryId);
                    if (category == null)
                    {
                        return NotFound("Category not found");
                    }
                    
                }
                product = await _productService.Create(productDto, category);
                return Ok(product);
            }
            return NotFound("Product can not be null");
        }
        [HttpPut()]
        [Route("api/[controller]/Update/{id}")]
        public async Task<IActionResult> Update(string id, ProductDTO productDto)
        {
            try
            {
                Category category = null;
                var product = await _productService.Get(id);

                if (product == null)
                {
                    return NotFound($"Product not found by id:{id}");
                }
                if(productDto.CategoryId != null)
                {
                    category = await _categoryService.Get(productDto.CategoryId);
                    if (category == null)
                    {
                        return NotFound($"Category not found by categoryId:{productDto.CategoryId}");
                    }
                }

                await _productService.Update(id, productDto, category);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var product = await _productService.Get(id);
                if (product == null)
                {
                    return NotFound();
                }
                await _productService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}