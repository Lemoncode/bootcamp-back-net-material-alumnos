using Microsoft.EntityFrameworkCore;

using Repetify.Domain.Abstractions.Repositories;
using Repetify.Application.Abstractions.Services;
using Repetify.Application.Services;
using Repetify.Domain.Abstractions;
using Repetify.Domain.Abstractions.Services;
using Repetify.Domain.Services;
using Repetify.Infrastructure.Persistence.EfCore.Context;
using Repetify.Infrastructure.Persistence.EfCore.Repositories;
using Repetify.Infrastructure.Time;

namespace Repetify.Web.Extensions.DI;

internal static class RepetifyDiConfig
{

	public static IServiceCollection AddRepetifyDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDomainDependencies()
			.AddInfrastructureDependencies()
			.AddPersistenceDependencies(configuration)
			.AddApplicationDependencies();

		return services;
	}

	private static IServiceCollection AddDomainDependencies(this IServiceCollection services)
	{
		services.AddScoped<IDeckValidator, DeckValidator>();
		services.AddSingleton<ICardReviewService, CardReviewService>();
		return services;
	}

	private static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
	{
		services.AddSingleton<IClock, SystemClock>();
		return services;
	}

	private static IServiceCollection AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		services.AddDbContext<RepetifyDbContext>(options =>
			options.UseSqlServer(connectionString)
		);

		services.AddScoped<IDeckRepository, DeckRepository>();

		return services;
	}

	private static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
	{
		services.AddScoped<IDeckAppService, DeckAppService>();

		return services;
	}
}
