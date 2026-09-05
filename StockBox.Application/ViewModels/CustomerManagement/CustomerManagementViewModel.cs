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

        public List<CustomerServiceRecordViewModel> Services { get; set; }
            = new();

        public List<DebtViewModel> Debts { get; set; }
            = new();
    }
}