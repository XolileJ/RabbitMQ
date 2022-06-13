using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Publisher
{
    public class RabbitClient : IRabbitClient
    {
        public RabbitMessage RabbitMessage { get; set; }
        public RabbitConnection RabbitConnection { get; set; }

        public void Publish(RabbitConnection rabbitConnection, RabbitMessage rabbitMessage)
        {
            ConnectionFactory factory = CreateConnectionFactory();

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: rabbitMessage.RoutingKey ?? "Name",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var message = rabbitMessage.Payload;

            var body = Encoding.UTF8.GetBytes(message);

            IBasicProperties properties = channel.CreateBasicProperties();

            properties.Persistent = true;

            properties.DeliveryMode = (int)DeliveryMode.Persistent;

            channel.BasicPublish(exchange: rabbitMessage.Exchange ?? "",
                                 routingKey: rabbitMessage.RoutingKey,
                                 body: body);
        }

        public string Consume(RabbitConnection rabbitConnection, RabbitMessage rabbitMessage)
        {
            var factory = new ConnectionFactory() {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                DispatchConsumersAsync = true };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: rabbitMessage.RoutingKey ?? "Name",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            string name = "";

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();

                await Task.Delay(1000);

                name = Encoding.UTF8.GetString(body);
            };

            channel.BasicConsume(queue: rabbitMessage.RoutingKey,
                                 autoAck: false,
                                 consumer: consumer);

            return name;
        }

        private static ConnectionFactory CreateConnectionFactory()
        {
            return new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }
    }
}
