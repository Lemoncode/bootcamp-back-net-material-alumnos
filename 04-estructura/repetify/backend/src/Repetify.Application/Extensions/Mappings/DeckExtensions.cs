using Repetify.Application.Dtos;
using Repetify.Domain.Entities;

namespace Repetify.Application.Extensions.Mappings;

/// <summary>
/// Provides extension methods for mapping Deck domain entities to DeckDto objects and vice versa.
/// </summary>
public static class DeckExtensions
{
	/// <summary>
	/// Converts a Deck domain entity to a DeckDto.
	/// </summary>
	/// <param name="deck">The Deck domain entity to convert.</param>
	/// <returns>A DeckDto representing the Deck domain entity.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the deck is null.</exception>
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
	/// Converts a collection of Deck domain entities to a collection of DeckDto objects.
	/// </summary>
	/// <param name="decks">The collection of Deck domain entities to convert.</param>
	/// <returns>A collection of DeckDto objects representing the Deck entities.</returns>
	public static IEnumerable<DeckDto> ToDtoList(this IEnumerable<Deck> decks)
	{
		return decks.Select(deck => deck.ToDto());
	}
}
