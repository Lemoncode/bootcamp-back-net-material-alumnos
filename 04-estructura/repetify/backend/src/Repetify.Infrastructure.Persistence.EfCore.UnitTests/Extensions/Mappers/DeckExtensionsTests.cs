using System;
using Xunit;
using Repetify.Domain.Entities;
using Repetify.Infrastructure.Persistence.EfCore.Entities;
using Repetify.Infrastructure.Persistence.EfCore.Extensions.Mappers;

namespace Repetify.Infrastructure.Persistence.EfCore.Tests.Extensions.Mappers;

public class DeckExtensionsTests
{
	[Fact]
	public void ToEntity_ShouldMapCorrectly()
	{
		// Arrange
		var deckId = Guid.NewGuid();
		var userId = Guid.NewGuid();
		var deck = new Deck(
			id: deckId,
			name: "Spanish Vocabulary",
			description: "Basic Spanish words",
			userId: userId,
			originalLanguage: "Spanish",
			translatedLanguage: "English"
		);

		// Act
		var entity = deck.ToDataEntity();

		// Assert
		Assert.NotNull(entity);
		Assert.Equal(deck.Id, entity.Id);
		Assert.Equal(deck.Name, entity.Name);
		Assert.Equal(deck.Description, entity.Description);
		Assert.Equal(deck.UserId, entity.UserId);
		Assert.Equal(deck.OriginalLanguage, entity.OriginalLanguage);
		Assert.Equal(deck.TranslatedLanguage, entity.TranslatedLanguage);
	}

	[Fact]
	public void ToDomain_ShouldMapCorrectly()
	{
		// Arrange
		var deckEntity = new DeckEntity
		{
			Id = Guid.NewGuid(),
			Name = "French Vocabulary",
			Description = "Intermediate French words",
			UserId = Guid.NewGuid(),
			OriginalLanguage = "French",
			TranslatedLanguage = "English"
		};

		// Act
		var domain = deckEntity.ToDomain();

		// Assert
		Assert.NotNull(domain);
		Assert.Equal(deckEntity.Id, domain.Id);
		Assert.Equal(deckEntity.Name, domain.Name);
		Assert.Equal(deckEntity.Description, domain.Description);
		Assert.Equal(deckEntity.UserId, domain.UserId);
		Assert.Equal(deckEntity.OriginalLanguage, domain.OriginalLanguage);
		Assert.Equal(deckEntity.TranslatedLanguage, domain.TranslatedLanguage);
	}

	[Fact]
	public void ToEntity_ShouldThrowArgumentNullException_WhenDeckIsNull()
	{
		// Arrange
		Deck? nullDeck = null;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => nullDeck!.ToDataEntity());
	}

	[Fact]
	public void ToDomain_ShouldThrowArgumentNullException_WhenDeckEntityIsNull()
	{
		// Arrange
		DeckEntity? nullEntity = null;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => nullEntity!.ToDomain());
	}


}
