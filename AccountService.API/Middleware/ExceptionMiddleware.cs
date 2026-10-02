using System.Net;
using System.Text.Json;
using AccountService.Application.Exceptions;

namespace AccountService.API.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(
			RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(
			HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (NotFoundException ex)
			{
				await HandleAsync(
					context,
					HttpStatusCode.NotFound,
					ex.Message);
			}
			catch (BusinessException ex)
			{
				await HandleAsync(
					context,
					HttpStatusCode.BadRequest,
					ex.Message);
			}
			catch (Exception)
			{
				await HandleAsync(
					context,
					HttpStatusCode.InternalServerError,
					"Ocurrió un error interno.");
			}
		}

		private static async Task HandleAsync(
			HttpContext context,
			HttpStatusCode statusCode,
			string message)
		{
			context.Response.ContentType =
				"application/json";

			context.Response.StatusCode =
				(int)statusCode;

			var response = new
			{
				status = context.Response.StatusCode,
				message
			};

			await context.Response.WriteAsync(
				JsonSerializer.Serialize(response));
		}
	}
}