using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Entities;
using backend.Repositories.EntitiesRepo;
using backend.Dtos;
using AutoMapper;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IRepo<Customer> _customerRepo;
        private readonly IMapper _mapper;

        public CustomersController(IRepo<Customer> customerRepo, IMapper mapper)
        {
            _customerRepo = customerRepo;
            _mapper = mapper;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerReadDto>>> GetCustomers()
        {
            var customers = await _customerRepo.GetAllAsync();
            var results = _mapper.Map<IEnumerable<CustomerReadDto>>(customers);
            return Ok(results);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerReadDto>> GetCustomer(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<CustomerReadDto>(customer);
            return Ok(result);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerUpdateDto customer)
        {
            var existingCustomer = await _customerRepo.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            var customerEntity = _mapper.Map<Customer>(customer);
            bool updated = await _customerRepo.UpdateAsync(customerEntity);
            if (!updated)
            {
                return BadRequest("Unable to update the customer.");
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerCreateDto customer)
        {
            var customerEntity = _mapper.Map<Customer>(customer);
            var added = await _customerRepo.AddAsync(customerEntity);
            if (added == null)
            {
                return BadRequest("Unable to add the customer.");
            }

            return CreatedAtAction("GetCustomer", new { id = added.CustomerID }, added);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            bool deleted = await _customerRepo.DeleteAsync(id);
            if (!deleted)
            {
                return BadRequest("Unable to delete the customer.");
            }

            return NoContent();
        }
        [HttpGet("export")]
        public async Task<IActionResult> ExportCustomersToCsv()
        {
            // Lấy danh sách khách hàng từ database
            var customers = await _customerRepo.GetAllAsync();

            // Chuyển đổi khách hàng thành CustomerReadDto
            var customerDtos = _mapper.Map<IEnumerable<CustomerReadDto>>(customers);

            // Tạo bộ ghi dữ liệu CSV
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            // Dùng StringWriter và CsvWriter để tạo file CSV
            using (var stringWriter = new StringWriter())
            using (var csvWriter = new CsvHelper.CsvWriter(stringWriter, csvConfig))
            {
                // Ghi tiêu đề cho CSV
                await csvWriter.WriteRecordsAsync(customerDtos);

                // Lấy nội dung CSV dưới dạng chuỗi
                var csvContent = stringWriter.ToString();

                // Trả về file CSV cho client
                var fileName = "customers.csv";
                var contentBytes = Encoding.UTF8.GetBytes(csvContent);
                return File(contentBytes, "text/csv", fileName);
            }
        }
    }
}
