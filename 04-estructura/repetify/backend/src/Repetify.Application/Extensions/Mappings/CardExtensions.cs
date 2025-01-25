﻿using Repetify.Application.Dtos;
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
			deckId: card.DeckId,
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

	/// <summary>
	/// Converts a CardDto object to a Card domain entity.
	/// </summary>
	/// <param name="cardDto">The CardDto object to convert.</param>
	/// <returns>A Card domain entity representing the CardDto object.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the cardDto is null.</exception>
	public static Card ToEntity(this CardDto cardDto)
	{
		ArgumentNullException.ThrowIfNull(cardDto);

		return new Card(
			deckId: cardDto.DeckId,
			originalWord: cardDto.OriginalWord,
			translatedWord: cardDto.TranslatedWord,
			correctReviewStreak: cardDto.CorrectReviewStreak,
			nextReviewDate: cardDto.NextReviewDate,
			previousCorrectReview: cardDto.PreviousCorrectReview
		);
	}

	/// <summary>
	/// Converts a collection of CardDto objects to a collection of Card domain entities.
	/// </summary>
	/// <param name="cardDtos">The collection of CardDto objects to convert.</param>
	/// <returns>A collection of Card domain entities representing the CardDto objects.</returns>
	public static IEnumerable<Card> ToEntityList(this IEnumerable<CardDto> cardDtos)
	{
		return cardDtos.Select(cardDto => cardDto.ToEntity());
	}
}