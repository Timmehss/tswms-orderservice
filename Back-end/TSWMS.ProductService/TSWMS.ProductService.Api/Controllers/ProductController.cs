using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TSWMS.ProductService.Api.Dto;
using TSWMS.ProductService.Shared.Interfaces;

namespace TSWMS.ProductService.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;
        private readonly IMapper _mapper;

        public ProductController(IProductManager productManager, IMapper mapper)
        {
            _productManager = productManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productManager.GetProducts();

                if (products == null || !products.Any())
                {
                    return NotFound("No products found.");
                }

                return Ok(_mapper.Map<List<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }
    }
}
