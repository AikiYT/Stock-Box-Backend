using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.ViewModels.CustomerManagement
{
    public class CreateDebtViewModel
    {
        public decimal Amount { get; set; }

        public decimal PaidAmount { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
