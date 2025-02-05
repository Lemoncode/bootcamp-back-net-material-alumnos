using Microsoft.EntityFrameworkCore;

using Repetify.Web.Extensions.DI;

namespace Repetify.Web;

internal class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Configuration.AddUserSecrets<Program>();

		// Register all services
		builder.Services.AddRepetifyDependencies(builder.Configuration);
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


		app.MapControllers();

		app.Run();
	} 
}
