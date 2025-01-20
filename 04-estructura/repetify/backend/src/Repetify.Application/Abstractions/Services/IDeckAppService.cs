using Repetify.Application.Dtos;

namespace Repetify.Application.Abstractions.Services;

/// <summary>
/// Interface for deck application service.
/// </summary>
public interface IDeckAppService
{
	/// <summary>
	/// Adds a card to the specified deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <param name="originalWord">The original word of the card.</param>
	/// <param name="translatedWord">The translated word of the card.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task AddCardAsync(Guid deckId, string originalWord, string translatedWord);

	/// <summary>
	/// Adds a new deck.
	/// </summary>
	/// <param name="name">The name of the deck.</param>
	/// <param name="description">The description of the deck.</param>
	/// <param name="userId">The ID of the user.</param>
	/// <param name="originalLanguage">The original language of the deck.</param>
	/// <param name="translatedLanguage">The translated language of the deck.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task AddDeckAsync(string name, string? description, Guid userId, string originalLanguage, string translatedLanguage);

	/// <summary>
	/// Deletes a card from the specified deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <param name="cardId">The ID of the card.</param>
	/// <returns>A task representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
	Task<bool> DeleteCardAsync(Guid deckId, Guid cardId);

	/// <summary>
	/// Deletes the specified deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <returns>A task representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
	Task<bool> DeleteDeckAsync(Guid deckId);

	/// <summary>
	/// Gets a card by its ID from the specified deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <param name="cardId">The ID of the card.</param>
	/// <returns>A task representing the asynchronous operation, with a result of the card DTO.</returns>
	Task<CardDto?> GetCardByIdAsync(Guid deckId, Guid cardId);

	/// <summary>
	/// Gets the count of cards in the specified deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <returns>A task representing the asynchronous operation, with a result of the card count.</returns>
	Task<int> GetCardCountAsync(Guid deckId);

	/// <summary>
	/// Gets a paginated list of cards from the specified deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <param name="page">The page number.</param>
	/// <param name="pageSize">The size of the page.</param>
	/// <returns>A task representing the asynchronous operation, with a result of an enumerable of card DTOs.</returns>
	Task<IEnumerable<CardDto>> GetCardsAsync(Guid deckId, int page, int pageSize);

	/// <summary>
	/// Gets a deck by its ID.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <returns>A task representing the asynchronous operation, with a result of the deck DTO.</returns>
	Task<DeckDto?> GetDeckByIdAsync(Guid deckId);

	/// <summary>
	/// Gets a list of decks for the specified user.
	/// </summary>
	/// <param name="userId">The ID of the user.</param>
	/// <returns>A task representing the asynchronous operation, with a result of an enumerable of deck DTOs.</returns>
	Task<IEnumerable<DeckDto>> GetUserDecksAsync(Guid userId);
}