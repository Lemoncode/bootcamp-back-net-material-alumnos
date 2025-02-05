using Microsoft.AspNetCore.Mvc;

using Repetify.Application.Abstractions.Services;
using Repetify.Application.Dtos;
using Repetify.Web.Extensions;

namespace Repetify.Web.Controllers;

/// <summary>
/// Controller to manage decks and cards.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DeckController : ControllerBase
{
	private readonly IDeckAppService _deckAppService;
	private readonly Guid _userId = Guid.Parse("D0292C56-57FC-4D64-A9F0-0191667125FC");

	/// <summary>
	/// Initializes a new instance of <see cref="DeckController"/>.
	/// </summary>
	/// <param name="deckAppService">The application service for decks.</param>
	public DeckController(IDeckAppService deckAppService)
	{
		_deckAppService = deckAppService;
	}

	/// <summary>
	/// Adds a new deck.
	/// </summary>
	/// <param name="deck">The DTO of the deck to add.</param>
	/// <returns>The created deck.</returns>
	[HttpPost]
	public async Task<IActionResult> AddDeck([FromBody] AddOrUpdateDeckDto deck)
	{
		ArgumentNullException.ThrowIfNull(deck);

		// The service now returns Result<Guid>
		var result = await _deckAppService.AddDeckAsync(deck, _userId).ConfigureAwait(false);
		return result.ToActionResult(deckId =>
		{
			var createdDeck = new DeckDto(
				deckId,
				deck.Name,
				deck.Description,
				_userId,
				deck.OriginalLanguage,
				deck.TranslatedLanguage);
			return CreatedAtAction(nameof(GetDeckById), new { deckId }, createdDeck);
		});
	}

	/// <summary>
	/// Updates an existing deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck to update.</param>
	/// <param name="deck">The DTO with the updated information.</param>
	/// <returns>NoContent on success.</returns>
	[HttpPut("{deckId}")]
	public async Task<IActionResult> UpdateDeck([FromRoute] Guid deckId, [FromBody] AddOrUpdateDeckDto deck)
	{
		ArgumentNullException.ThrowIfNull(deck);

		var result = await _deckAppService.UpdateDeckAsync(deck, _userId).ConfigureAwait(false);
		return result.ToActionResult(() => NoContent());
	}

	/// <summary>
	/// Deletes a deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck to delete.</param>
	/// <returns>NoContent if deleted or NotFound if it does not exist.</returns>
	[HttpDelete("{deckId}")]
	public async Task<IActionResult> DeleteDeck(Guid deckId)
	{
		// The service returns Result<bool>
		var result = await _deckAppService.DeleteDeckAsync(deckId).ConfigureAwait(false);
		return result.ToActionResult(deleted => deleted ? NoContent() : NotFound());
	}

	/// <summary>
	/// Gets a deck by its ID.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <returns>The deck if found or NotFound otherwise.</returns>
	[HttpGet("{deckId}")]
	public async Task<IActionResult> GetDeckById(Guid deckId)
	{
		// The service returns Result<DeckDto?>
		var result = await _deckAppService.GetDeckByIdAsync(deckId).ConfigureAwait(false);
		return result.ToActionResult(deck => Ok(deck));
	}

	/// <summary>
	/// Gets all decks of the current user.
	/// </summary>
	/// <returns>A list of decks.</returns>
	[HttpGet("user-decks")]
	public async Task<IActionResult> GetUserDecks()
	{
		var result = await _deckAppService.GetUserDecksAsync(_userId).ConfigureAwait(false);
		return result.ToActionResult(decks => Ok(decks));
	}

	/// <summary>
	/// Adds a new card to a deck.
	/// </summary>
	/// <param name="card">The DTO of the card to add.</param>
	/// <returns>Created (201) if added successfully.</returns>
	[HttpPost("cards")]
	public async Task<IActionResult> AddCard([FromBody] AddOrUpdateCardDto card)
	{
		ArgumentNullException.ThrowIfNull(card);

		var result = await _deckAppService.AddCardAsync(card).ConfigureAwait(false);
		return result.ToActionResult(cardId =>
		{
			var createdCard = new CardDto(
				cardId,
				card.DeckId,
				card.OriginalWord,
				card.TranslatedWord
				);
			return CreatedAtAction(nameof(GetCardById), new { cardId }, createdCard);
		});
	}

	/// <summary>
	/// Updates an existing card.
	/// </summary>
	/// <param name="card">The DTO with the updated information of the card.</param>
	/// <returns>NoContent on success.</returns>
	[HttpPut("cards")]
	public async Task<IActionResult> UpdateCard([FromBody] AddOrUpdateCardDto card)
	{
		var result = await _deckAppService.UpdateCardAsync(card).ConfigureAwait(false);
		return result.ToActionResult(() => NoContent());
	}

	/// <summary>
	/// Deletes a card from a deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <param name="cardId">The ID of the card to delete.</param>
	/// <returns>NoContent if deleted or NotFound if the card does not exist.</returns>
	[HttpDelete("cards/{deckId}/{cardId}")]
	public async Task<IActionResult> DeleteCard(Guid deckId, Guid cardId)
	{
		var result = await _deckAppService.DeleteCardAsync(deckId, cardId).ConfigureAwait(false);
		return result.ToActionResult();
	}

	/// <summary>
	/// Gets a card by its ID from a deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <param name="cardId">The ID of the card.</param>
	/// <returns>The card if found or NotFound otherwise.</returns>
	[HttpGet("cards/{deckId}/{cardId}")]
	public async Task<IActionResult> GetCardById(Guid deckId, Guid cardId)
	{
		var result = await _deckAppService.GetCardByIdAsync(deckId, cardId).ConfigureAwait(false);
		return result.ToActionResult(card => Ok(card));
	}

	/// <summary>
	/// Gets a paginated list of cards from a deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <param name="page">The page number.</param>
	/// <param name="pageSize">The size of the page.</param>
	/// <returns>A list of cards.</returns>
	[HttpGet("cards/{deckId}")]
	public async Task<IActionResult> GetCards(Guid deckId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
	{
		var result = await _deckAppService.GetCardsAsync(deckId, page, pageSize).ConfigureAwait(false);
		return result.ToActionResult(cards => Ok(cards));
	}

	/// <summary>
	/// Gets the count of cards in a deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <returns>The count of cards.</returns>
	[HttpGet("cards/count/{deckId}")]
	public async Task<IActionResult> GetCardCount(Guid deckId)
	{
		var result = await _deckAppService.GetCardCountAsync(deckId).ConfigureAwait(false);
		return result.ToActionResult(count => Ok(count));
	}

	/// <summary>
	/// Gets a list of cards to review from a deck until a certain date.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <param name="until">The date until which to get cards for review.</param>
	/// <param name="pageSize">The number of cards to retrieve.</param>
	/// <param name="cursor">Cursor for pagination (optional).</param>
	/// <returns>A list of cards to review.</returns>
	[HttpGet("cards/review/{deckId}")]
	public async Task<IActionResult> GetCardsToReview(Guid deckId, [FromQuery] DateTime until, [FromQuery] int pageSize, [FromQuery] DateTime? cursor = null)
	{
		if (pageSize < 1)
		{
			return BadRequest("Page size must be greater than 0.");
		}

		var result = await _deckAppService.GetCardsToReview(deckId, until, pageSize, cursor).ConfigureAwait(false);
		return result.ToActionResult(cards => Ok(cards));
	}

	/// <summary>
	/// Reviews a card in a deck.
	/// </summary>
	/// <param name="deckId">The ID of the deck.</param>
	/// <param name="cardId">The ID of the card.</param>
	/// <param name="isCorrect">Indicates whether the review was correct.</param>
	/// <returns>NoContent on success.</returns>
	[HttpPost("cards/review/{deckId}/{cardId}")]
	public async Task<IActionResult> ReviewCard(Guid deckId, Guid cardId, [FromQuery] bool isCorrect)
	{
		var result = await _deckAppService.ReviewCardAsync(deckId, cardId, isCorrect).ConfigureAwait(false);
		return result.ToActionResult(() => NoContent());
	}
}
