using AuthServer.Api.Controller;
using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IServicesGeneric<Product, ProductDto> _productService;
        public ProductController(IServicesGeneric<Product, ProductDto> productService)
        {
            _productService= productService;
        }
        [HttpGet]
        public async Task<IActionResult>GetProducts()
        {
            return ActionResultInstance(await _productService.GetAllAsync());
        }
        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _productService.AddAsync(productDto));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _productService.Update(productDto,productDto.Id));
        }
        //api/product/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return ActionResultInstance(await _productService.Remove(id));
        }


    }
}
