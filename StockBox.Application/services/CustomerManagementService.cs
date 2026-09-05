using Stock_Box.Entities;
using StockBox.Application.Interfaces.Repositories;
using StockBox.Application.Interfaces.Services;
using StockBox.Application.ViewModels.CustomerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockBox.Application.services
{
    public class CustomerManagementService : ICustomerManagementService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerServiceRecordRepository _serviceRecordRepository;
        private readonly IDebtRepository _debtRepository;

        public CustomerManagementService(
            ICustomerRepository customerRepository,
            ICustomerServiceRecordRepository serviceRecordRepository,
            IDebtRepository debtRepository)
        {
            _customerRepository = customerRepository;
            _serviceRecordRepository = serviceRecordRepository;
            _debtRepository = debtRepository;
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public async Task<List<CustomerManagementViewModel>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            var result = new List<CustomerManagementViewModel>();

            foreach (var customer in customers)
            {
                var services =
                    await _serviceRecordRepository
                        .GetByCustomerIdAsync(customer.Id);

                var debts =
                    await _debtRepository
                        .GetByCustomerIdAsync(customer.Id);

                result.Add(new CustomerManagementViewModel
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Email = customer.Email,
                    Address = customer.Address,

                    Services = services.Select(s =>
                        new CustomerServiceManagementViewModel
                        {
                            Id = s.Id,
                            Description = s.Description,
                            ServiceDate = s.ServiceDate,
                            Amount = s.Amount,
                            Notes = s.Notes
                        }).ToList(),

                    Debts = debts.Select(d =>
                        new DebtManagementViewModel
                        {
                            Id = d.Id,
                            Amount = d.Amount,
                            PaidAmount = d.PaidAmount,
                            RemainingAmount = d.RemainingAmount,
                            IsPaid = d.IsPaid,
                            CreatedAt = d.CreatedAt,
                            PaidAt = d.PaidAt,
                            Notes = d.Notes
                        }).ToList()
                });
            }

            return result;
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public async Task<CustomerManagementViewModel?> GetByIdAsync(int id)
        {
            var customer =
                await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return null;

            var services =
                await _serviceRecordRepository
                    .GetByCustomerIdAsync(id);

            var debts =
                await _debtRepository
                    .GetByCustomerIdAsync(id);

            return new CustomerManagementViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,

                Services = services.Select(s =>
                    new CustomerServiceManagementViewModel
                    {
                        Id = s.Id,
                        Description = s.Description,
                        ServiceDate = s.ServiceDate,
                        Amount = s.Amount,
                        Notes = s.Notes
                    }).ToList(),

                Debts = debts.Select(d =>
                    new DebtManagementViewModel
                    {
                        Id = d.Id,
                        Amount = d.Amount,
                        PaidAmount = d.PaidAmount,
                        RemainingAmount = d.RemainingAmount,
                        IsPaid = d.IsPaid,
                        CreatedAt = d.CreatedAt,
                        PaidAt = d.PaidAt,
                        Notes = d.Notes
                    }).ToList()
            };
        }


        // =========================================================
        // CREATE CUSTOMER + SERVICE + DEBT
        // =========================================================

        public async Task<CustomerManagementViewModel> CreateAsync(
            CreateCustomerManagementViewModel vm)
        {
            // -----------------------------------------------------
            // 1. Crear cliente
            // -----------------------------------------------------

            var customer = new Customer
            {
                Name = vm.Name,
                Phone = vm.Phone,
                Email = vm.Email,
                Address = vm.Address
            };

            await _customerRepository.AddAsync(customer);


            // -----------------------------------------------------
            // 2. Crear servicio
            // -----------------------------------------------------

            CustomerServiceRecord? serviceRecord = null;

            if (vm.Service != null)
            {
                serviceRecord = new CustomerServiceRecord
                {
                    CustomerId = customer.Id,
                    Description = vm.Service.Description,
                    ServiceDate = vm.Service.ServiceDate,
                    Amount = vm.Service.Amount,
                    Notes = vm.Service.Notes
                };

                await _serviceRecordRepository.AddAsync(serviceRecord);
            }


            // -----------------------------------------------------
            // 3. Crear deuda
            // -----------------------------------------------------

            Debt? debt = null;

            if (vm.Debt != null)
            {
                if (vm.Debt.PaidAmount < 0)
                    throw new ArgumentException(
                        "Paid amount cannot be negative.");

                if (vm.Debt.Amount < 0)
                    throw new ArgumentException(
                        "Debt amount cannot be negative.");

                if (vm.Debt.PaidAmount > vm.Debt.Amount)
                    throw new ArgumentException(
                        "Paid amount cannot be greater than the debt amount.");

                var remainingAmount =
                    vm.Debt.Amount - vm.Debt.PaidAmount;

                debt = new Debt
                {
                    CustomerId = customer.Id,

                    Amount = vm.Debt.Amount,

                    PaidAmount = vm.Debt.PaidAmount,

                    RemainingAmount = remainingAmount,

                    IsPaid = remainingAmount == 0,

                    CreatedAt = DateTime.UtcNow,

                    PaidAt = remainingAmount == 0
                        ? DateTime.UtcNow
                        : null,

                    Notes = vm.Debt.Notes
                };

                await _debtRepository.AddAsync(debt);
            }


            // -----------------------------------------------------
            // 4. Construir respuesta
            // -----------------------------------------------------

            var result = new CustomerManagementViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address
            };


            if (serviceRecord != null)
            {
                result.Services.Add(
                    new CustomerServiceManagementViewModel
                    {
                        Id = serviceRecord.Id,
                        Description = serviceRecord.Description,
                        ServiceDate = serviceRecord.ServiceDate,
                        Amount = serviceRecord.Amount,
                        Notes = serviceRecord.Notes
                    });
            }


            if (debt != null)
            {
                result.Debts.Add(
                    new DebtManagementViewModel
                    {
                        Id = debt.Id,
                        Amount = debt.Amount,
                        PaidAmount = debt.PaidAmount,
                        RemainingAmount = debt.RemainingAmount,
                        IsPaid = debt.IsPaid,
                        CreatedAt = debt.CreatedAt,
                        PaidAt = debt.PaidAt,
                        Notes = debt.Notes
                    });
            }

            return result;
        }


        // =========================================================
        // UPDATE CUSTOMER + SERVICE + DEBT
        // =========================================================

        public async Task UpdateCustomerAsync(
            int id,
            CreateCustomerManagementViewModel vm)
        {
            // -----------------------------------------------------
            // 1. Buscar cliente
            // -----------------------------------------------------

            var customer =
                await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                throw new KeyNotFoundException(
                    "Customer not found.");


            // -----------------------------------------------------
            // 2. Actualizar datos del cliente
            // -----------------------------------------------------

            customer.Name = vm.Name;
            customer.Phone = vm.Phone;
            customer.Email = vm.Email;
            customer.Address = vm.Address;

            await _customerRepository.UpdateAsync(customer);


            // -----------------------------------------------------
            // 3. Actualizar servicio
            // -----------------------------------------------------

            if (vm.Service != null)
            {
                var services =
                    await _serviceRecordRepository
                        .GetByCustomerIdAsync(id);

                CustomerServiceRecord? serviceRecord =
                    services.FirstOrDefault();

                if (serviceRecord == null)
                {
                    serviceRecord = new CustomerServiceRecord
                    {
                        CustomerId = id,
                        Description = vm.Service.Description,
                        ServiceDate = vm.Service.ServiceDate,
                        Amount = vm.Service.Amount,
                        Notes = vm.Service.Notes
                    };

                    await _serviceRecordRepository
                        .AddAsync(serviceRecord);
                }
                else
                {
                    serviceRecord.Description =
                        vm.Service.Description;

                    serviceRecord.ServiceDate =
                        vm.Service.ServiceDate;

                    serviceRecord.Amount =
                        vm.Service.Amount;

                    serviceRecord.Notes =
                        vm.Service.Notes;

                    await _serviceRecordRepository
                        .UpdateAsync(serviceRecord);
                }
            }


            // -----------------------------------------------------
            // 4. Actualizar deuda
            // -----------------------------------------------------

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
                        "Paid amount cannot be greater than the debt amount.");

                var debts =
                    await _debtRepository
                        .GetByCustomerIdAsync(id);

                Debt? debt =
                    debts.FirstOrDefault();

                var remainingAmount =
                    vm.Debt.Amount - vm.Debt.PaidAmount;

                if (debt == null)
                {
                    debt = new Debt
                    {
                        CustomerId = id,
                        Amount = vm.Debt.Amount,
                        PaidAmount = vm.Debt.PaidAmount,
                        RemainingAmount = remainingAmount,
                        IsPaid = remainingAmount == 0,
                        CreatedAt = DateTime.UtcNow,
                        PaidAt = remainingAmount == 0
                            ? DateTime.UtcNow
                            : null,
                        Notes = vm.Debt.Notes
                    };

                    await _debtRepository.AddAsync(debt);
                }
                else
                {
                    debt.Amount = vm.Debt.Amount;

                    debt.PaidAmount =
                        vm.Debt.PaidAmount;

                    debt.RemainingAmount =
                        remainingAmount;

                    debt.IsPaid =
                        remainingAmount == 0;

                    debt.PaidAt =
                        debt.IsPaid
                            ? DateTime.UtcNow
                            : null;

                    debt.Notes =
                        vm.Debt.Notes;

                    await _debtRepository
                        .UpdateAsync(debt);
                }
            }
        }


        // =========================================================
        // PAY DEBT
        // =========================================================

        public async Task PayDebtAsync(
            int customerId,
            int debtId,
            PaymentViewModel vm)
        {
            if (vm.PaymentAmount <= 0)
                throw new ArgumentException(
                    "Payment amount must be greater than zero.");


            // -----------------------------------------------------
            // Buscar deuda
            // -----------------------------------------------------

            var debt =
                await _debtRepository.GetByIdAsync(debtId);

            if (debt == null)
                throw new KeyNotFoundException(
                    "Debt not found.");


            // -----------------------------------------------------
            // Verificar que la deuda pertenece al cliente
            // -----------------------------------------------------

            if (debt.CustomerId != customerId)
                throw new ArgumentException(
                    "The debt does not belong to this customer.");


            // -----------------------------------------------------
            // Verificar que no esté pagada
            // -----------------------------------------------------

            if (debt.IsPaid)
                throw new InvalidOperationException(
                    "This debt is already paid.");


            // -----------------------------------------------------
            // Verificar que el pago no supere el restante
            // -----------------------------------------------------

            if (vm.PaymentAmount > debt.RemainingAmount)
                throw new ArgumentException(
                    "Payment amount cannot be greater than the remaining debt.");


            // -----------------------------------------------------
            // Registrar pago
            // -----------------------------------------------------

            debt.PaidAmount += vm.PaymentAmount;

            debt.RemainingAmount =
                debt.Amount - debt.PaidAmount;

            debt.IsPaid =
                debt.RemainingAmount == 0;

            debt.PaidAt =
                debt.IsPaid
                    ? DateTime.UtcNow
                    : null;


            await _debtRepository.UpdateAsync(debt);
        }


        // =========================================================
        // DELETE CUSTOMER
        // =========================================================

        public async Task DeleteAsync(int id)
        {
            var customer =
                await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                throw new KeyNotFoundException(
                    "Customer not found.");

            await _customerRepository.DeleteAsync(customer);
        }
    }
}