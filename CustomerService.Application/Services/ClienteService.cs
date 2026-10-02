using CustomerService.Application.DTOs;
using CustomerService.Application.Exceptions;
using CustomerService.Application.Interfaces;
using CustomerService.Domain.Entities;
using Shared.Contracts.Events;

namespace CustomerService.Application.Services
{
	public class ClienteService : IClienteService
	{
		private readonly IClienteRepository _clienteRepository;
		private readonly IEventPublisher _eventPublisher;

		public ClienteService(IClienteRepository clienteRepository, IEventPublisher eventPublisher)
		{
			_clienteRepository = clienteRepository;
			_eventPublisher = eventPublisher;
		}

		public async Task<List<ClienteDto>> GetAllAsync()
		{
			var clientes =
				await _clienteRepository.GetAllAsync();

			return clientes
				.Select(MapToDto)
				.ToList();
		}

		public async Task<ClienteDto> GetByIdAsync(int id)
		{
			var cliente =
				await _clienteRepository.GetByIdAsync(id);

			if (cliente == null)
			{
				throw new NotFoundException(
					"Cliente no encontrado.");
			}

			return MapToDto(cliente);
		}

		public async Task<ClienteDto> CreateAsync(	CreateClienteDto dto)
		{
			if (string.IsNullOrWhiteSpace(dto.Nombre))
			{
				throw new BusinessException(
					"El nombre es obligatorio.");
			}

			if (string.IsNullOrWhiteSpace(dto.Identificacion))
			{
				throw new BusinessException(
					"La identificación es obligatoria.");
			}

			if (dto.Edad <= 0)
			{
				throw new BusinessException(
					"La edad debe ser mayor que cero.");
			}

			var existente =
				await _clienteRepository
					.GetByIdentificacionAsync(
						dto.Identificacion);

			if (existente != null)
			{
				throw new BusinessException(
					"Ya existe un cliente con esa identificación.");
			}

			var cliente = new Cliente
			{
				Nombre = dto.Nombre,
				Genero = dto.Genero,
				Edad = dto.Edad,
				Identificacion = dto.Identificacion,
				Direccion = dto.Direccion,
				Telefono = dto.Telefono,
				Contrasena = dto.Contrasena,
				Estado = dto.Estado
			};

			await _clienteRepository.AddAsync(cliente);

			await _clienteRepository.SaveChangesAsync();

			var evento = new ClienteCreadoEvent
			{
				ClienteId = cliente.Id,
				Nombre = cliente.Nombre,
				Identificacion = cliente.Identificacion,
				Estado = cliente.Estado
			};

			await _eventPublisher.PublishAsync(evento, "cliente.creado");

			return MapToDto(cliente);
		}

		public async Task<ClienteDto> UpdateAsync(int id, UpdateClienteDto dto)
		{
			var cliente =
				await _clienteRepository.GetByIdAsync(id);

			if (cliente == null)
			{
				throw new NotFoundException(
					"Cliente no encontrado.");
			}

			cliente.Nombre = dto.Nombre;
			cliente.Genero = dto.Genero;
			cliente.Edad = dto.Edad;
			cliente.Direccion = dto.Direccion;
			cliente.Telefono = dto.Telefono;
			cliente.Contrasena = dto.Contrasena;
			cliente.Estado = dto.Estado;

			_clienteRepository.Update(cliente);

			await _clienteRepository.SaveChangesAsync();

			var evento = new ClienteActualizadoEvent
			{
				ClienteId = cliente.Id,
				Nombre = cliente.Nombre,
				Identificacion = cliente.Identificacion,
				Estado = cliente.Estado
			};

			await _eventPublisher.PublishAsync(evento, "cliente.actualizado");

			return MapToDto(cliente);
		}

		public async Task DeleteAsync(int id)
		{
			var cliente = await _clienteRepository.GetByIdAsync(id);

			if (cliente == null)
			{
				throw new NotFoundException(
					"Cliente no encontrado.");
			}

			var clienteId = cliente.Id;

			_clienteRepository.Delete(cliente);

			await _clienteRepository.SaveChangesAsync();

			var evento = new ClienteEliminadoEvent
			{
				ClienteId = clienteId
			};

			await _eventPublisher.PublishAsync(evento, "cliente.eliminado");
		}

		private static ClienteDto MapToDto(Cliente cliente)
		{
			return new ClienteDto
			{
				ClienteId = cliente.Id,
				Nombre = cliente.Nombre,
				Genero = cliente.Genero,
				Edad = cliente.Edad,
				Identificacion = cliente.Identificacion,
				Direccion = cliente.Direccion,
				Telefono = cliente.Telefono,
				Estado = cliente.Estado
			};
		}
	}
}