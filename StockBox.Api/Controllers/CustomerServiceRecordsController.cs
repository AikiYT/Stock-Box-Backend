using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.ViewModels.ServiceRecords;

namespace StockBox.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerServiceRecordsController : ControllerBase
    {
        private readonly ICustomerServiceRecordService _service;

        public CustomerServiceRecordsController(
            ICustomerServiceRecordService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);

            if (record == null)
                return NotFound();

            return Ok(record);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            return Ok(
                await _service.GetByCustomerIdAsync(customerId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            SaveCustomerServiceRecordViewModel vm)
        {
            await _service.CreateAsync(vm);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            SaveCustomerServiceRecordViewModel vm)
        {
            await _service.UpdateAsync(id, vm);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}