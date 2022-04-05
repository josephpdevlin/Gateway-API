using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.DB
{
    public class IdempotencyRecord
    {
        public int Id { get; set; }

        public string IdempotencyKey { get; set; }

        public int RequestCount { get; set; }

        public int PaymentId { get; set; }
        public virtual Payment Payment { get; set; }

        public DateTime CreatedDateTime {get;set;}
    }
}
