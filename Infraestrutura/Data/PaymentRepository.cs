using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Core.Entities;
using Core.Interfaces;
using MongoDB.Bson;

namespace Infraestrutura.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMongoCollection<Payment> _payments;

        public PaymentRepository(IMongoContext context)
        {
            _payments = context.GetCollection<Payment>("Payment");
        }

        public async Task AddAsync(Payment payment) => await _payments.InsertOneAsync(payment);

        public async Task<Payment> GetByIdAsync(ObjectId id) => await _payments.Find(p => p.Id == id).FirstOrDefaultAsync();
        public async Task UpdateAsync(Payment payment) => await _payments.ReplaceOneAsync(p => p.Id == payment.Id, payment);

    }

}
