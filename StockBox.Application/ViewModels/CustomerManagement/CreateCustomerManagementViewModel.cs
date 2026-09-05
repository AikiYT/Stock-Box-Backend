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
        public SaveCustomerViewModel Customer { get; set; } = new();

        public SaveCustomerServiceRecordViewModel? Service { get; set; }

        public SaveDebtViewModel? Debt { get; set; }
    }
}