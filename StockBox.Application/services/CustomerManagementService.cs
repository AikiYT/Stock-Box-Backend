using Stock_Box.Entities;
using StockBox.Application.Interfaces.Repositories;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.ViewModels.CustomerManagement;
using StockBox.Application.ViewModels.Customers;
using StockBox.Application.ViewModels.Deudas;
using StockBox.Application.ViewModels.ServiceRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.services
{
    public class CustomerManagementService : ICustomerManagementService
    {
        private readonly ICustomerManagementRepository _repository;

        public CustomerManagementService(
            ICustomerManagementRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomerManagementViewModel> CreateAsync(
            CreateCustomerManagementViewModel vm)
        {
            // =========================
            // CUSTOMER
            // =========================

            var customer = new Customer
            {
                Name = vm.Customer.Name,
                Phone = vm.Customer.Phone,
                Email = vm.Customer.Email,
                Address = vm.Customer.Address
            };

            // =========================
            // SERVICE
            // =========================

            CustomerServiceRecord? serviceRecord = null;

            if (vm.Service != null)
            {
                serviceRecord = new CustomerServiceRecord
                {
                    Description = vm.Service.Description,
                    ServiceDate = vm.Service.ServiceDate,
                    Amount = vm.Service.Amount,
                    Notes = vm.Service.Notes
                };
            }

            // =========================
            // DEBT
            // =========================

            Debt? debt = null;

            if (vm.Debt != null)
            {
                if (vm.Debt.Amount < 0)
                    throw new ArgumentException(
                        "Debt amount cannot be negative.");

                if (vm.Debt.PaidAmount < 0)
                    throw new ArgumentException(
                        "Paid amount cannot be negative.");

                if (vm.Debt.PaidAmount > vm.Debt.Amount)
                    throw new ArgumentException(
                        "Paid amount cannot be greater than debt amount.");

                var remaining =
                    vm.Debt.Amount - vm.Debt.PaidAmount;

                debt = new Debt
                {
                    Amount = vm.Debt.Amount,
                    PaidAmount = vm.Debt.PaidAmount,
                    RemainingAmount = remaining,
                    IsPaid = remaining <= 0,
                    CreatedAt = DateTime.UtcNow,
                    PaidAt = remaining <= 0
                        ? DateTime.UtcNow
                        : null,
                    Notes = vm.Debt.Notes
                };
            }

            // =========================
            // DATABASE
            // =========================

            var createdCustomer =
                await _repository.CreateCompleteCustomerAsync(
                    customer,
                    serviceRecord,
                    debt);

            // =========================
            // RESPONSE
            // =========================

            return MapToViewModel(createdCustomer);
        }

        public async Task<CustomerManagementViewModel?> GetByIdAsync(
            int id)
        {
            var customer =
                await _repository.GetCompleteCustomerByIdAsync(id);

            if (customer == null)
                return null;

            return MapToViewModel(customer);
        }

        public async Task<List<CustomerManagementViewModel>> GetAllAsync()
        {
            var customers =
                await _repository.GetAllCompleteCustomersAsync();

            return customers
                .Select(MapToViewModel)
                .ToList();
        }

        private static CustomerManagementViewModel MapToViewModel(
            Customer customer)
        {
            return new CustomerManagementViewModel
            {
                Id = customer.Id,

                Name = customer.Name,

                Phone = customer.Phone,

                Email = customer.Email,

                Address = customer.Address,

                Services = customer.ServiceRecords
                    .Select(r => new CustomerServiceRecordViewModel
                    {
                        Id = r.Id,

                        CustomerId = r.CustomerId,

                        CustomerName = customer.Name,

                        Description = r.Description,

                        ServiceDate = r.ServiceDate,

                        Amount = r.Amount,

                        Notes = r.Notes
                    })
                    .OrderByDescending(r => r.ServiceDate)
                    .ToList(),

                Debts = customer.Debts
                    .Select(d => new DebtViewModel
                    {
                        Id = d.Id,

                        CustomerId = d.CustomerId,

                        CustomerName = customer.Name,

                        Amount = d.Amount,

                        PaidAmount = d.PaidAmount,

                        RemainingAmount = d.RemainingAmount,

                        IsPaid = d.IsPaid,

                        CreatedAt = d.CreatedAt,

                        PaidAt = d.PaidAt,

                        Notes = d.Notes
                    })
                    .OrderByDescending(d => d.CreatedAt)
                    .ToList()
            };
        }
    }
}