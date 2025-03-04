using AutoMapper;
using backend.Dtos;
using backend.Repositories.EntitiesRepo;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockHistoriesController : ControllerBase
    {
        private readonly StockHistoryRepo _stockHistoryRepo;
        private readonly IMapper _mapper;

        public StockHistoriesController(StockHistoryRepo stockHistoryRepo, IMapper mapper)
        {
            _stockHistoryRepo = stockHistoryRepo;
            _mapper = mapper;
        }

        [HttpGet("{productSizeId}")]
        public async Task<IActionResult> GetStockHistoryByProductSizeId(int productSizeId)
        {
            var stockHistories = await _stockHistoryRepo.GetAllByProductSizeId(productSizeId);
            var stockHistoriesResult = _mapper.Map<IEnumerable<StockHistoryReadDto>>(stockHistories);
            return Ok(stockHistoriesResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetStockHistoryByProductSizeIdAndPeriod(int productSizeId, DateTime startDate, DateTime endDate)
        {
            var stockHistories = await _stockHistoryRepo.GetManyByProductSizeIdAndPeriod(productSizeId, startDate, endDate);
            var stockHistoriesResult = _mapper.Map<IEnumerable<StockHistoryReadDto>>(stockHistories);
            return Ok(stockHistoriesResult);
        }
    }
}
