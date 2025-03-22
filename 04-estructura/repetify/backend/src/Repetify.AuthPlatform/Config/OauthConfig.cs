namespace Repetify.AuthPlatform.Config;

/// <summary>
/// Configuration settings for OAuth authentication. This class can't be instantiated directly.
/// </summary>
public abstract record class OauthConfig
{
	/// <summary>
	/// Gets or sets the URL to obtain the OAuth authorization code.
	/// </summary>
	public required Uri OauthCodeUrl { get; set; }

	/// <summary>
	/// Gets or sets the URL to obtain the OAuth token.
	/// </summary>
	public required Uri OauthTokenUrl { get; set; }

	/// <summary>
	/// Gets or sets the client ID for the OAuth application.
	/// </summary>
	public required string ClientId { get; set; }

	/// <summary>
	/// Gets or sets the client secret for the OAuth application.
	/// </summary>
	public required string ClientSecret { get; set; }

	/// <summary>
	/// Gets or sets the redirect URI for the OAuth application.
	/// </summary>
	public required Uri RedirectUri { get; set; }

	/// <summary>
	/// Gets or sets the scopes for the OAuth application.
	/// </summary>
	public required string[] Scopes { get; set; }
}
