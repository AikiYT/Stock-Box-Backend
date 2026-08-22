using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.ViewModels.Deudas;

namespace StockBox.Api.Controllers.Debts
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebtsController : ControllerBase
    {
        private readonly IDebtService _service;

        public DebtsController(IDebtService service)
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
            var debt = await _service.GetByIdAsync(id);

            if (debt == null)
                return NotFound();

            return Ok(debt);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            return Ok(
                await _service.GetByCustomerIdAsync(customerId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            SaveDebtViewModel vm)
        {
            await _service.CreateAsync(vm);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            SaveDebtViewModel vm)
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
