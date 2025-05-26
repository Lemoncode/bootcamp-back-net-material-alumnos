using Repetify.Application.Dtos;
using Repetify.Domain.Entities;

namespace Repetify.Application.Extensions.Mappers;

/// <summary>
///  Extensions for round-trip conversions between Decks-related Dtos and their domain entity
/// </summary>
public static class DeckExtensions
{
	/// <summary>
	///  Convert a domain entity to its DTO entity
	/// </summary>
	/// <param name="deck">The domain entity to be converted</param>
	/// <returns>The resulting DTO.</returns>
	public static DeckDto ToDto(this Deck deck)
	{
		ArgumentNullException.ThrowIfNull(deck);

		return new DeckDto(
			id: deck.Id,
			name: deck.Name,
			description: deck.Description,
			userId: deck.UserId,
			originalLanguage: deck.OriginalLanguage,
			translatedLanguage: deck.TranslatedLanguage
		);
	}

	/// <summary>
	///  Converts a list of domain entities to a list of Dtos.
	/// </summary>
	/// <param name="decks">The list of domain decks</param>
	/// <returns>The resulting list of DeckDto list.</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public static IEnumerable<DeckDto> ToDtoList(this IEnumerable<Deck> decks)
	{
		return decks is null ? throw new ArgumentNullException(nameof(decks)) : decks.Select(deck => deck.ToDto());
	}

	/// <summary>
	///  Converts a Dto used to add or update a deck, into its domain entity.
	/// </summary>
	/// <param name="deckDto">The dto to convert.</param>
	/// <param name="userId">The ID of the user who owns the deck.</param>
	/// <param name="deckId">The id of the deck (if the operation is an update.</param>
	/// <returns></returns>
	public static Deck ToEntity(this AddOrUpdateDeckDto deckDto, Guid userId, Guid? deckId)
	{
		ArgumentNullException.ThrowIfNull(deckDto);

		return new(
				id: deckId,
				name: deckDto.Name!,
				description: deckDto.Description,
				userId: userId,
				originalLanguage: deckDto.OriginalLanguage!,
				translatedLanguage: deckDto.TranslatedLanguage!
			);
	}
}
