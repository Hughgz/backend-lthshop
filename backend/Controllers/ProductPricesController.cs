using AutoMapper;
using backend.Dtos;
using backend.Entities;
using backend.Repositories.EntitiesRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPricesController : ControllerBase
    {
        private readonly ProductPriceRepo _repository;
        private readonly IMapper _mapper;

        public ProductPricesController(ProductPriceRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{productSizeId}")]
        public async Task<IActionResult> GetActiveProductPriceByProductSizeId(int productSizeId)
        {
            var productPrice = await _repository.GetActiveProductPriceByProductSizeId(productSizeId);
            if (productPrice == null)
            {
                return NotFound();
            }

            var productPriceReadDto = _mapper.Map<ProductPriceReadDto>(productPrice);
            return Ok(productPriceReadDto);
        }


        [HttpGet("all-product-price/{productSizeId}")]
        public async Task<IActionResult> GetAllProductPriceByProductSizeId(int productSizeId)
        {
            var productPrices = await _repository.GetAllProductPriceByProductSizeId(productSizeId);
            if (productPrices == null)
            {
                return NotFound();
            }
            var productPricesReadDto = _mapper.Map<IEnumerable<ProductPriceReadDto>>(productPrices);
            return Ok(productPricesReadDto);
        }

        [HttpGet("all-product-price-pending-for-approval")]
        public async Task<IActionResult> GetAllProductPricePendingForApproval()
        {
            var productPrices = await _repository.GetAllProductPricePendingForApproval();
            var productPricesReadDto = _mapper.Map<IEnumerable<ProductPriceReadDto>>(productPrices);
            return Ok(productPricesReadDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductPrice(ProductPriceCreateDto productPriceCreateDto)
        {
            var productPrice = _mapper.Map<ProductPrice>(productPriceCreateDto);
            productPrice.CreatedAt = DateTime.Now;
            productPrice.productPriceStatus = ProductPriceStatus.PendingForApproval;

            await _repository.AddAsync(productPrice);
            return Ok();
        }

        /// <summary>
        /// Khi admin duyệt giá sản phẩm, giá sản phẩm đó sẽ được chuyển sang trạng thái Active đồng thời giá đang active phải chuyển về inactive
        /// </summary>
        /// <param name="productPriceId"></param>
        /// <returns></returns>
        [HttpPost("approve/{productPriceId}")]
        public async Task<IActionResult> ApproveProductPrice(int productPriceId)
        {
            var productPrice = await _repository.GetByIdAsync(productPriceId);
            if (productPrice == null)
            {
                return NotFound();
            }
            var activeProductPrice = await _repository.GetActiveProductPriceByProductSizeId(productPrice.ProductSizeId);
            if (activeProductPrice != null)
            {
                activeProductPrice.productPriceStatus = ProductPriceStatus.Inactive;
                await _repository.UpdateAsync(activeProductPrice);
            }
            productPrice.productPriceStatus = ProductPriceStatus.Active;
            await _repository.UpdateAsync(productPrice);

            return Ok();
        }

        [HttpPost("reject/{productPriceId}")]
        public async Task<IActionResult> RejectProductPrice(int productPriceId)
        {
            var productPrice = await _repository.GetByIdAsync(productPriceId);
            if (productPrice == null)
            {
                return NotFound();
            }

            productPrice.productPriceStatus = ProductPriceStatus.Rejected;
            await _repository.UpdateAsync(productPrice);
            return Ok();
        }
    }
}
