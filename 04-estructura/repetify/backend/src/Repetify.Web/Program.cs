using Microsoft.EntityFrameworkCore;

using Repetify.Infrastructure.Persistence.EfCore.Context;

namespace Repetify.Web;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Configuration.AddUserSecrets<Program>();
		var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

		// Registrar el contexto en la inyección de dependencias
		builder.Services.AddDbContext<RepetifyDbContext>(options =>
			options.UseSqlServer(connectionString)
		);

		builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
