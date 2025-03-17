using Repetify.Domain.Entities;
using Repetify.Infrastructure.Persistence.EfCore.Entities;
using Repetify.Infrastructure.Persistence.EfCore.Extensions.Mappers;

namespace Repetify.Infrastructure.Persistence.EfCore.Tests.Extensions.Mappers;

public class CardExtensionsTests
{
	[Fact]
	public void ToEntity_ShouldMapCorrectly()
	{
		// Arrange
		var card = new Card(
			id: Guid.NewGuid(),
			deckId: Guid.NewGuid(),
			originalWord: "Hola",
			translatedWord: "Hello",
			correctReviewStreak: 5,
			nextReviewDate: DateTime.UtcNow.AddDays(1),
			previousCorrectReview: DateTime.UtcNow.AddDays(-1)
		);

		// Act
		var entity = card.ToEntity();

		// Assert
		Assert.NotNull(entity);
		Assert.Equal(card.Id, entity.Id);
		Assert.Equal(card.DeckId, entity.DeckId);
		Assert.Equal(card.OriginalWord, entity.OriginalWord);
		Assert.Equal(card.TranslatedWord, entity.TranslatedWord);
		Assert.Equal(card.CorrectReviewStreak, entity.CorrectReviewStreak);
		Assert.Equal(card.NextReviewDate, entity.NextReviewDate);
		Assert.Equal(card.PreviousCorrectReview, entity.PreviousCorrectReview);
	}

	[Fact]
	public void ToDomain_ShouldMapCorrectly()
	{
		// Arrange
		var cardEntity = new CardEntity
		{
			Id = Guid.NewGuid(),
			DeckId = Guid.NewGuid(),
			OriginalWord = "Bonjour",
			TranslatedWord = "Hello",
			CorrectReviewStreak = 3,
			NextReviewDate = DateTime.UtcNow.AddDays(2),
			PreviousCorrectReview = DateTime.UtcNow.AddDays(-3)
		};

		// Act
		var domain = cardEntity.ToDomain();

		// Assert
		Assert.NotNull(domain);
		Assert.Equal(cardEntity.Id, domain.Id);
		Assert.Equal(cardEntity.OriginalWord, domain.OriginalWord);
		Assert.Equal(cardEntity.TranslatedWord, domain.TranslatedWord);
		Assert.Equal(cardEntity.CorrectReviewStreak, domain.CorrectReviewStreak);
		Assert.Equal(cardEntity.NextReviewDate, domain.NextReviewDate);
		Assert.Equal(cardEntity.PreviousCorrectReview, domain.PreviousCorrectReview);
	}

	[Fact]
	public void ToEntity_ShouldThrowArgumentNullException_WhenCardIsNull()
	{
		// Arrange
		Card? nullCard = null;
		var deckId = Guid.NewGuid();

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => nullCard!.ToEntity());
	}

	[Fact]
	public void ToDomain_ShouldThrowArgumentNullException_WhenCardEntityIsNull()
	{
		// Arrange
		CardEntity? nullEntity = null;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => nullEntity!.ToDomain());
	}
}
