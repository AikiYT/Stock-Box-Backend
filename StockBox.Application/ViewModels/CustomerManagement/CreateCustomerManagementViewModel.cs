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
    public class CreateCustomerManagementViewModel
    {
        // CUSTOMER

        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;


        // SERVICE

        public CreateServiceViewModel? Service { get; set; }


        // DEBT

        public CreateDebtViewModel? Debt { get; set; }
    }
}