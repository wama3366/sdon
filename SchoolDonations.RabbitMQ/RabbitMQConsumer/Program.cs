using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SchoolDonations.RabbitMQ.RabbitMQConsumer;

internal class Program
{
	static void Main()
	{
		var tasks = new Task[50];

            for (var i = 0; i < tasks.Length; i++)
            {
                var localI = i;
			tasks[i] = Task.Run(() => Run(localI));
            }

            Task.WaitAll(tasks);
	}

	private static async Task Run(int i)
	{
		var factory = new ConnectionFactory
		{
			Uri = new Uri("amqp://wassim:NewPass01@192.168.0.101:5672/TestVirtualHost")
		};
		IConnection conn = await factory.CreateConnectionAsync();
		IChannel channel = await conn.CreateChannelAsync();

		var consumer = new AsyncEventingBasicConsumer(channel);
		DateTime? firstReceivedTime = null;
		consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = long.Parse(Encoding.UTF8.GetString(ea.Body.ToArray()));
				var sentDateTime = new DateTime(body);
				firstReceivedTime ??= sentDateTime;
				Console.WriteLine($"{i} :: " + (DateTime.UtcNow - firstReceivedTime)?.ToString());
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                };

		var _ = await channel.BasicConsumeAsync("TestQueue", false, consumer);

		Console.ReadLine();
	}
}
