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

	///  <inheritdoc/>
	public async Task<Result<Guid>> AddDeckAsync(AddOrUpdateDeckDto deck, Guid userId)
	{
		try
		{
			var deckDomain = deck.ToEntity(userId);
			await _deckValidator.EnsureIsValid(deckDomain).ConfigureAwait(false);
			await _deckRepository.AddDeckAsync(deckDomain).ConfigureAwait(false);
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			return Success(deckDomain.Id);
		}
		catch (Exception)
		{
			return UnexpectedError<Guid>("Error when adding a new deck.");
		}
	}

	///  <inheritdoc/>
	public async Task<Result> UpdateDeckAsync(AddOrUpdateDeckDto deck, Guid userId)
	{
		try
		{
			var deckDomain = deck.ToEntity(userId);
			await _deckValidator.EnsureIsValid(deckDomain).ConfigureAwait(false);
			_deckRepository.UpdateDeck(deckDomain);
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			return Success();
		}
		catch (Exception)
		{
			return UnexpectedError("Error when updating the deck.");
		}
	}

	///  <inheritdoc/>
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

	///  <inheritdoc/>
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

	///  <inheritdoc/>
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

	///  <inheritdoc/>
	public async Task<Result<Guid>> AddCardAsync(AddOrUpdateCardDto card, Guid deckId)
	{
		try
		{
			var cardDomain = card.ToEntity(deckId);
			await _deckRepository.AddCardAsync(cardDomain).ConfigureAwait(false);
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			return Success(cardDomain.Id);
		}
		catch (Exception)
		{
			return UnexpectedError<Guid>("Error when adding a card."); ;
		}
	}

	///  <inheritdoc/>
	public async Task<Result> UpdateCardAsync(AddOrUpdateCardDto card, Guid deckId, Guid cardId)
	{
		try
		{
			_deckRepository.UpdateCard(card.ToEntity(deckId, cardId));
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			return Success();
		}
		catch (Exception)
		{
			return UnexpectedError("Error when updating a card.");
		}
	}

	///  <inheritdoc/>
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

	///  <inheritdoc/>
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

	///  <inheritdoc/>
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

	///  <inheritdoc/>
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

	///  <inheritdoc/>
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

	///  <inheritdoc/>
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
