using Core.Entities;
using Core.Interfaces;
using Core.Interfaces;
using Infraestrutura.Messaging.DTOs;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static MongoDB.Driver.WriteConcern;

namespace Application.UseCases.Pagamentos
{
    public class PaymentUseCase
    {
        private readonly IPaymentRepository _repository;
        private readonly IMessageQueue _queue;

        private const string PaymentConfirmationQueue = "payment-confirmation";

        public PaymentUseCase(IPaymentRepository repository, IMessageQueue queue)
        {
            _repository = repository;
            _queue = queue;
        }
        public async Task ProcessPaymentAsync(decimal amount)
        {
            var payment = new Payment
            {
                Id = ObjectId.GenerateNewId(),
                Amount = amount,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
            };

            await _repository.AddAsync(payment);

            var paymentMessage = new PaymentMessage
            {
                Id = payment.Id.ToString(),
                Amount = amount,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt,
            };
            var msg = JsonSerializer.Serialize(paymentMessage);
            await _queue.PublishAsync(PaymentConfirmationQueue, msg);
        }

        public async Task ConfirmPaymentAsync(string message)
        {
            var paymentMessage = JsonSerializer.Deserialize<PaymentMessage>(message);
            if (paymentMessage != null)
            {
                var payment = new Payment
                {
                    Id = ObjectId.Parse(paymentMessage.Id),
                    Amount = paymentMessage.Amount,
                    Status = paymentMessage.Status,
                    CreatedAt = paymentMessage.CreatedAt,
                };
                payment.Status = "Confirmed";
                await _repository.UpdateAsync(payment);
                Console.WriteLine($"Pagamento {payment.Id} confirmado");
            }
        }
        public async Task StartPaymentConfirmationListener() =>
                await _queue.SubscribeAsync(PaymentConfirmationQueue, ConfirmPaymentAsync);
    }

}
