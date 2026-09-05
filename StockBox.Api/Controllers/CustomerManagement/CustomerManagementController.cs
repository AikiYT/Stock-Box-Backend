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
            var customer = await _service.GetByIdAsync(id);

            if (customer == null)
                return NotFound(new
                {
                    message = "Customer not found."
                });

            return Ok(customer);
        }


        // POST: api/CustomerManagement
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCustomerManagementViewModel vm)
        {
            var customer = await _service.CreateAsync(vm);

            return CreatedAtAction(
                nameof(GetById),
                new { id = customer.Id },
                customer);
        }


        // PUT: api/CustomerManagement/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CreateCustomerManagementViewModel vm)
        {
            await _service.UpdateCustomerAsync(id, vm);

            return Ok(new
            {
                message = "Customer updated successfully."
            });
        }


        // PUT: api/CustomerManagement/5/debts/2/pay
        [HttpPut("{customerId}/debts/{debtId}/pay")]
        public async Task<IActionResult> PayDebt(
            int customerId,
            int debtId,
            [FromBody] PaymentViewModel vm)
        {
            await _service.PayDebtAsync(
                customerId,
                debtId,
                vm);

            return Ok(new
            {
                message = "Debt payment registered successfully."
            });
        }


        // DELETE: api/CustomerManagement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}