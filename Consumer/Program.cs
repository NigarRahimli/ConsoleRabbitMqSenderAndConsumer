

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
//consumer
var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "hello",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);
    //
    // Declare the exchange (same exchange name and type as used in the sender)
    channel.ExchangeDeclare(exchange: "my_exchange",
                            type: ExchangeType.Direct);
    //
    //

    // Bind the queue to the exchange with the same routing key used in the producer
    channel.QueueBind(queue: "hello",
                      exchange: "my_exchange",
                      routingKey: "my_routing_key");
    //
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine("Received: {0}", message);
    };
    channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);

    Console.WriteLine("hello world.");
    Console.ReadLine();
}