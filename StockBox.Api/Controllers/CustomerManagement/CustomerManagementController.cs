using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.ViewModels.CustomerManagement;

namespace StockBox.Api.Controllers.CustomerManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerManagementController : ControllerBase
    {
        private readonly ICustomerManagementService _service;

        public CustomerManagementController(
            ICustomerManagementService service)
        {
            _service = service;
        }

        // GET: api/CustomerManagement
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _service.GetAllAsync();

            return Ok(customers);
        }

        // GET: api/CustomerManagement/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer =
                await _service.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // POST: api/CustomerManagement
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateCustomerManagementViewModel vm)
        {
            try
            {
                var customer =
                    await _service.CreateAsync(vm);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = customer.Id },
                    customer);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
