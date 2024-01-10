
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQListening;

/*public class InfoData
{
    public int Id { get; set; }
    public string Message { get; set; }
}*/

/*public class ApplicationContext : DbContext
{
    public DbSet<RabbitMQListening.InfoData> InfoDatas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=RabbitMQDb;Username=postgres;Password=Murad3645");
    }
}
*/
class Program
{
    static void Main()
    {
        Console.WriteLine("RabbitMQ'yi dinlemeye başla...");

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Gelen Mesaj: {message}");

                // PostgreSQL'e yazma işlemi
                using (var dbContext = new ApplicationContext())
                {
                    var entity = new InfoData { Message = message };
                    dbContext.InfoDatas.Add(entity);
                    dbContext.SaveChanges();
                }

                Console.WriteLine("Mesaj PostgreSQL'e yazıldı.");
            };

            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine("Çıxış etmek ucun bir enter'e basın.");
            Console.ReadLine();
        }
    }
}
