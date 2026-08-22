using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.ViewModels.Customers;

namespace StockBox.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service)
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
            var customer = await _service.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            SaveCustomerViewModel vm)
        {
            await _service.CreateAsync(vm);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            SaveCustomerViewModel vm)
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