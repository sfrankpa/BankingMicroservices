# BankingMicroservices

## Descripción
Solución .NET 8 basada en dos microservicios independientes:

- **CustomerService**: administra Persona y Cliente.
- **AccountService**: administra Cuenta, Movimiento y una referencia local del cliente.

La comunicación entre ambos servicios se realiza de forma asíncrona mediante RabbitMQ.

## Tecnologías
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server 2022
- RabbitMQ
- Docker / Docker Compose
- xUnit
- Moq
- Postman

## Arquitectura

### CustomerService
- CustomerService.API
- CustomerService.Application
- CustomerService.Domain
- CustomerService.Infrastructure

### AccountService
- AccountService.API
- AccountService.Application
- AccountService.Domain
- AccountService.Infrastructure

### Compartido
- Shared.Contracts

## Bases de datos
Cada microservicio mantiene su propia base de datos:

- **CustomerDb** para CustomerService
- **AccountDb** para AccountService

Scripts disponibles en la carpeta `Database`:

- `BaseDatos.sql`
- `CustomerDatabase.sql`
- `AccountDatabase.sql`

## Ejecución con Docker

Desde la raíz del proyecto:

```bash
docker compose up --build
```

Servicios expuestos:

- CustomerService Swagger: `http://localhost:8081/swagger`
- AccountService Swagger: `http://localhost:8082/swagger`
- RabbitMQ Management: `http://localhost:15672`

Credenciales RabbitMQ:

- Usuario: `banking`
- Contraseña: `BankingRabbit2026`

## Endpoints principales

### Clientes
- `GET /api/clientes`
- `GET /api/clientes/{id}`
- `POST /api/clientes`
- `PUT /api/clientes/{id}`
- `DELETE /api/clientes/{id}`

### Cuentas
- `GET /api/cuentas`
- `GET /api/cuentas/{id}`
- `POST /api/cuentas`
- `PUT /api/cuentas/{id}`

### Movimientos
- `GET /api/movimientos`
- `GET /api/movimientos/{id}`
- `GET /api/movimientos/cuenta/{cuentaId}`
- `POST /api/movimientos`
- `PUT /api/movimientos/{id}`

### Reportes
- `GET /api/reportes?clienteId={id}&desde={fecha}&hasta={fecha}`

## Reglas de negocio
- Un movimiento positivo incrementa el saldo.
- Un movimiento negativo reduce el saldo.
- Si un movimiento deja la cuenta con saldo negativo, la API devuelve:
  - `Saldo no disponible`
- Cada movimiento conserva el saldo resultante.
- El historial de movimientos se mantiene por cuenta.

## Comunicación asíncrona
CustomerService publica eventos:

- `cliente.creado`
- `cliente.actualizado`
- `cliente.eliminado`

AccountService consume estos eventos y mantiene `ClienteReferencia`.

## Pruebas
Proyectos de pruebas:

- `CustomerService.Tests`
- `AccountService.Tests`

Incluyen:

- Prueba unitaria de Cliente.
- Prueba de integración de CustomerService.
- Prueba de saldo insuficiente.
- Prueba de depósito válido y actualización de saldo.

## Postman
La colección está disponible en la carpeta `Postman`:

- `BankingMicroservices.postman_collection.json`

Incluye pruebas para:
- Clientes
- Cuentas
- Movimientos
- Saldo no disponible
- Reporte de estado de cuenta

## Cumplimiento funcional

### F1
CRUD de Cliente y CRU de Cuenta y Movimiento.

### F2
Registro de movimientos positivos y negativos con actualización de saldo e historial.

### F3
Validación de saldo insuficiente con mensaje `Saldo no disponible`.

### F4
Reporte de estado de cuenta por cliente y rango de fechas.

### F5
Prueba unitaria de la entidad Cliente.

### F6
Prueba de integración.

### F7
Despliegue con contenedores usando Docker Compose.
