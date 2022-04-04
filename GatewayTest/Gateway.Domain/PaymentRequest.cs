using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain
{
    public class PaymentRequest
    {
        public string ReferenceKey { get; set; }

        public int IdempotencyKey { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public int CVV { get; set; }
    }
}
