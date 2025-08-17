using System.Text;
using RabbitMQ.Client;

namespace SchoolDonations.RabbitMQ.RabbitMQ;

internal class Program
{
    static void Main()
    {
		Task[] tasks = new Task[50];

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(Run);
        }

        Task.WaitAll(tasks);
    }

    private static async Task Run()
    {
		ConnectionFactory factory = new()
		{
		    Uri = new Uri("amqp://wassim:NewPass01@192.168.0.101:5672/TestVirtualHost")
	    };
		IConnection conn = await factory.CreateConnectionAsync();
		IChannel channel = await conn.CreateChannelAsync();

        await channel.ExchangeDeclareAsync("TestExchange", ExchangeType.Direct);
        await channel.QueueDeclareAsync("TestQueue", true, false, false);
        await channel.QueueBindAsync("TestQueue", "TestExchange", "misc");

		List<Task> tasks = [];
        for(int i = 0; i < 1000000; i++)
        {
			byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"{DateTime.UtcNow.Ticks}");
			BasicProperties props = new();
		    tasks.Add(channel.BasicPublishAsync("TestExchange", "misc", false, props, messageBodyBytes).AsTask());
	    }

        await Task.WhenAll(tasks);

        await channel.CloseAsync();
        await conn.CloseAsync();
        await channel.DisposeAsync();
        await conn.DisposeAsync();

        Console.WriteLine("Done");

		_ = Console.ReadLine();
    }
}
