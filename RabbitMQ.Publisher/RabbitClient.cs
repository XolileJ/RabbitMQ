using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.Publisher
{
    public class RabbitClient : IRabbitClient
    {
        public RabbitMessage RabbitMessage { get; set; }
        public RabbitConnection RabbitConnection { get; set; }

        public void Publish(RabbitConnection rabbitConnection, RabbitMessage rabbitMessage)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: rabbitMessage.RoutingKey,
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
    }
}
