using StockBox.Application.ViewModels.Customers;
using StockBox.Application.ViewModels.Deudas;
using StockBox.Application.ViewModels.ServiceRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.ViewModels.CustomerManagement
{
    public class CustomerManagementViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public List<CustomerServiceManagementViewModel> Services { get; set; }
            = new();

        public List<DebtManagementViewModel> Debts { get; set; }
            = new();
    }


    public class CustomerServiceManagementViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime ServiceDate { get; set; }

        public decimal Amount { get; set; }

        public string Notes { get; set; } = string.Empty;
    }


    public class DebtManagementViewModel
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal RemainingAmount { get; set; }

        public bool IsPaid { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? PaidAt { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}