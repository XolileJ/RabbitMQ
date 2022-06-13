using RabbitMQ.Publisher;
using System;

namespace Sender
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IRabbitClient rabbitClient = new RabbitClient();

                Console.Write("Hi, please enter your Name: ");

                rabbitClient.RabbitMessage = new RabbitMessage()
                {
                    RoutingKey = "Name",
                    Payload = Console.ReadLine()
                };

                rabbitClient.Publish(rabbitClient.RabbitConnection, rabbitClient.RabbitMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
