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
    public class PurchaseReceiptsController : ControllerBase
    {
        private readonly PurchaseReceiptRepo _purchaseReceiptRepo;
        private readonly PurchaseReceiptDetailRepo _purchaseReceiptDetailRepo;
        private readonly IMapper _mapper;

        public PurchaseReceiptsController(PurchaseReceiptRepo purchaseReceiptRepo, PurchaseReceiptDetailRepo purchaseReceiptDetailRepo, IMapper mapper)
        {
            _purchaseReceiptRepo = purchaseReceiptRepo;
            _purchaseReceiptDetailRepo = purchaseReceiptDetailRepo; 
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPurchaseReceipts()
        {
            var purchaseReceipts = await _purchaseReceiptRepo.GetAllAsync();
            var purchaseReceiptsResult = _mapper.Map<IEnumerable<PurchaseReceiptReadDto>>(purchaseReceipts);
            foreach (var purchaseReceipt in purchaseReceiptsResult)
            {
                var purchaseReceiptDetails = _purchaseReceiptDetailRepo.GetManyByPurchaseReceiptId(purchaseReceipt.PurchaseReceiptID);
                var purchaseReceiptDetailsDto = _mapper.Map<IEnumerable<PurchaseReceiptDetailReadDto>>(purchaseReceiptDetails);
                purchaseReceipt.Details = purchaseReceiptDetailsDto;
            }
            return Ok(purchaseReceiptsResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseReceiptById(int id)
        {
            var purchaseReceipt = await _purchaseReceiptRepo.GetByIdAsync(id);
            if (purchaseReceipt == null)
                return NotFound();
            return Ok(_mapper.Map<PurchaseReceipt>(purchaseReceipt));
        }

        [HttpGet("by-filter")]
        public async Task<IActionResult> GetPurchaseReceiptsByFilter([FromQuery] DateTime? date, [FromQuery] decimal? minTotalPrice, [FromQuery] decimal? maxTotalPrice, [FromQuery] PurchaseReceiptStatus? status)
        {
            var purchaseReceipts = await _purchaseReceiptRepo.GetManyByFilter(pr =>
                (!date.HasValue || pr.DateTime.Date == date.Value.Date) &&
                (!minTotalPrice.HasValue || pr.TotalPrice >= minTotalPrice.Value) &&
                (!maxTotalPrice.HasValue || pr.TotalPrice <= maxTotalPrice.Value) &&
                (!status.HasValue || pr.Status == status.Value)
            );
            return Ok(_mapper.Map<IEnumerable<PurchaseReceipt>>(purchaseReceipts));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseReceipt([FromBody] PurchaseReceiptCreateDto purchaseReceiptCreateDto)
        {
            if (purchaseReceiptCreateDto == null)
                return BadRequest();

            var createPurchaseReceipt = _mapper.Map<PurchaseReceipt>(purchaseReceiptCreateDto);

            var createdReceipt = await _purchaseReceiptRepo.AddAsync(createPurchaseReceipt);
            return CreatedAtAction(nameof(GetPurchaseReceiptById), new { id = createdReceipt.PurchaseReceiptID }, createdReceipt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseReceipt(int id, [FromBody] PurchaseReceipt purchaseReceipt)
        {
            if (purchaseReceipt == null || id != purchaseReceipt.PurchaseReceiptID)
                return BadRequest();

            var existingReceipt = await _purchaseReceiptRepo.GetByIdAsync(id);
            if (existingReceipt == null)
                return NotFound();

            await _purchaseReceiptRepo.UpdateAsync(purchaseReceipt);
            return NoContent();
        }


    }
}
