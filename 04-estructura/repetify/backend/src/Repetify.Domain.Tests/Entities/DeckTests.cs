using Repetify.Domain.Entities;

using System;

using Xunit;

namespace Repetify.Tests.Domain.Entities;

public class DeckTests
{
	[Fact]
	public void Deck_Should_Initialize_With_Valid_Values()
	{
		// Arrange
		var name = "My Deck";
		var description = "This is a test deck.";
		var userId = Guid.NewGuid();
		var originalLanguage = "English";
		var translatedLanguage = "Spanish";

		// Act
		var deck = new Deck(Guid.NewGuid(),name, description, userId, originalLanguage, translatedLanguage);

		// Assert
		Assert.Equal(name, deck.Name);
		Assert.Equal(description, deck.Description);
		Assert.Equal(userId, deck.UserId);
		Assert.Equal(originalLanguage, deck.OriginalLanguage);
		Assert.Equal(translatedLanguage, deck.TranslatedLanguage);
	}

	[Fact]
	public void Deck_Should_Throw_Exception_When_Name_Is_Null()
	{
		// Arrange
		var description = "This is a test deck.";
		var userId = Guid.NewGuid();
		var originalLanguage = "English";
		var translatedLanguage = "Spanish";

		// Act & Assert
		var exception = Assert.Throws<ArgumentNullException>(() => new Deck(Guid.NewGuid(), null!, description, userId, originalLanguage, translatedLanguage));
		Assert.Equal("Value cannot be null. (Parameter 'name')", exception.Message);
	}

	[Fact]
	public void Deck_Should_Throw_Exception_When_OriginalLanguage_Is_Null()
	{
		// Arrange
		var name = "My Deck";
		var description = "This is a test deck.";
		var userId = Guid.NewGuid();
		var translatedLanguage = "Spanish";

		// Act & Assert
		var exception = Assert.Throws<ArgumentNullException>(() => new Deck(Guid.NewGuid(), name, description, userId, null!, translatedLanguage));
		Assert.Equal("Value cannot be null. (Parameter 'originalLanguage')", exception.Message);
	}

	[Fact]
	public void Deck_Should_Throw_Exception_When_TranslatedLanguage_Is_Null()
	{
		// Arrange
		var name = "My Deck";
		var description = "This is a test deck.";
		var userId = Guid.NewGuid();
		var originalLanguage = "English";

		// Act & Assert
		var exception = Assert.Throws<ArgumentNullException>(() => new Deck(Guid.NewGuid(), name, description, userId, originalLanguage, null!));
		Assert.Equal("Value cannot be null. (Parameter 'translatedLanguage')", exception.Message);
	}

	[Fact]
	public void Deck_Should_Allow_Null_Description()
	{
		// Arrange
		var name = "My Deck";
		var userId = Guid.NewGuid();
		var originalLanguage = "English";
		var translatedLanguage = "Spanish";

		// Act
		var deck = new Deck(Guid.NewGuid(), name, null, userId, originalLanguage, translatedLanguage);

		// Assert
		Assert.Equal(name, deck.Name);
		Assert.Null(deck.Description);
		Assert.Equal(userId, deck.UserId);
		Assert.Equal(originalLanguage, deck.OriginalLanguage);
		Assert.Equal(translatedLanguage, deck.TranslatedLanguage);
	}
}
