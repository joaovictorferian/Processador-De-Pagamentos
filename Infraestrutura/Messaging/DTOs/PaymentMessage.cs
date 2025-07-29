using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Messaging.DTOs
{
    public class PaymentMessage
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }

        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
