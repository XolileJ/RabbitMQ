namespace RabbitMQ.Publisher
{
    public interface IRabbitClient
    {
        void Publish(RabbitConnection rabbitConnection, RabbitMessage rabbitMessage);

        RabbitMessage RabbitMessage { get; set; }

        RabbitConnection RabbitConnection { get; set; }
    }
}
