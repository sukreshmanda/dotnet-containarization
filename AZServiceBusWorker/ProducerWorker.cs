using Azure.Messaging.ServiceBus;

namespace AZServiceBusWorker;

public class ProducerWorker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Producer is almost started");
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.Write("Enter to sent a message: ");
            Console.ReadLine();
            var connectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            var queueName = Environment.GetEnvironmentVariable("QueueName");
            ServiceBusClient client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(queueName);

            await sender.SendMessagesAsync([new ServiceBusMessage($"Simple message {DateTime.UtcNow}")]);
            Console.WriteLine($"Message send from {Environment.MachineName}");

            await sender.DisposeAsync();
            await client.DisposeAsync();
            await Task.Delay((int) new Random().NextInt64(1, 10), stoppingToken);
        }
    }
}
