using Repetify.Domain.Entities;
using Repetify.Infrastructure.Persistence.EfCore.Entities;
using Repetify.Infrastructure.Persistence.EfCore.Extensions.Mappers;
using Xunit;
using System;

namespace Repetify.Infrastructure.Persistence.EfCore.Tests.Extensions;

public class CardExtensionsTests
{
	[Fact]
	public void ToEntity_ShouldMapCorrectly()
	{
		// Arrange
		var card = new Card(
			id: Guid.NewGuid(),
			originalWord: "Hola",
			translatedWord: "Hello",
			correctReviewStreak: 5,
			nextReviewDate: DateTime.UtcNow.AddDays(1),
			previousCorrectReview: DateTime.UtcNow.AddDays(-1)
		);

		var deckId = Guid.NewGuid();

		// Act
		var entity = card.ToEntity(deckId);

		// Assert
		Assert.NotNull(entity);
		Assert.Equal(card.Id, entity.Id);
		Assert.Equal(deckId, entity.DeckId);
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
		Assert.Throws<ArgumentNullException>(() => nullCard!.ToEntity(deckId));
	}

	[Fact]
	public void ToDomain_ShouldThrowArgumentNullException_WhenCardEntityIsNull()
	{
		// Arrange
		CardEntity? nullEntity = null;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => nullEntity!.ToDomain());
	}
	[Fact]
	public void ToEntity_ShouldHandleValidInputs()
	{
		// Arrange
		var card = new Card("Hello", "Hola");
		var deckId = Guid.NewGuid();

		// Act
		var entity = card.ToEntity(deckId);

		// Assert
		Assert.Equal(card.OriginalWord, entity.OriginalWord);
		Assert.Equal(card.TranslatedWord, entity.TranslatedWord);
		Assert.Equal(deckId, entity.DeckId);
	}

	[Fact]
	public void ToDomain_ShouldHandleValidInputs()
	{
		// Arrange
		var cardEntity = new CardEntity
		{
			Id = Guid.NewGuid(),
			DeckId = Guid.NewGuid(),
			OriginalWord = "Goodbye",
			TranslatedWord = "Adiós",
			CorrectReviewStreak = 1,
			NextReviewDate = DateTime.UtcNow,
			PreviousCorrectReview = DateTime.UtcNow.AddDays(-1)
		};

		// Act
		var domain = cardEntity.ToDomain();

		// Assert
		Assert.Equal(cardEntity.OriginalWord, domain.OriginalWord);
		Assert.Equal(cardEntity.TranslatedWord, domain.TranslatedWord);
	}
}
