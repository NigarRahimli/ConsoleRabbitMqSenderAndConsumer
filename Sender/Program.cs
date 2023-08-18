using RabbitMQ.Client;
using System.Text;
//sender
var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://localhost:5672");
using var connection=factory.CreateConnection();
var channel=connection.CreateModel();
//
// adli exchange
channel.ExchangeDeclare(exchange: "my_exchange",
                        type: ExchangeType.Direct); // direct,fanout,topic
//

channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
//adli exchange
// Bind the queue to the exchange with a specific routing key
channel.QueueBind(queue: "hello",
                  exchange: "my_exchange",
                  routingKey: "my_routing_key"); // istenilen ad ola biler her irooting key ucun

//
Console.Write("Gondermek istediyiniz mesaj :");
var message =Console.ReadLine();
var body=Encoding.UTF8.GetBytes(message);
channel.BasicPublish(/*exchange: ""*/ //adsiz olanda quebind ve exchange declare ehtiyac qalmir
                            exchange:"my_exchange",
                              routingKey: "my_routing_key",
                              basicProperties: null,
                              body: body);
Console.WriteLine("Message sent");
Console.ReadLine();
