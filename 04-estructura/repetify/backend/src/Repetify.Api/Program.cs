using Repetify.Api.Extensions.DI;
using Repetify.Api.Middlewares;

namespace Repetify.Api;

internal sealed class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Configuration.AddUserSecrets<Program>();

		// Register all services
		builder.Services.AddRepetifyDependencies(builder.Configuration);
		// HttpClient
		builder.Services.AddHttpClient("RepetifyApi", client =>
		{
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		});
		builder.Services.AddControllers();
		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
			app.UseSwaggerUi(options =>
			{
				options.DocumentPath = "openapi/v1.json";
			});
		}

		app.UseHttpsRedirection();
		app.UseAuthorization();
		app.UseMiddleware<SlidingExpirationMiddleware>();

		app.MapControllers();

		app.Run();
	}
}
