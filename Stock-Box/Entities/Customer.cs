using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Box.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public ICollection<CustomerServiceRecord> ServiceRecords { get; set; }
            = new List<CustomerServiceRecord>();

        public ICollection<Debt> Debts { get; set; }
            = new List<Debt>();
    }
}
