
using Azure.Messaging.ServiceBus;

namespace AZServiceBusWorker;

public class ConsumerWorker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Consumer is almost started");
        while (!stoppingToken.IsCancellationRequested)
        {
            var connectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            var queueName = Environment.GetEnvironmentVariable("QueueName");
            var subscriptionName = Environment.GetEnvironmentVariable("SubscriptionName");
            ServiceBusClient client = new ServiceBusClient(connectionString);
            ServiceBusReceiver receiver;
            if(subscriptionName != null){
                Console.WriteLine($"Listening on the subscription {subscriptionName} from topic {queueName}");
                receiver = client.CreateReceiver(queueName, subscriptionName);
            }else{
                Console.WriteLine($"Listening on the queue {queueName}");
                receiver = client.CreateReceiver(queueName);
            }
            

            var message = await receiver.ReceiveMessageAsync(cancellationToken: stoppingToken);
            if (new Random().NextInt64() % 2 == 0)
            {
                Console.WriteLine("Message is consumed without any failure");
                await receiver.CompleteMessageAsync(message, stoppingToken);
            }
            else
            {
                Console.WriteLine("Message is consumed with failure, so moving it to dead letter queue");
                await receiver.DeadLetterMessageAsync(message, cancellationToken: stoppingToken);
            }
            Console.WriteLine($"Message received {message.Body} inside Hostname: {Environment.MachineName}");

            await receiver.DisposeAsync();
            await client.DisposeAsync();
            await Task.Delay((int) new Random().NextInt64(1, 10), stoppingToken);
        }
    }
}