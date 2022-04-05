using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain
{
    public class PaymentResponse
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public DateTime ProcessedDate { get; set; }

        public string IssuingBank { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string Number { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }
    }
}
