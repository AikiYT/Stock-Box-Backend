using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.ViewModels.ServiceRecords
{
    public class SaveCustomerServiceRecordViewModel
    {
        public int CustomerId { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime ServiceDate { get; set; }

        public decimal Amount { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
