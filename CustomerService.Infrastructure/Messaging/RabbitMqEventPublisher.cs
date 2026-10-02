using System.Text;
using System.Text.Json;
using CustomerService.Application.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CustomerService.Infrastructure.Messaging
{
	public class RabbitMqEventPublisher : IEventPublisher
	{
		private readonly RabbitMqSettings _settings;

		public RabbitMqEventPublisher(
			IOptions<RabbitMqSettings> options)
		{
			_settings = options.Value;
		}

		public async Task PublishAsync<T>(
			T message,
			string routingKey)
		{
			var factory = new ConnectionFactory
			{
				HostName = _settings.HostName,
				Port = _settings.Port,
				UserName = _settings.UserName,
				Password = _settings.Password
			};

			await using var connection =
				await factory.CreateConnectionAsync();

			await using var channel =
				await connection.CreateChannelAsync();

			await channel.ExchangeDeclareAsync(
				exchange: _settings.ExchangeName,
				type: ExchangeType.Topic,
				durable: true,
				autoDelete: false);

			var json = JsonSerializer.Serialize(message);

			var body = Encoding.UTF8.GetBytes(json);

			await channel.BasicPublishAsync(
				exchange: _settings.ExchangeName,
				routingKey: routingKey,
				body: body);
		}
	}
}