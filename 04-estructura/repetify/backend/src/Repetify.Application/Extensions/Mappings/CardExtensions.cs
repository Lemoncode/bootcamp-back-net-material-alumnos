using Repetify.Application.Dtos;
using Repetify.Domain.Entities;

namespace Repetify.Application.Extensions.Mappings;

/// <summary>
/// Extension methods for converting Card domain entities to CardDto objects and vice versa.
/// </summary>
public static class CardExtensions
{
	/// <summary>
	/// Converts a Card domain entity to a CardDto object.
	/// </summary>
	/// <param name="card">The Card domain entity to convert.</param>
	/// <returns>A CardDto object representing the Card domain entity.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the card is null.</exception>
	public static CardDto ToDto(this Card card)
	{
		ArgumentNullException.ThrowIfNull(card);

		return new CardDto(
			id: card.Id,
			originalWord: card.OriginalWord,
			translatedWord: card.TranslatedWord,
			correctReviewStreak: card.CorrectReviewStreak,
			nextReviewDate: card.NextReviewDate,
			previousCorrectReview: card.PreviousCorrectReview
		);
	}

	/// <summary>
	/// Converts a collection of Card domain entities to a collection of CardDto objects.
	/// </summary>
	/// <param name="cards">The collection of Card domain entities to convert.</param>
	/// <returns>A collection of CardDto objects representing the Card domain entities.</returns>
	public static IEnumerable<CardDto> ToDtoList(this IEnumerable<Card> cards)
	{
		return cards.Select(card => card.ToDto());
	}
}

