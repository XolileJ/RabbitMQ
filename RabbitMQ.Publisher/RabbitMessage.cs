namespace RabbitMQ.Publisher
{
    public class RabbitMessage
    {
        public string RoutingKey { get; set; }
        public string Exchange { get; set; }
        public string Payload { get; set; }
    }
}
