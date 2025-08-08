using Core.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using static MongoDB.Driver.WriteConcern;

namespace Infraestrutura.Messaging
{
    public class RabbitMqMessageQueue : IMessageQueue, IDisposable
    {
        private IConnection _connection;
        private IModel _channel;

        public void Initialize()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest"
            };

            const int maxRetries = 5;
            int retry = 0;

            while (retry < maxRetries)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    break;
                }
                catch (Exception)
                {
                    retry++;
                    Thread.Sleep(2000);
                    if (retry == maxRetries) throw;
                }
            }
        }


        public Task PublishAsync(string queue, string message)
        {
            _channel.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: "",
                routingKey: queue,
                basicProperties: null,
                body: body
            );

            return Task.CompletedTask;
        }

        public Task SubscribeAsync(string queue, Func<string, Task> onMessage)
        {
            _channel.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await onMessage(message);
            };

            _channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
