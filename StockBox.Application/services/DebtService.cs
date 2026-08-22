using Stock_Box.Entities;
using Stock_Box.Enums;
using StockBox.Application.Interfaces.Repositories;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.ViewModels.Deudas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.services
{
    public class DebtService : IDebtService
    {
        private readonly IDebtRepository _repository;

        public DebtService(IDebtRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(SaveDebtViewModel vm)
        {
            var remainingAmount = vm.Amount - vm.PaidAmount;

            var debt = new Debt
            {
                CustomerId = vm.CustomerId,
                Amount = vm.Amount,
                PaidAmount = vm.PaidAmount,
                RemainingAmount = remainingAmount,
                IsPaid = remainingAmount <= 0,
                CreatedAt = DateTime.UtcNow,
                PaidAt = remainingAmount <= 0 ? DateTime.UtcNow : null,
                Notes = vm.Notes
            };

            await _repository.AddAsync(debt);
        }

        public async Task<List<DebtViewModel>> GetAllAsync()
        {
            var debts = await _repository.GetAllAsync();

            return debts.Select(d => MapToViewModel(d)).ToList();
        }

        public async Task<DebtViewModel?> GetByIdAsync(int id)
        {
            var debt = await _repository.GetByIdAsync(id);

            return debt == null ? null : MapToViewModel(debt);
        }

        public async Task<List<DebtViewModel>> GetByCustomerIdAsync(int customerId)
        {
            var debts = await _repository.GetByCustomerIdAsync(customerId);

            return debts.Select(d => MapToViewModel(d)).ToList();
        }

        public async Task UpdateAsync(int id, SaveDebtViewModel vm)
        {
            var debt = await _repository.GetByIdAsync(id);

            if (debt == null)
                throw new KeyNotFoundException("Debt not found.");

            debt.CustomerId = vm.CustomerId;
            debt.Amount = vm.Amount;
            debt.PaidAmount = vm.PaidAmount;
            debt.RemainingAmount = vm.Amount - vm.PaidAmount;
            debt.IsPaid = debt.RemainingAmount <= 0;
            debt.PaidAt = debt.IsPaid ? DateTime.UtcNow : null;
            debt.Notes = vm.Notes;

            await _repository.UpdateAsync(debt);
        }

        public async Task DeleteAsync(int id)
        {
            var debt = await _repository.GetByIdAsync(id);

            if (debt == null)
                throw new KeyNotFoundException("Debt not found.");

            await _repository.DeleteAsync(debt);
        }

        private static DebtViewModel MapToViewModel(Debt d)
        {
            return new DebtViewModel
            {
                Id = d.Id,
                CustomerId = d.CustomerId,
                CustomerName = d.Customer?.Name ?? string.Empty,
                Amount = d.Amount,
                PaidAmount = d.PaidAmount,
                RemainingAmount = d.RemainingAmount,
                IsPaid = d.IsPaid,
                CreatedAt = d.CreatedAt,
                PaidAt = d.PaidAt,
                Notes = d.Notes
            };
        }
    }
}