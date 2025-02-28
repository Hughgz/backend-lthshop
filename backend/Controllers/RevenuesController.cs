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
    public class RevenuesController : ControllerBase
    {

        private readonly RevenueRepo _revenueRepo;
        private readonly IMapper _mapper;

        public RevenuesController(RevenueRepo revenueRepo, IMapper mapper)
        {
            _revenueRepo = revenueRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRevenues()
        {
            var revenues = await _revenueRepo.GetAllAsync();
            var revenueDtos = _mapper.Map<IEnumerable<RevenueReadDto>>(revenues);
            return Ok(revenueDtos);
        }

        [HttpGet("period")] 
        public async Task<IActionResult> GetRevenuesWithPeriod([FromQuery] DateOnly DateFrom, [FromQuery] DateOnly DateTo)
        {
            if (DateFrom == default || DateTo == default)
                return BadRequest("DateFrom and DateTo are required.");

            var revenues = await _revenueRepo.GetRevenueWithPeriod(DateFrom, DateTo);
            return Ok(_mapper.Map<IEnumerable<RevenueReadDto>>(revenues));
        }
    }
}
