using Repetify.Application.Abstractions.Services;
using Repetify.Application.Common;
using Repetify.Application.Dtos;
using Repetify.Application.Enums;
using Repetify.Application.Extensions.Mappings;
using Repetify.Domain.Abstractions.Repositories;
using Repetify.Domain.Abstractions.Services;

namespace Repetify.Application.Services;

/// <summary>
/// Service to handle decks and cards
/// </summary>
public class DeckAppService : IDeckAppService
{
	private readonly IDeckValidator _deckValidator;
	private readonly ICardReviewService _cardReviewService;
	private readonly IDeckRepository _deckRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="DeckAppService"/> class.
	/// </summary>
	/// <param name="deckValidator">The deck validator.</param>
	/// <param name="cardReviewService">The card review service.</param>
	/// <param name="deckRepository">The deck repository.</param>
	public DeckAppService(
		IDeckValidator deckValidator,
		ICardReviewService cardReviewService,
		IDeckRepository deckRepository)
	{
		_deckValidator = deckValidator;
		_cardReviewService = cardReviewService;
		_deckRepository = deckRepository;
	}

	/// <summary>
	/// Adds a new deck asynchronously.
	/// </summary>
	/// <param name="deck">The deck to add.</param>
	/// <param name="userId">The user ID.</param>
	/// <returns>The result containing the new deck ID.</returns>
	public async Task<Result<Guid>> AddDeckAsync(AddOrUpdateDeckDto deck, Guid userId)
	{
		try
		{
			var deckDomain = deck.ToEntity(userId);
			await _deckRepository.AddDeckAsync(deckDomain).ConfigureAwait(false);
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			return Success(deckDomain.Id);
		}
		catch (Exception)
		{
			return UnexpectedError<Guid>("Error when adding a new deck.");
		}
	}

	/// <summary>
	/// Updates an existing deck asynchronously.
	/// </summary>
	/// <param name="deck">The deck to update.</param>
	/// <param name="userId">The user ID.</param>
	/// <returns>The result of the update operation.</returns>
	public async Task<Result> UpdateDeckAsync(AddOrUpdateDeckDto deck, Guid userId)
	{
		try
		{
			var deckDomain = deck.ToEntity(userId);
			_deckRepository.UpdateDeck(deckDomain);
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			return Success();
		}
		catch (Exception)
		{
			return UnexpectedError("Error when updating the deck.");
		}
	}

	/// <summary>
	/// Deletes a deck asynchronously.
	/// </summary>
	/// <param name="deckId">The deck ID.</param>
	/// <returns>The result of the delete operation.</returns>
	public async Task<Result<bool>> DeleteDeckAsync(Guid deckId)
	{
		try
		{
			var deleted = await _deckRepository.DeleteDeckAsync(deckId).ConfigureAwait(false);
			if (deleted)
			{
				await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			}
			if (deleted)
			{
				return Success(true);
			}
			else
			{
				return NotFound<bool>("Unable to find the deck to delete.");
			}
		}
		catch (Exception)
		{
			return UnexpectedError<bool>("Error when deleting the deck.");
		}
	}

	/// <summary>
	/// Gets a deck by its ID asynchronously.
	/// </summary>
	/// <param name="deckId">The deck ID.</param>
	/// <returns>The result containing the deck DTO.</returns>
	public async Task<Result<DeckDto>> GetDeckByIdAsync(Guid deckId)
	{
		try
		{
			var deck = await _deckRepository.GetDeckByIdAsync(deckId).ConfigureAwait(false);
			if (deck is null)
			{
				return NotFound<DeckDto>("Deck not found.");
			}
			return Success(deck.ToDto());
		}
		catch (Exception)
		{
			return UnexpectedError<DeckDto>("Error when retrieving a card.");
		}
	}

	/// <summary>
	/// Gets the decks of a user asynchronously.
	/// </summary>
	/// <param name="userId">The user ID.</param>
	/// <returns>The result containing the list of deck DTOs.</returns>
	public async Task<Result<IEnumerable<DeckDto>>> GetUserDecksAsync(Guid userId)
	{
		try
		{
			var decks = await _deckRepository.GetDecksByUserIdAsync(userId).ConfigureAwait(false);
			return Success(decks.ToDtoList());
		}
		catch (Exception)
		{
			return UnexpectedError<IEnumerable<DeckDto>>("Error when retrieving the cards for a user.");
		}
	}

	/// <summary>
	/// Adds a new card asynchronously.
	/// </summary>
	/// <param name="card">The card to add.</param>
	/// <returns>The result containing the new card ID.</returns>
	public async Task<Result<Guid>> AddCardAsync(AddOrUpdateCardDto card)
	{
		try
		{
			var cardDomain = card.ToEntity();
			await _deckRepository.AddCardAsync(cardDomain).ConfigureAwait(false);
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			return Success(cardDomain.Id);
		}
		catch (Exception)
		{
			return UnexpectedError<Guid>("Error when adding a card."); ;
		}
	}

	/// <summary>
	/// Updates an existing card asynchronously.
	/// </summary>
	/// <param name="card">The card to update.</param>
	/// <returns>The result of the update operation.</returns>
	public async Task<Result> UpdateCardAsync(AddOrUpdateCardDto card)
	{
		try
		{
			_deckRepository.UpdateCard(card.ToEntity());
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			return Success();
		}
		catch (Exception)
		{
			return UnexpectedError("Error when updating a card.");
		}
	}

	/// <summary>
	/// Deletes a card asynchronously.
	/// </summary>
	/// <param name="deckId">The deck ID.</param>
	/// <param name="cardId">The card ID.</param>
	/// <returns>The result of the delete operation.</returns>
	public async Task<Result> DeleteCardAsync(Guid deckId, Guid cardId)
	{
		try
		{
			var deleted = await _deckRepository.DeleteCardAsync(deckId, cardId).ConfigureAwait(false);
			if (deleted)
			{
				await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
				return Success();
			}
			return NotFound("Card not found.");
		}
		catch (Exception)
		{
			return UnexpectedError("Error when deleting the card.");
		}
	}

	/// <summary>
	/// Gets a card by its ID asynchronously.
	/// </summary>
	/// <param name="deckId">The deck ID.</param>
	/// <param name="cardId">The card ID.</param>
	/// <returns>The result containing the card DTO.</returns>
	public async Task<Result<CardDto>> GetCardByIdAsync(Guid deckId, Guid cardId)
	{
		try
		{
			var card = await _deckRepository.GetCardByIdAsync(deckId, cardId).ConfigureAwait(false);
			if (card is null)
			{
				return NotFound<CardDto>("Card not found.");
			}
			return Success(card.ToDto());
		}
		catch (Exception)
		{
			return UnexpectedError<CardDto>("Error when retrieving a card.");
		}
	}

	/// <summary>
	/// Gets the cards of a deck asynchronously with pagination.
	/// </summary>
	/// <param name="deckId">The deck ID.</param>
	/// <param name="page">The page number.</param>
	/// <param name="pageSize">The page size.</param>
	/// <returns>The result containing the list of card DTOs.</returns>
	public async Task<Result<IEnumerable<CardDto>>> GetCardsAsync(Guid deckId, int page, int pageSize)
	{
		try
		{
			var cards = await _deckRepository.GetCardsAsync(deckId, page, pageSize).ConfigureAwait(false);
			return Success(cards.ToDtoList());
		}
		catch (Exception)
		{
			return UnexpectedError<IEnumerable<CardDto>>("Error when retrieving the cards of a deck.");
		}
	}

	/// <summary>
	/// Gets the count of cards in a deck asynchronously.
	/// </summary>
	/// <param name="deckId">The deck ID.</param>
	/// <returns>The result containing the card count.</returns>
	public async Task<Result<int>> GetCardCountAsync(Guid deckId)
	{
		try
		{
			int count = await _deckRepository.GetCardCountAsync(deckId).ConfigureAwait(false);
			return Success(count);
		}
		catch (Exception)
		{
			return UnexpectedError<int>("Error when retrieving the number of cards of a deck.");
		}
	}

	/// <summary>
	/// Gets the cards to review from a deck asynchronously.
	/// </summary>
	/// <param name="deckId">The deck ID.</param>
	/// <param name="until">The date until which to get cards for review.</param>
	/// <param name="pageSize">The number of cards to retrieve.</param>
	/// <param name="cursor">The cursor for pagination.</param>
	/// <returns>The result containing the list of card DTOs.</returns>
	public async Task<Result<IEnumerable<CardDto>>> GetCardsToReview(Guid deckId, DateTime until, int pageSize, DateTime? cursor)
	{
		try
		{
			if (pageSize < 1)
			{
				return new Result<IEnumerable<CardDto>>(ResultStatus.InvalidARguments, "The page number must be greater than 0.");
			}

			var cards = await _deckRepository.GetCardsToReview(deckId, until, pageSize, cursor).ConfigureAwait(false);
			return Success(cards.ToDtoList());
		}
		catch (Exception)
		{
			return UnexpectedError<IEnumerable<CardDto>>("Error when retrieving the cards to be reviewed.");
		}
	}

	/// <summary>
	/// Reviews a card asynchronously.
	/// </summary>
	/// <param name="deckId">The deck ID.</param>
	/// <param name="cardId">The card ID.</param>
	/// <param name="isCorrect">Indicates whether the review was correct.</param>
	/// <returns>The result of the review operation.</returns>
	public async Task<Result> ReviewCardAsync(Guid deckId, Guid cardId, bool isCorrect)
	{
		try
		{
			var card = await _deckRepository.GetCardByIdAsync(deckId, cardId).ConfigureAwait(false);
			if (card is null)
			{
				return NotFound("The card to review was not found.");
			}

			_cardReviewService.UpdateReview(card, isCorrect);
			_deckRepository.UpdateCard(card);
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			return Success();
		}
		catch (Exception)
		{
			return UnexpectedError("Error when reviewing a card.");
		}
	}

	private static Result<T> Success<T>(T value) => new(value);

	private static Result Success() => new();

	private static Result<T> UnexpectedError<T>(string? errorMessage = null) => new(ResultStatus.UnexpectedError, errorMessage);

	private static Result UnexpectedError(string? errorMessage = null) => new(ResultStatus.UnexpectedError, errorMessage);

	private static Result<T> NotFound<T>(string? errorMessage = null) => new(ResultStatus.NotFound, errorMessage);

	private static Result NotFound(string? errorMessage = null) => new(ResultStatus.NotFound, errorMessage);
}
