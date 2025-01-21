using Repetify.Domain.Entities;

namespace Repetify.Application.Abstractions.Repositories;

/// <summary>
/// Interface for deck repository operations.
/// </summary>
public interface IDeckRepository
{
	/// <summary>
	/// Adds a new deck asynchronously.
	/// </summary>
	/// <param name="deck">The deck to add.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task AddDeckAsync(Deck deck);

	/// <summary>
	/// Deletes a deck asynchronously by its identifier.
	/// </summary>
	/// <param name="deckId">The identifier of the deck to delete.</param>
	/// <returns>A task representing the asynchronous operation which results will be true (deleted) or false otherwise.</returns>
	Task<bool> DeleteDeckAsync(Guid deckId);

	/// <summary>
	/// Updates an existing deck.
	/// </summary>
	/// <param name="deck">The deck to update.</param>
	void UpdateDeck(Deck deck);

	/// <summary>
	/// Gets a deck by its identifier asynchronously.
	/// </summary>
	/// <param name="deckId">The identifier of the deck to retrieve.</param>
	/// <returns>A task representing the asynchronous operation, with a deck as the result.</returns>
	Task<Deck?> GetDeckByIdAsync(Guid deckId);

	/// <summary>
	/// Gets all decks associated with a specific user asynchronously.
	/// </summary>
	/// <param name="userId">The identifier of the user.</param>
	/// <returns>A task representing the asynchronous operation, with a list of decks as the result.</returns>
	Task<IEnumerable<Deck>> GetDecksByUserIdAsync(Guid userId);

	/// <summary>
	/// Gets the count of cards in a deck asynchronously.
	/// </summary>
	/// <param name="deckId">The identifier of the deck.</param>
	/// <returns>A task representing the asynchronous operation, with the card count as the result.</returns>
	Task<int> GetCardCountAsync(Guid deckId);

	/// <summary>
	/// Gets a card by its identifier within a specific deck asynchronously.
	/// </summary>
	/// <param name="deckId">The identifier of the deck.</param>
	/// <param name="cardId">The identifier of the card to retrieve.</param>
	/// <returns>A task representing the asynchronous operation, with a card as the result.</returns>
	Task<Card?> GetCardByIdAsync(Guid deckId, Guid cardId);

	/// <Summary>
	/// Gets a list of cards to review according to a deadline and a date cursor (optional) from a specific deck asynchronously.
	/// </summary>
	/// <param name="deckId">The identifier of the deck.</param>
	/// <param name="until">The date until which cards should be reviewed.</param>
	/// <param name="pageSize">The number of cards to retrieve.</param>
	/// <param name="cursor">The cursor for pagination.</param>
	/// <returns>A task representing the asynchronous operation, with a list of cards as the result.</returns>
	Task<IEnumerable<Card>> GetCardsToReview(Guid deckId, DateTime until, int pageSize, DateTime? cursor);

	/// <summary>
	/// Adds a new card to a specific deck asynchronously.
	/// </summary>
	/// <param name="card">The card to add.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task AddCardAsync(Card card);

	/// <summary>
	/// Updates an existing card within a specific deck asynchronously.
	/// </summary>
	/// <param name="card">The card to update.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	void UpdateCard(Card card);

	/// <summary>
	/// Deletes a card from a specific deck asynchronously.
	/// </summary>
	/// <param name="deckId">The identifier of the deck containing the card.</param>
	/// <param name="cardId">The identifier of the card to delete.</param>
	/// <returns>A task representing the asynchronous operation which results will be true (deleted) or false otherwise.</returns>
	Task<bool> DeleteCardAsync(Guid deckId, Guid cardId);

	/// <summary>
	/// Gets a paginated list of cards from a specific deck asynchronously.
	/// </summary>
	/// <param name="deckId">The identifier of the deck.</param>
	/// <param name="page">The page number to retrieve.</param>
	/// <param name="pageSize">The number of cards per page.</param>
	/// <returns>A task representing the asynchronous operation, with a list of cards as the result.</returns>
	Task<IEnumerable<Card>> GetCardsAsync(Guid deckId, int page, int pageSize);

	/// <summary>
	/// Saves all changes made in the context of this repository asynchronously.
	/// </summary>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task SaveChangesAsync();
}