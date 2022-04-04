using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.DB
{
    public class Card
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public Guid? Token { get; set; }
    }
}
