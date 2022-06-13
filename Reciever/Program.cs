using RabbitMQ.Publisher;
using System;

namespace Reciever
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IRabbitClient rabbitClient = new RabbitClient();

                rabbitClient.RabbitMessage = new RabbitMessage()
                {
                    RoutingKey = "Name"
                };

                var name = rabbitClient.Consume(rabbitClient.RabbitConnection, rabbitClient.RabbitMessage);

                Console.WriteLine($"Hello {name}, I am your father!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
