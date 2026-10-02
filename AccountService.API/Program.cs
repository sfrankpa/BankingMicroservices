using AccountService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AccountService.Application.Interfaces;
using AccountService.Infrastructure.Repositories;
using AccountService.Infrastructure.Messaging;
using AccountService.Application.Services;
using AccountService.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

///coloco
builder.Services.AddDbContext<AccountDbContext>(
options =>
	options.UseSqlServer(
		builder.Configuration
			.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<	ICuentaRepository,	CuentaRepository>();
builder.Services.AddScoped<	IMovimientoRepository,	MovimientoRepository>();
builder.Services.AddScoped<	IClienteReferenciaRepository,	ClienteReferenciaRepository>();
builder.Services.AddScoped<	ICuentaService, CuentaService>();
builder.Services.AddScoped<	IMovimientoService, MovimientoService>();
builder.Services.AddScoped<	IReporteService, ReporteService>();

builder.Services.Configure<RabbitMqSettings>(
	builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddHostedService<CustomerEventConsumer>();

////

var app = builder.Build();

//migracion automatica
using (var scope = app.Services.CreateScope())
{
	var db =
		scope.ServiceProvider
			.GetRequiredService<AccountDbContext>();

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
