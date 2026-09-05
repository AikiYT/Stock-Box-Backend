using Stock_Box.Entities;
using StockBox.Application.Interfaces.Repositories;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.ViewModels.ServiceRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.services
{
    public class CustomerServiceRecordService : ICustomerServiceRecordService
    {
        private readonly ICustomerServiceRecordRepository _repository;

        public CustomerServiceRecordService(
            ICustomerServiceRecordRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(SaveCustomerServiceRecordViewModel vm)
        {
            var record = new CustomerServiceRecord
            {
                CustomerId = vm.CustomerId,
                Description = vm.Description,

                Amount = vm.Amount,
                Notes = vm.Notes,
                ServiceDate = DateTime.SpecifyKind(
    vm.ServiceDate,
    DateTimeKind.Utc)
            };

            await _repository.AddAsync(record);
        }

        public async Task<List<CustomerServiceRecordViewModel>> GetAllAsync()
        {
            var records = await _repository.GetAllAsync();

            return records.Select(r => new CustomerServiceRecordViewModel
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer?.Name ?? string.Empty,
                Description = r.Description,
                ServiceDate = r.ServiceDate,
                Amount = r.Amount,
                Notes = r.Notes
            }).ToList();
        }

        public async Task<CustomerServiceRecordViewModel?> GetByIdAsync(int id)
        {
            var r = await _repository.GetByIdAsync(id);

            if (r == null)
                return null;

            return new CustomerServiceRecordViewModel
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer?.Name ?? string.Empty,
                Description = r.Description,
                ServiceDate = r.ServiceDate,
                Amount = r.Amount,
                Notes = r.Notes
            };
        }

        public async Task<List<CustomerServiceRecordViewModel>> GetByCustomerIdAsync(
            int customerId)
        {
            var records = await _repository.GetByCustomerIdAsync(customerId);

            return records.Select(r => new CustomerServiceRecordViewModel
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer?.Name ?? string.Empty,
                Description = r.Description,
                ServiceDate = r.ServiceDate,
                Amount = r.Amount,
                Notes = r.Notes
            }).ToList();
        }

        public async Task UpdateAsync(
            int id,
            SaveCustomerServiceRecordViewModel vm)
        {
            var record = await _repository.GetByIdAsync(id);

            if (record == null)
                throw new KeyNotFoundException("Service record not found.");

            record.CustomerId = vm.CustomerId;
            record.Description = vm.Description;
            record.ServiceDate = DateTime.SpecifyKind(
    vm.ServiceDate,
    DateTimeKind.Utc);
            record.Amount = vm.Amount;
            record.Notes = vm.Notes;

            await _repository.UpdateAsync(record);
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _repository.GetByIdAsync(id);

            if (record == null)
                throw new KeyNotFoundException("Service record not found.");

            await _repository.DeleteAsync(record);
        }
    }
}
