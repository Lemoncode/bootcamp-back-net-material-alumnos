using Moq;

using Repetify.Domain.Abstractions;
using Repetify.Domain.Entities;

namespace Repetify.Tests.Domain.Entities;

public class CardTests
{

	[Fact]
	public void Card_Should_Initialize_With_Valid_Values()
	{
		// Arrange
		var originalWord = "Hello";
		var translatedWord = "Hola";

		// Act
		var card = new Card(Guid.NewGuid(), originalWord, translatedWord);

		// Assert
		Assert.Equal(originalWord, card.OriginalWord);
		Assert.Equal(translatedWord, card.TranslatedWord);
		Assert.Equal(0, card.CorrectReviewStreak);
		Assert.True(card.NextReviewDate > DateTime.UtcNow);
		Assert.Equal(DateTime.MinValue, card.PreviousCorrectReview);
	}



	[Fact]
	public void Card_ShouldThrowArgumentNullException_WhenOriginalWordIsNull()
	{
		// Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			new Card(Guid.NewGuid(), Guid.NewGuid(), null!, "Hello", 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
	}

	[Fact]
	public void Card_ShouldThrowArgumentException_WhenOriginalWordIsEmpty()
	{
		// Act & Assert
		Assert.Throws<ArgumentException>(() =>
			new Card(Guid.NewGuid(), Guid.NewGuid(), "", "Hello", 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
	}

	[Fact]
	public void Card_ShouldThrowArgumentException_WhenOriginalWordIsWhitespace()
	{
		// Act & Assert
		Assert.Throws<ArgumentException>(() =>
			new Card(Guid.NewGuid(), Guid.NewGuid(), "   ", "Hello", 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
	}

	[Fact]
	public void Card_ShouldThrowArgumentNullException_WhenTranslatedWordIsNull()
	{
		// Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			new Card(Guid.NewGuid(), Guid.NewGuid(), "Hola", null!, 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
	}

	[Fact]
	public void Card_ShouldThrowArgumentException_WhenTranslatedWordIsEmpty()
	{
		// Act & Assert
		Assert.Throws<ArgumentException>(() =>
			new Card(Guid.NewGuid(), Guid.NewGuid(), "Hola", "", 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
	}

	[Fact]
	public void Card_ShouldThrowArgumentException_WhenTranslatedWordIsWhitespace()
	{
		// Act & Assert
		Assert.Throws<ArgumentException>(() =>
			new Card(Guid.NewGuid(), Guid.NewGuid(), "Hola", "   ", 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
	}
}
