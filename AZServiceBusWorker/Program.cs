using AZServiceBusWorker;

var builder = Host.CreateApplicationBuilder(args);
string? producer = Environment.GetEnvironmentVariable("Producer");
string? consumer = Environment.GetEnvironmentVariable("Consumer");
Console.WriteLine($"Producer {producer}");
Console.WriteLine($"Consumer {consumer}");
Console.WriteLine($"Hostname: {Environment.MachineName}");

if (producer == "true")
{
    Console.WriteLine("Producer is starting");
    builder.Services.AddHostedService<ProducerWorker>();
}
if (consumer == "true")
{
    Console.WriteLine("Consumer is starting");
    builder.Services.AddHostedService<ConsumerWorker>();
}

var host = builder.Build();
host.Run();
