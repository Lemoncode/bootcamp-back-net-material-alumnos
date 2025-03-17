using Microsoft.EntityFrameworkCore;

using Repetify.Domain.Entities;
using Repetify.Infrastructure.Persistence.EfCore.Context;
using Repetify.Infrastructure.Persistence.EfCore.Repositories;


namespace Repetify.Infrastructure.Persistence.EfCore.Tests.Repositories;

public class DeckRepositoryTests
{
	private static RepetifyDbContext GetInMemoryDbContext()
	{
		var options = new DbContextOptionsBuilder<RepetifyDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())  // Usa un nombre único para cada test
			.Options;

		return new(options);
	}

	[Fact]
	public async Task AddAsync_ShouldAddDeckSuccessfully()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var deck = new Deck(Guid.NewGuid(), "Test Deck", "Test Description", Guid.NewGuid(), "EN", "ES");

		// Act
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		// Assert
		var storedDeck = await dbContext.Decks.FirstOrDefaultAsync(d => d.Id == deck.Id);
		Assert.NotNull(storedDeck);
		Assert.Equal(deck.Name, storedDeck.Name);
		Assert.Equal(deck.OriginalLanguage, storedDeck.OriginalLanguage);
	}

	[Fact]
	public async Task DeleteAsync_ShouldReturnTrue_WhenDeckExists()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var deck = new Deck(Guid.NewGuid(), "Deck to delete", "Test", Guid.NewGuid(), "EN", "FR");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		// Act
		var result = await repository.DeleteDeckAsync(deck.Id);
		await repository.SaveChangesAsync();

		// Assert
		Assert.True(result);
		Assert.Null(await dbContext.Decks.FirstOrDefaultAsync(d => d.Id == deck.Id));
	}

	[Fact]
	public async Task DeleteAsync_ShouldReturnFalse_WhenDeckDoesNotExist()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		// Act
		var result = await repository.DeleteDeckAsync(Guid.NewGuid());

		// Assert
		Assert.False(result);
	}


	[Fact]
	public async Task GetByIdAsync_ShouldReturnDeck_WhenExists()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var deck = new Deck(Guid.NewGuid(), "Spanish Deck", "Learning Spanish", Guid.NewGuid(), "EN", "ES");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		// Act
		var result = await repository.GetDeckByIdAsync(deck.Id);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(deck.Name, result.Name);
		Assert.Equal(deck.OriginalLanguage, result.OriginalLanguage);
	}

	[Fact]
	public async Task GetByIdAsync_ShouldReturnNull_WhenDeckDoesNotExist()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		// Act
		var result = await repository.GetDeckByIdAsync(Guid.NewGuid());

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetDecksByUserIdAsync_ShouldReturnDecks_WhenUserHasDecks()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var userId = Guid.NewGuid();
		var deck1 = new Deck(Guid.NewGuid(), "Deck 1", "Description 1", userId, "EN", "FR");
		var deck2 = new Deck(Guid.NewGuid(), "Deck 2", "Description 2", userId, "EN", "ES");

		await repository.AddDeckAsync(deck1);
		await repository.AddDeckAsync(deck2);
		await repository.SaveChangesAsync();

		// Act
		var decks = await repository.GetDecksByUserIdAsync(userId);

		// Assert
		Assert.NotNull(decks);
		Assert.Equal(2, decks.Count());
		Assert.Contains(decks, d => d.Name == "Deck 1");
		Assert.Contains(decks, d => d.Name == "Deck 2");
	}

	[Fact]
	public async Task GetDecksByUserIdAsync_ShouldReturnEmptyList_WhenUserHasNoDecks()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var userId = Guid.NewGuid();

		// Act
		var decks = await repository.GetDecksByUserIdAsync(userId);

		// Assert
		Assert.NotNull(decks);
		Assert.Empty(decks);
	}

	[Fact]
	public async Task GetDecksByUserIdAsync_ShouldReturnOnlyUserDecks()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var userId1 = Guid.NewGuid();
		var userId2 = Guid.NewGuid();

		var deck1 = new Deck(Guid.NewGuid(), "Deck 1", "Description 1", userId1, "EN", "FR");
		var deck2 = new Deck(Guid.NewGuid(), "Deck 2", "Description 2", userId2, "EN", "ES");

		await repository.AddDeckAsync(deck1);
		await repository.AddDeckAsync(deck2);
		await repository.SaveChangesAsync();

		// Act
		var decks = await repository.GetDecksByUserIdAsync(userId1);

		// Assert
		Assert.NotNull(decks);
		Assert.Single(decks);
		Assert.Equal("Deck 1", decks.First().Name);
		Assert.DoesNotContain(decks, d => d.UserId == userId2);
	}

	[Fact]
	public async Task GetDecksByUserIdAsync_ShouldHandleMultipleDecksPerUser()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var userId = Guid.NewGuid();

		var decks = new List<Deck>
		{
			new Deck(Guid.NewGuid(), "Deck A", "Desc A", userId, "EN", "DE"),
			new Deck(Guid.NewGuid(), "Deck B", "Desc B", userId, "EN", "IT"),
			new Deck(Guid.NewGuid(), "Deck C", "Desc C", userId, "EN", "PT")
		};

		foreach (var deck in decks)
		{
			await repository.AddDeckAsync(deck);
		}
		await repository.SaveChangesAsync();

		// Act
		var resultDecks = await repository.GetDecksByUserIdAsync(userId);

		// Assert
		Assert.NotNull(resultDecks);
		Assert.Equal(3, resultDecks.Count());
		Assert.Contains(resultDecks, d => d.Name == "Deck A");
		Assert.Contains(resultDecks, d => d.Name == "Deck B");
		Assert.Contains(resultDecks, d => d.Name == "Deck C");
	}

	[Fact]
	public async Task GetCardsAsync_ShouldReturnPaginatedResults()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var deck = new Deck(Guid.NewGuid(), "Test Deck", "Test Description", Guid.NewGuid(), "EN", "ES");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		var card1 = new Card(Guid.NewGuid(), deck.Id, "Word1", "Translation1");
		var card2 = new Card(Guid.NewGuid(), deck.Id, "Word2", "Translation2");

		await repository.AddCardAsync(card1);
		await repository.AddCardAsync(card2);
		await repository.SaveChangesAsync();

		// Act
		var cards = await repository.GetCardsAsync(deck.Id, page: 1, pageSize: 1);

		// Assert
		Assert.Single(cards);
		Assert.Equal("Word1", cards.First().OriginalWord);
	}

	[Fact]
	public async Task GetCardCountAsync_ShouldReturnCorrectCount()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var deck = new Deck(Guid.NewGuid(), "Test Deck", "Test Description", Guid.NewGuid(), "EN", "ES");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		await repository.AddCardAsync(new Card(Guid.NewGuid(), deck.Id, "Word1", "Translation1"));
		await repository.AddCardAsync(new Card(Guid.NewGuid(), deck.Id, "Word2", "Translation2"));
		await repository.SaveChangesAsync();

		// Act
		var count = await repository.GetCardCountAsync(deck.Id);

		// Assert
		Assert.Equal(2, count);
	}

	[Fact]
	public async Task AddCardAsync_ShouldAddCardToDeck()
	{
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var deck = new Deck(Guid.NewGuid(), "Vocabulary", "Spanish words", Guid.NewGuid(), "EN", "ES");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		var card = new Card(Guid.NewGuid(), deck.Id, "Hola", "Hello");

		await repository.AddCardAsync(card);
		await repository.SaveChangesAsync();

		var storedCard = await dbContext.Cards.FirstOrDefaultAsync(c => c.OriginalWord == "Hola");
		Assert.NotNull(storedCard);
		Assert.Equal("Hello", storedCard.TranslatedWord);
	}

	[Fact]
	public async Task DeleteCardAsync_ShouldReturnTrue_WhenCardExists()
	{
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var deck = new Deck(Guid.NewGuid(), "Languages", "Learning French", Guid.NewGuid(), "EN", "FR");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		var card = new Card(Guid.NewGuid(), deck.Id, "Chat", "Cat");
		await repository.AddCardAsync(card);
		await repository.SaveChangesAsync();

		var result = await repository.DeleteCardAsync(deck.Id, card.Id);
		await repository.SaveChangesAsync();

		Assert.True(result);
		var deletedCard = await dbContext.Cards.FirstOrDefaultAsync(c => c.Id == card.Id);
		Assert.Null(deletedCard);
	}

	[Fact]
	public async Task DeleteCardAsync_ShouldReturnFalse_WhenCardDoesNotExist()
	{
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var deck = new Deck(Guid.NewGuid(), "Practice", "Deck for tests", Guid.NewGuid(), "EN", "IT");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		var result = await repository.DeleteCardAsync(deck.Id, Guid.NewGuid());

		Assert.False(result);
	}

	[Fact]
	public async Task GetCardByIdAsync_ShouldReturnCard_WhenExists()
	{
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var deck = new Deck(Guid.NewGuid(), "Words", "Deck description", Guid.NewGuid(), "EN", "DE");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		var card = new Card(Guid.NewGuid(), deck.Id, "Auto", "Car");
		await repository.AddCardAsync(card);
		await repository.SaveChangesAsync();

		var retrievedCard = await repository.GetCardByIdAsync(deck.Id, card.Id);

		Assert.NotNull(retrievedCard);
		Assert.Equal("Auto", retrievedCard.OriginalWord);
	}

	[Fact]
	public async Task DeckNameExistsForUser_ShouldReturnTrue_WhenDeckNameExistsForUser()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var userId = Guid.NewGuid();
		var name = "should be unique";
		var deck = new Deck(Guid.NewGuid(), name, "Description", userId, "EN", "ES");
		var deckToTest = new Deck(Guid.NewGuid(), name, "Description", userId, "EN", "ES");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		// Act
		var exists = await repository.DeckNameExistsForUserAsync(deckToTest.Id, name, userId);

		// Assert
		Assert.True(exists);
	}

	[Fact]
	public async Task DeckNameExistsForUser_ShouldReturnFalse_WhenDeckNameExistsForOtherUser()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var name = "should be unique";
		var deck = new Deck(Guid.NewGuid(), name, "Description", Guid.NewGuid(), "EN", "ES");
		var deckToTest = new Deck(Guid.NewGuid(), name, "Description", Guid.NewGuid(), "EN", "ES");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		// Act
		var exists = await repository.DeckNameExistsForUserAsync(deckToTest.Id, name, deckToTest.UserId);

		// Assert
		Assert.False(exists);
	}

	[Fact]
	public async Task DeckNameExistsForUser_ShouldReturnFalse_WhenDeckNameDoesNotExistForUser()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var userId = Guid.NewGuid();
		var repository = new DeckRepository(dbContext);
		var deck = new Deck(Guid.NewGuid(), "Existing Deck", "Description", userId, "EN", "ES");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		// Act
		var exists = await repository.DeckNameExistsForUserAsync(deck.Id, "Non-Existing Deck", userId);

		// Assert
		Assert.False(exists);
	}

	[Fact]
	public async Task DeckNameExistsForUser_ShouldReturnFalse_WhenDeckNameExistsForSameDeck()
	{
		// Arrange
		using var dbContext = GetInMemoryDbContext();
		var repository = new DeckRepository(dbContext);

		var userId = Guid.NewGuid();
		var deck = new Deck(Guid.NewGuid(), "Existing Deck", "Description", userId, "EN", "ES");
		await repository.AddDeckAsync(deck);
		await repository.SaveChangesAsync();

		// Act
		var exists = await repository.DeckNameExistsForUserAsync(deck.Id, deck.Name, userId);

		// Assert
		Assert.False(exists);
	}
}
