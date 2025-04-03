using Repetify.Application.Abstractions.Services;
using Repetify.Application.Common;
using Repetify.Application.Dtos;
using Repetify.Application.Extensions.Mappings;
using Repetify.Domain.Abstractions.Repositories;
using Repetify.Domain.Abstractions.Services;
using Repetify.Domain.Exceptions;

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
			return ResultFactory.Success(deckDomain.Id);
		}
		catch (EntityExistsException ex)
		{
			return ResultFactory.Conflict<Guid>(ex.Message);
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
			return ResultFactory.Success();
		}
		catch (EntityExistsException ex)
		{
			return ResultFactory.Conflict(ex.Message);
		}
	}

	///  <inheritdoc/>
	public async Task<Result<bool>> DeleteDeckAsync(Guid deckId)
	{
		var deleted = await _deckRepository.DeleteDeckAsync(deckId).ConfigureAwait(false);
		if (deleted)
		{
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			return ResultFactory.Success(true);
		}

		return ResultFactory.NotFound<bool>("Unable to find the deck to delete.");
	}

	///  <inheritdoc/>
	public async Task<Result<DeckDto>> GetDeckByIdAsync(Guid deckId)
	{
		var deck = await _deckRepository.GetDeckByIdAsync(deckId).ConfigureAwait(false);
		if (deck is null)
		{
			return ResultFactory.NotFound<DeckDto>("Deck not found.");
		}

		return ResultFactory.Success(deck.ToDto());
	}

	///  <inheritdoc/>
	public async Task<Result<IEnumerable<DeckDto>>> GetUserDecksAsync(Guid userId)
	{
		var decks = await _deckRepository.GetDecksByUserIdAsync(userId).ConfigureAwait(false);
		return ResultFactory.Success(decks.ToDtoList());
	}

	///  <inheritdoc/>
	public async Task<Result<Guid>> AddCardAsync(AddOrUpdateCardDto card, Guid deckId)
	{
		var cardDomain = card.ToEntity(deckId);
		await _deckRepository.AddCardAsync(cardDomain).ConfigureAwait(false);
		await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
		return ResultFactory.Success(cardDomain.Id);
	}

	///  <inheritdoc/>
	public async Task<Result> UpdateCardAsync(AddOrUpdateCardDto card, Guid deckId, Guid cardId)
	{
		_deckRepository.UpdateCard(card.ToEntity(deckId, cardId));
		await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
		return ResultFactory.Success();
	}

	///  <inheritdoc/>
	public async Task<Result> DeleteCardAsync(Guid deckId, Guid cardId)
	{
		var deleted = await _deckRepository.DeleteCardAsync(deckId, cardId).ConfigureAwait(false);
		if (deleted)
		{
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
			return ResultFactory.Success();
		}
		return ResultFactory.NotFound("Card not found.");
	}

	///  <inheritdoc/>
	public async Task<Result<CardDto>> GetCardByIdAsync(Guid deckId, Guid cardId)
	{
		var card = await _deckRepository.GetCardByIdAsync(deckId, cardId).ConfigureAwait(false);
		if (card is null)
		{
			return ResultFactory.NotFound<CardDto>("Card not found.");
		}
		return ResultFactory.Success(card.ToDto());
	}

	///  <inheritdoc/>
	public async Task<Result<IEnumerable<CardDto>>> GetCardsAsync(Guid deckId, int page, int pageSize)
	{
		var cards = await _deckRepository.GetCardsAsync(deckId, page, pageSize).ConfigureAwait(false);
		return ResultFactory.Success(cards.ToDtoList());
	}

	///  <inheritdoc/>
	public async Task<Result<int>> GetCardCountAsync(Guid deckId)
	{
		int count = await _deckRepository.GetCardCountAsync(deckId).ConfigureAwait(false);
		return ResultFactory.Success(count);
	}

	///  <inheritdoc/>
	public async Task<Result<IEnumerable<CardDto>>> GetCardsToReview(Guid deckId, DateTime until, int pageSize, DateTime? cursor)
	{
		if (pageSize < 1)
		{
			return ResultFactory.InvalidArgument<IEnumerable<CardDto>>("The page number must be greater than 0.");
		}

		var cards = await _deckRepository.GetCardsToReview(deckId, until, pageSize, cursor).ConfigureAwait(false);
		return ResultFactory.Success(cards.ToDtoList());
	}

	///  <inheritdoc/>
	public async Task<Result> ReviewCardAsync(Guid deckId, Guid cardId, bool isCorrect)
	{
		var card = await _deckRepository.GetCardByIdAsync(deckId, cardId).ConfigureAwait(false);
		if (card is null)
		{
			return ResultFactory.NotFound("The card to review was not found.");
		}

		_cardReviewService.UpdateReview(card, isCorrect);
		_deckRepository.UpdateCard(card);
		await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
		return ResultFactory.Success();
	}
}
