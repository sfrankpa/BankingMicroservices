using CustomerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CustomerService.Application.Interfaces;
using CustomerService.Infrastructure.Repositories;
using CustomerService.Application.Services;
using CustomerService.API.Middleware;
using CustomerService.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//coloco
builder.Services.AddDbContext<CustomerDbContext>(
	options =>
		options.UseSqlServer(
			builder.Configuration
				.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<	IClienteRepository,	ClienteRepository>();
builder.Services.AddScoped<	IClienteService, ClienteService>();

builder.Services.Configure<RabbitMqSettings>(
	builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();

////

var app = builder.Build();

//Migracion automatica
using (var scope = app.Services.CreateScope())
{
	var db =
		scope.ServiceProvider
			.GetRequiredService<CustomerDbContext>();

	db.Database.Migrate();
}

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
