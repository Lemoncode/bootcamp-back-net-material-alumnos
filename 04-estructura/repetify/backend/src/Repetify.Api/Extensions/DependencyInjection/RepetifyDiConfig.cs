using Microsoft.EntityFrameworkCore;

using Repetify.Api.Config;
using Repetify.Application.Abstractions.Services;
using Repetify.Application.Services;
using Repetify.AuthPlatform;
using Repetify.AuthPlatform.Abstractions;
using Repetify.AuthPlatform.Abstractions.IdentityProviders;
using Repetify.AuthPlatform.Config;
using Repetify.AuthPlatform.Config.Google;
using Repetify.AuthPlatform.Config.Microsoft;
using Repetify.AuthPlatform.IdentityProviders;
using Repetify.Domain.Abstractions;
using Repetify.Domain.Abstractions.Repositories;
using Repetify.Domain.Abstractions.Services;
using Repetify.Domain.Services;
using Repetify.Infrastructure.Persistence.EfCore.Context;
using Repetify.Infrastructure.Persistence.EfCore.Repositories;
using Repetify.Infrastructure.Time;

namespace Repetify.Api.Extensions.DependencyInjection;

internal static class RepetifyDiConfig
{

	public static IServiceCollection AddRepetifyDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDomainDependencies()
			.AddInfrastructureDependencies()
			.AddPersistenceDependencies(configuration)
			.AddApplicationDependencies()
			.AddApplicationConfig(configuration);

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
		services.AddSingleton<IJwtService, JwtService>();
		services.AddScoped<IGoogleOauthService, GoogleOauthService>();
		services.AddScoped<IMicrosoftOauthService, MicrosoftOauthService>();

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

	private static IServiceCollection AddApplicationConfig(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtConfig>(configuration.GetSection(JwtConfig.ConfigSection));
		services.Configure<GoogleOauthConfig>(configuration.GetSection(GoogleOauthConfig.ConfigSection));
		services.Configure<MicrosoftOauthConfig>(configuration.GetSection(MicrosoftOauthConfig.ConfigSection));
		services.Configure<FrontendConfig>(configuration.GetSection(FrontendConfig.ConfigSection));

		return services;
	}
}