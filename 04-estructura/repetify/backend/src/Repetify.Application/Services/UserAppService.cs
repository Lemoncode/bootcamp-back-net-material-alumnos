using Microsoft.Extensions.Options;

using Repetify.Application.Config;
using Repetify.Application.Abstractions.Services;
using Repetify.Application.Dtos;
using Repetify.Application.Extensions.Mappers;
using Repetify.AuthPlatform.Abstractions;
using Repetify.AuthPlatform.Abstractions.IdentityProviders;
using Repetify.Crosscutting;
using Repetify.Crosscutting.OAuth;
using Repetify.Crosscutting.Exceptions;
using Repetify.Crosscutting.Extensions;
using Repetify.Domain.Abstractions.Repositories;
using Repetify.Domain.Abstractions.Services;
using Repetify.Domain.Entities;

namespace Repetify.Application.Services;

public class UserAppService : IUserAppService
{
	private readonly IGoogleOAuthService _googleOAuthService;
	private readonly IMicrosoftOAuthService _microsoftOAuthService;
	private readonly IJwtService _jwtService;
	private readonly IUserRepository _userRepository;
	private readonly FrontendConfig _frontendConfig;

	public UserAppService(IGoogleOAuthService googleOAuthService, IMicrosoftOAuthService microsoftOAuthService, IJwtService jwtService, IUserRepository repository, IOptionsSnapshot<FrontendConfig> frontendConfig)
	{
		_googleOAuthService = googleOAuthService ?? throw new ArgumentNullException(nameof(googleOAuthService));
		_microsoftOAuthService = microsoftOAuthService ?? throw new ArgumentNullException(nameof(microsoftOAuthService));
		_jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
		_userRepository = repository ?? throw new ArgumentNullException(nameof(repository));
		_frontendConfig = frontendConfig?.Value ?? throw new ArgumentNullException(nameof(frontendConfig));
	}

	public async Task<Result<UserDto>> GetUserByEmailAsync(string email)
	{
		try
		{
			var user = await _userRepository.GetUserByEmailAsync(email).EnsureSuccessAsync().ConfigureAwait(false);
			return ResultFactory.Success<UserDto>(user.ToDto());
		}
		catch (ResultFailureException ex)
		{
			return ResultFactory.PropagateFailure<UserDto>(ex.Result);
		}
	}

	public Result<Uri> GetUriToInitiateOAuthSignin(IdentityProvider provider, Uri? returnUrl = null)
	{
		if (returnUrl is null)
		{
			returnUrl = _frontendConfig.FrontendBaseUrl;
		}

		var redirectUri = provider switch
		{
			IdentityProvider.Google => _googleOAuthService.GetOAuthCodeUrl(returnUrl),
			IdentityProvider.Microsoft => _microsoftOAuthService.GetOAuthCodeUrl(returnUrl),
			_ => null
		};

		if (redirectUri is null)
		{
			return ResultFactory.InvalidArgument<Uri>("Identity provider not supported.");
		}

		return ResultFactory.Success(redirectUri);
	}

	public async Task<Result<FinishedOAuthResponseDto>> FinishOAuthFlow(IdentityProvider provider, string code, Uri? returnUrl = null)
	{
		try
		{
			string? token = null;

			switch (provider)
			{
				case IdentityProvider.Google:
					var tokenResponse = await _googleOAuthService.ExchangeCodeForToken(code).ConfigureAwait(false);
					var payload = await _googleOAuthService.GetUserInfo(tokenResponse.IdToken).ConfigureAwait(false);
					await CheckAndAddNewUserAsync(payload.Email, payload.Email).ConfigureAwait(false);
					token = _jwtService.GenerateJwtToken(payload.FamilyName, payload.GivenName, payload.Email);
					break;
				case IdentityProvider.Microsoft:
					var msTokenResponse = await _microsoftOAuthService.ExchangeCodeForToken(code).ConfigureAwait(false);
					var userInfo = await _microsoftOAuthService.GetUserInfo(msTokenResponse.AccessToken).ConfigureAwait(false);
					await CheckAndAddNewUserAsync(userInfo.Mail, userInfo.Mail).ConfigureAwait(false);
					token = _jwtService.GenerateJwtToken(userInfo.Surname, userInfo.GivenName, userInfo.Mail);
					break;
				default:
					return ResultFactory.InvalidArgument<FinishedOAuthResponseDto>("This identity provider is not supported.");
			}

			return ResultFactory.Success(new FinishedOAuthResponseDto { JwtToken = token, ReturnUrl = returnUrl ?? _frontendConfig.FrontendBaseUrl });
		}
		catch (ResultFailureException ex)
		{
			return ResultFactory.PropagateFailure<FinishedOAuthResponseDto>(ex.Result);
		}
	}

	private async Task CheckAndAddNewUserAsync(string username, string email)
	{
		var userResult = await GetUserByEmailAsync(email).ConfigureAwait(false);
		if (!userResult.IsSuccess)
		{
			await _userRepository.AddUserAsync(new User(null, email, username)).EnsureSuccessAsync().ConfigureAwait(false);
			await _userRepository.SaveChangesAsync().ConfigureAwait(false);
		}
	}

}