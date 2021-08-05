using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitConsumidor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "qWilliamTest",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($" [x] Recebida {message}");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception)
                    {
                        //logar exc
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                    
                };
                channel.BasicConsume(queue: "qWilliamTest",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}
