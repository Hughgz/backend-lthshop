using AutoMapper;
using backend.Dtos;
using backend.Repositories.EntitiesRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistedItemsController : ControllerBase
    {
        private readonly WishlistedItemRepo _wishlistedItemRepo;
        private readonly IMapper _mapper;

        public WishlistedItemsController(WishlistedItemRepo wishlistedItemRepo, IMapper mapper)
        {
            _wishlistedItemRepo = wishlistedItemRepo;
            _mapper = mapper;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetWishlistedItemsByCustomerId(int customerId)
        {
            var wishlistedItems = await _wishlistedItemRepo.GetWishlistedItemsByCustomerId(customerId);
            if (wishlistedItems == null)
            {
                return NotFound();
            }
            var wishlistedItemsReadDto = _mapper.Map<IEnumerable<WishlistedItemReadDto>>(wishlistedItems);
            return Ok(wishlistedItemsReadDto);
        }
    }
}
