namespace Repetify.AuthPlatform.Config;

/// <summary>
/// Configuration settings for JWT authentication.
/// </summary>
public record class JwtConfig
{
	/// <summary>
	/// The configuration section name for JWT settings.
	/// </summary>
	public static readonly string ConfigSection = "JWT";

	/// <summary>
	/// Gets or sets the audience for the JWT.
	/// </summary>
	public required string Audience { get; set; }

	/// <summary>
	/// Gets or sets the signing key for the JWT.
	/// </summary>
	public required string SigningKey { get; set; }

	/// <summary>
	/// Gets or sets the issuer of the JWT.
	/// </summary>
	public required string Issuer { get; set; }
}
