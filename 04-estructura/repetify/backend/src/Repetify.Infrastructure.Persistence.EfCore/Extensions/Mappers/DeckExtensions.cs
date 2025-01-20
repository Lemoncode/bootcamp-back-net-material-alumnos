using Repetify.Infrastructure.Persistence.EfCore.Entities;
using Repetify.Domain.Entities;

namespace Repetify.Infrastructure.Persistence.EfCore.Extensions.Mappers;

/// <summary>
/// Provides methods to map between domain and entity models for Deck and Card.
/// </summary>
public static class DeckExtensions
{
	/// <summary>
	/// Maps a Deck domain model to a DeckEntity.
	/// </summary>
	/// <param name="deckDomain">The Deck domain model.</param>
	/// <returns>The corresponding DeckEntity.</returns>
	public static DeckEntity ToEntity(this Deck deckDomain)
	{
		ArgumentNullException.ThrowIfNull(deckDomain);

		return new DeckEntity
		{
			Id = deckDomain.Id,
			Name = deckDomain.Name,
			Description = deckDomain.Description,
			UserId = deckDomain.UserId,
			OriginalLanguage = deckDomain.OriginalLanguage,
			TranslatedLanguage = deckDomain.TranslatedLanguage
		};
	}

	/// <summary>
	/// Maps a DeckEntity to a Deck domain model.
	/// </summary>
	/// <param name="deckEntity">The DeckEntity.</param>
	/// <returns>The corresponding Deck domain model, or null if the entity is null.</returns>
	public static Deck ToDomain(this DeckEntity deckEntity)
	{
		ArgumentNullException.ThrowIfNull(deckEntity);

		return new Deck(
			id: deckEntity.Id,
			name: deckEntity.Name,
			description: deckEntity.Description,
			userId: deckEntity.UserId,
			originalLanguage: deckEntity.OriginalLanguage,
			translatedLanguage: deckEntity.TranslatedLanguage
		);
	}
}
