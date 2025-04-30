using Repetify.Domain.Entities;

using System;

namespace Repetify.Domain.Tests.Entities;

public class CardTests
{

	[Fact]
	public void Card_Should_Initialize_With_Valid_Values()
	{
		// Arrange
		var originalWord = "Hello";
		var translatedWord = "Hola";

		// Act
		var card = new Card(Guid.NewGuid(), Guid.NewGuid(), originalWord, translatedWord);

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
		var exception = Assert.Throws<ArgumentNullException>(() =>
			new Card(Guid.NewGuid(), null!, "Hello", 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
		Assert.Equal("originalWord", exception.ParamName);
	}

	[Fact]
	public void Card_ShouldThrowArgumentException_WhenOriginalWordIsEmpty()
	{
		// Act & Assert
		var exception = Assert.Throws<ArgumentException>(() =>
			new Card(Guid.NewGuid(), "", "Hello", 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
		Assert.Equal("originalWord", exception.ParamName);
	}

	[Fact]
	public void Card_ShouldThrowArgumentException_WhenOriginalWordIsWhitespace()
	{
		// Act & Assert
		var exception = Assert.Throws<ArgumentException>(() =>
			new Card(Guid.NewGuid(), "   ", "Hello", 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
		Assert.Equal("originalWord", exception.ParamName);
	}

	[Fact]
	public void Card_ShouldThrowArgumentNullException_WhenTranslatedWordIsNull()
	{
		// Act & Assert
		var exception = Assert.Throws<ArgumentNullException>(() =>
			new Card(Guid.NewGuid(), "Hola", null!, 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
		Assert.Equal("translatedWord", exception.ParamName);
	}

	[Fact]
	public void Card_ShouldThrowArgumentException_WhenTranslatedWordIsEmpty()
	{
		// Act & Assert
		var exception = Assert.Throws<ArgumentException>(() =>
			new Card(Guid.NewGuid(), "Hola", "", 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
		Assert.Equal("translatedWord", exception.ParamName);
	}

	[Fact]
	public void Card_ShouldThrowArgumentException_WhenTranslatedWordIsWhitespace()
	{
		// Act & Assert
		var exception = Assert.Throws<ArgumentException>(() =>
			new Card(Guid.NewGuid(), "Hola", "   ", 0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
		Assert.Equal("translatedWord", exception.ParamName);
	}

	[Fact]
	public void Card_ShouldThrowArgumentOutOfRangeException_WhenCorrectReviewStreakIsNegative()
	{
		// Act & Assert
		var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
			new Card(Guid.NewGuid(), "Hola", "Hello", -1, DateTime.UtcNow.AddDays(1), DateTime.UtcNow));
		Assert.Equal("correctReviewStreak", exception.ParamName);
	}
}
