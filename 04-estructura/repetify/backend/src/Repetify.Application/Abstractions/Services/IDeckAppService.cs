using Repetify.Application.Dtos;
using Repetify.Domain.Entities;

namespace Repetify.Application.Abstractions.Services;

/// <summary>
/// Interface for deck application service.
/// </summary>
public interface IDeckAppService
{

	/// <summary>
	/// Adds a new deck.
	/// </summary>
	/// <param name="deck">The deck to add.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task AddDeckAsync(DeckDto deck);

	/// <summary>
	/// Updates an existing deck.
	/// </summary>
	/// <param name="deck">The deck DTO containing updated information.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task UpdateDeckAsync(DeckDto deck);

	/// <summary>
	/// Deletes the specified deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <returns>A task representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
	Task<bool> DeleteDeckAsync(Guid deckId);

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

	/// <summary>
	/// Adds a card to the specified deck.
	/// </summary>
	/// <param name="card">The card to be updated.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task AddCardAsync(CardDto card);

	/// <summary>
	/// Updates a card in the specified deck.
	/// </summary>
	/// <param name="Card">The card to be updated.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task UpdateCardAsync(CardDto card);

	/// <summary>
	/// Deletes a card from the specified deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <param name="cardId">The ID of the card.</param>
	/// <returns>A task representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
	Task<bool> DeleteCardAsync(Guid deckId, Guid cardId);

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
	/// Gets a list of cards to review from the specified deck until a certain date.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <param name="until">The date until which to get cards for review.</param>
	/// <param name="pageSize">The number of cards to retrieve.</param>
	/// <param name="cursor">The cursor for pagination, representing the last retrieved card's review date.</param>
	/// <returns>A task representing the asynchronous operation, with a result of an enumerable of card DTOs.</returns>
	Task<IEnumerable<CardDto>> GetCardsToReview(Guid deckId, DateTime until, int pageSize, DateTime? cursor);
}