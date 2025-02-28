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
        private readonly ProductSizeRepo _productSizeRepo;
        private readonly IMapper _mapper;

        public PurchaseReceiptsController(PurchaseReceiptRepo purchaseReceiptRepo, PurchaseReceiptDetailRepo purchaseReceiptDetailRepo, ProductSizeRepo productSizeRepo, IMapper mapper)
        {
            _purchaseReceiptRepo = purchaseReceiptRepo;
            _purchaseReceiptDetailRepo = purchaseReceiptDetailRepo;
            _productSizeRepo = productSizeRepo;
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
            var purchaseReceiptResult = await _purchaseReceiptRepo.GetByIdAsync(id);
            var purchaseReceipt = _mapper.Map<PurchaseReceiptReadDto>(purchaseReceiptResult);
            if (purchaseReceipt == null)
                return NotFound();
            var purchaseReceiptDetails = _purchaseReceiptDetailRepo.GetManyByPurchaseReceiptId(purchaseReceipt.PurchaseReceiptID);
            var purchaseReceiptDetailsDto = _mapper.Map<IEnumerable<PurchaseReceiptDetailReadDto>>(purchaseReceiptDetails);
            purchaseReceipt.Details = purchaseReceiptDetailsDto;
            return Ok(purchaseReceipt);
        }

        [HttpGet("by-filter")]
        public async Task<IActionResult> GetPurchaseReceiptsByFilter([FromQuery] DateTime? date, [FromQuery] decimal? minTotalPrice, [FromQuery] decimal? maxTotalPrice, [FromQuery] PurchaseReceiptStatus? status)
        {
            var purchaseReceiptsResults = await _purchaseReceiptRepo.GetManyByFilter(pr =>
                (!date.HasValue || pr.DateTime.Date == date.Value.Date) &&
                (!minTotalPrice.HasValue || pr.TotalPrice >= minTotalPrice.Value) &&
                (!maxTotalPrice.HasValue || pr.TotalPrice <= maxTotalPrice.Value) &&
                (!status.HasValue || pr.Status == status.Value)
            );

            var purchaseReceipts = _mapper.Map<IEnumerable<PurchaseReceiptReadDto>>(purchaseReceiptsResults);
            foreach (var purchaseReceipt in purchaseReceipts)
            {
                var purchaseReceiptDetails = _purchaseReceiptDetailRepo.GetManyByPurchaseReceiptId(purchaseReceipt.PurchaseReceiptID);
                var purchaseReceiptDetailsDto = _mapper.Map<IEnumerable<PurchaseReceiptDetailReadDto>>(purchaseReceiptDetails);
                purchaseReceipt.Details = purchaseReceiptDetailsDto;
            }
            return Ok(purchaseReceipts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseReceipt([FromBody] PurchaseReceiptCreateDto purchaseReceiptCreateDto)
        {
            if (purchaseReceiptCreateDto == null)
                return BadRequest();

            var createPurchaseReceipt = _mapper.Map<PurchaseReceipt>(purchaseReceiptCreateDto);
            var createPurchaseReceiptDetails = _mapper.Map<IEnumerable<PurchaseReceiptDetail>>(purchaseReceiptCreateDto.Details);
            var createdReceipt = await _purchaseReceiptRepo.AddAsync(createPurchaseReceipt);
            foreach (var item in createPurchaseReceiptDetails)
            {
                await _purchaseReceiptDetailRepo.AddAsync(item);
            }
            return CreatedAtAction(nameof(GetPurchaseReceiptById), new { id = createdReceipt.PurchaseReceiptID }, createdReceipt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseReceipt(int id, [FromBody] PurchaseReceiptReadDto purchaseReceipt)
        {
            if (purchaseReceipt == null || id != purchaseReceipt.PurchaseReceiptID)
                return BadRequest();

            var existingReceipt = await _purchaseReceiptRepo.GetByIdAsync(id);
            if (existingReceipt == null)
                return NotFound();

            var purchaseReceiptEntity = _mapper.Map<PurchaseReceipt>(purchaseReceipt);
            await _purchaseReceiptRepo.UpdateAsync(purchaseReceiptEntity);
            
            return NoContent();
        }

        [HttpPost("comfirm/{id}")]
        public async Task<IActionResult> ConfirmPurchaseReceipt(int id, IEnumerable<PurchaseReceiptDetailUpdateDto> purchaseReceiptDetailUpdateDtos)
        {
            var purchaseReceipt = await _purchaseReceiptRepo.GetByIdAsync(id);
            if (purchaseReceipt == null)
                return NotFound();

            // Update purchase receipt status
            purchaseReceipt.Status = PurchaseReceiptStatus.Confirmed;
            await _purchaseReceiptRepo.UpdateAsync(purchaseReceipt);

            // Update stock quantity
            foreach (var detail in purchaseReceiptDetailUpdateDtos)
            {
                // Update real quantity to purchase receipt detail
                var purchaseReceiptDetail = await _purchaseReceiptDetailRepo.GetByIdAsync(detail.PurchaseReceiptDetailID);
                purchaseReceiptDetail.RealQuantity = detail.RealQuantity.Value;
                
                await _purchaseReceiptDetailRepo.UpdateAsync(purchaseReceiptDetail);

                // Update stock quantity to product size
                var productSize = await _productSizeRepo.GetByIdAsync(detail.ProductSizeID);
                productSize.StockQuantity += detail.RealQuantity.Value;
                productSize.RealQuantity += detail.RealQuantity.Value;

                await _productSizeRepo.UpdateAsync(productSize);
            }

            return NoContent();
        }
    }
}
