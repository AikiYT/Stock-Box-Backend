using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Application.ViewModels.ServiceRecords
{
    public class CustomerServiceRecordViewModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime ServiceDate { get; set; }

        public decimal Amount { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
