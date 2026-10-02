using System.Text;
using System.Text.Json;
using AccountService.Application.Interfaces;
using AccountService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;

namespace AccountService.Infrastructure.Messaging
{
	public class CustomerEventConsumer : BackgroundService
	{
		private readonly RabbitMqSettings _settings;
		private readonly IServiceScopeFactory _scopeFactory;

		private IConnection? _connection;
		private IChannel? _channel;

		public CustomerEventConsumer(
			IOptions<RabbitMqSettings> options,
			IServiceScopeFactory scopeFactory)
		{
			_settings = options.Value;
			_scopeFactory = scopeFactory;
		}

		protected override async Task ExecuteAsync(	CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					var factory = new ConnectionFactory
					{
						HostName = _settings.HostName,
						Port = _settings.Port,
						UserName = _settings.UserName,
						Password = _settings.Password
					};

					_connection =
						await factory.CreateConnectionAsync();

					_channel =
						await _connection.CreateChannelAsync();

					await _channel.ExchangeDeclareAsync(
						exchange: _settings.ExchangeName,
						type: ExchangeType.Topic,
						durable: true,
						autoDelete: false);

					await _channel.QueueDeclareAsync(
						queue: _settings.QueueName,
						durable: true,
						exclusive: false,
						autoDelete: false);

					await _channel.QueueBindAsync(
						queue: _settings.QueueName,
						exchange: _settings.ExchangeName,
						routingKey: "cliente.*");

					var consumer =
						new AsyncEventingBasicConsumer(_channel);

					consumer.ReceivedAsync += async (
						sender,
						args) =>
					{
						try
						{
							var json =
								Encoding.UTF8.GetString(
									args.Body.ToArray());

							var routingKey =
								args.RoutingKey;

							using var scope =
								_scopeFactory.CreateScope();

							var repository =
								scope.ServiceProvider
									.GetRequiredService<
										IClienteReferenciaRepository>();

							if (routingKey == "cliente.creado")
							{
								var evento =
									JsonSerializer.Deserialize<
										ClienteCreadoEvent>(json);

								if (evento != null)
								{
									var existente =
										await repository
											.GetByClienteIdAsync(
												evento.ClienteId);

									if (existente == null)
									{
										var cliente =
											new ClienteReferencia
											{
												ClienteId =
													evento.ClienteId,

												Nombre =
													evento.Nombre,

												Identificacion =
													evento.Identificacion,

												Estado =
													evento.Estado
											};

										await repository
											.AddAsync(cliente);

										await repository
											.SaveChangesAsync();
									}
								}
							}
							else if (
								routingKey ==
								"cliente.actualizado")
							{
								var evento =
									JsonSerializer.Deserialize<
										ClienteActualizadoEvent>(json);

								if (evento != null)
								{
									var cliente =
										await repository
											.GetByClienteIdAsync(
												evento.ClienteId);

									if (cliente != null)
									{
										cliente.Nombre =
											evento.Nombre;

										cliente.Identificacion =
											evento.Identificacion;

										cliente.Estado =
											evento.Estado;

										repository.Update(cliente);

										await repository
											.SaveChangesAsync();
									}
								}
							}
							else if (
								routingKey ==
								"cliente.eliminado")
							{
								var evento =
									JsonSerializer.Deserialize<
										ClienteEliminadoEvent>(json);

								if (evento != null)
								{
									var cliente =
										await repository
											.GetByClienteIdAsync(
												evento.ClienteId);

									if (cliente != null)
									{
										cliente.Estado = false;

										repository.Update(cliente);

										await repository
											.SaveChangesAsync();
									}
								}
							}

							await _channel.BasicAckAsync(
								args.DeliveryTag,
								multiple: false);
						}
						catch
						{
							await _channel.BasicNackAsync(
								args.DeliveryTag,
								multiple: false,
								requeue: true);
						}
					};

					await _channel.BasicConsumeAsync(
						queue: _settings.QueueName,
						autoAck: false,
						consumer: consumer);

					await Task.Delay(
						Timeout.Infinite,
						stoppingToken);
				}
				catch (OperationCanceledException)
				{
					break;
				}
				catch (Exception)
				{
					await Task.Delay(
						TimeSpan.FromSeconds(5),
						stoppingToken);
				}
			}
		}
	}
}