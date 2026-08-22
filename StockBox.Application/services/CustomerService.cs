using Stock_Box.Entities;
using StockBox.Application.Interfaces.Repositories;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task CreateAsync(SaveCustomerViewModel vm)
        {
            var customer = new Customer
            {
                Name = vm.Name,
                Phone = vm.Phone,
                Email = vm.Email,
                Address = vm.Address
            };

            await _customerRepository.AddAsync(customer);
        }

        public async Task<List<CustomerViewModel>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            return customers.Select(c => new CustomerViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address
            }).ToList();
        }

        public async Task<CustomerViewModel?> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return null;

            return new CustomerViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address
            };
        }

        public async Task UpdateAsync(int id, SaveCustomerViewModel vm)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            customer.Name = vm.Name;
            customer.Phone = vm.Phone;
            customer.Email = vm.Email;
            customer.Address = vm.Address;

            await _customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            await _customerRepository.DeleteAsync(customer);
        }
    }
}
