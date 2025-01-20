using Repetify.Infrastructure.Persistence.EfCore.Entities;
using Repetify.Domain.Entities;

namespace Repetify.Infrastructure.Persistence.EfCore.Extensions.Mappers;

/// <summary>
/// Provides methods to map between Card domain objects and CardEntity data objects.
/// </summary>
public static class CardExtensions
{
	/// <summary>
	/// Maps a Card domain object to a CardEntity data object.
	/// </summary>
	/// <param name="cardDomain">The Card domain object to map.</param>
	/// <param name="deckId">The ID of the deck to associate with the card.</param>
	/// <returns>A CardEntity data object.</returns>
	public static CardEntity ToEntity(this Card cardDomain, Guid deckId)
	{
		ArgumentNullException.ThrowIfNull(cardDomain);

		return new CardEntity
		{
			Id = cardDomain.Id,
			DeckId = deckId,
			OriginalWord = cardDomain.OriginalWord,
			TranslatedWord = cardDomain.TranslatedWord,
			CorrectReviewStreak = cardDomain.CorrectReviewStreak,
			NextReviewDate = cardDomain.NextReviewDate,
			PreviousCorrectReview = cardDomain.PreviousCorrectReview
		};
	}

	/// <summary>
	/// Maps a CardEntity data object to a Card domain object.
	/// </summary>
	/// <param name="cardEntity">The CardEntity data object to map.</param>
	/// <returns>A Card domain object.</returns>
	public static Card ToDomain(this CardEntity cardEntity)
	{
		ArgumentNullException.ThrowIfNull(cardEntity);

		return new Card(
			id: cardEntity.Id,
			originalWord: cardEntity.OriginalWord,
			translatedWord: cardEntity.TranslatedWord,
			correctReviewStreak: cardEntity.CorrectReviewStreak,
			nextReviewDate: cardEntity.NextReviewDate,
			previousCorrectReview: cardEntity.PreviousCorrectReview
		);
	}
}
