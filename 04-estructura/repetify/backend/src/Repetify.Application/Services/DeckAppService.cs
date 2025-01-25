using Repetify.Application.Abstractions.Repositories;
using Repetify.Application.Abstractions.Services;
using Repetify.Application.Dtos;
using Repetify.Application.Exceptions;
using Repetify.Application.Extensions.Mappings;
using Repetify.Domain.Abstractions.Services;

namespace Repetify.Application.Services;

/// <summary>
/// Service for managing decks and cards.
/// </summary>
public class DeckAppService : IDeckAppService
{
	private readonly IDeckRepository _deckRepository;

	private readonly ICardReviewService _reviewCardService;

	/// <summary>
	/// Initializes a new instance of the <see cref="DeckAppService"/> class.
	/// </summary>
	/// <param name="deckRepository">The repository for deck operations.</param>
	/// <param name="reviewCardService">The service for card review operations.</param></param>
	public DeckAppService(IDeckRepository deckRepository, ICardReviewService reviewCardService)
	{
		_deckRepository = deckRepository;
		_reviewCardService = reviewCardService;
	}

	/// <inheritdoc />
	public async Task<Guid> AddDeckAsync(DeckDto deck)
	{
		var deckDomain = deck.ToEntity();
		await _deckRepository.AddDeckAsync(deckDomain).ConfigureAwait(false);
		await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
		return deckDomain.Id;
	}

	/// <inheritdoc />
	public async Task UpdateDeckAsync(DeckDto deck)
	{
		var deckDomain = deck.ToEntity();
		_deckRepository.UpdateDeck(deckDomain);
		await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
	}

	/// <inheritdoc />
	public async Task<bool> DeleteDeckAsync(Guid deckId)
	{
		var result = await _deckRepository.DeleteDeckAsync(deckId).ConfigureAwait(false);
		if (result)
		{
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
		}
		return result;
	}

	/// <inheritdoc />
	public async Task<DeckDto?> GetDeckByIdAsync(Guid deckId)
	{
		var deck = await _deckRepository.GetDeckByIdAsync(deckId).ConfigureAwait(false);
		return deck?.ToDto();
	}


	/// <inheritdoc />
	public async Task<IEnumerable<DeckDto>> GetUserDecksAsync(Guid userId)
	{
		var decks = await _deckRepository.GetDecksByUserIdAsync(userId).ConfigureAwait(false);
		return decks.ToDtoList();
	}

	/// <inheritdoc />
	public async Task<Guid> AddCardAsync(CardDto card)
	{
		var cardDomain = card.ToEntity();
		await _deckRepository.AddCardAsync(cardDomain).ConfigureAwait(false);
		await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
		return cardDomain.Id;
	}

	/// <inheritdoc />
	public async Task UpdateCardAsync(CardDto card)
	{
		_deckRepository.UpdateCard(card.ToEntity());
		await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
	}

	/// <inheritdoc />
	public async Task<bool> DeleteCardAsync(Guid deckId, Guid cardId)
	{
		var result = await _deckRepository.DeleteCardAsync(deckId, cardId).ConfigureAwait(false);
		if (result)
		{
			await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
		}
		return result;
	}

	/// <inheritdoc />
	public async Task<CardDto?> GetCardByIdAsync(Guid deckId, Guid cardId)
	{
		var card = await _deckRepository.GetCardByIdAsync(deckId, cardId).ConfigureAwait(false);
		return card?.ToDto();
	}

	/// <inheritdoc />
	public async Task<IEnumerable<CardDto>> GetCardsAsync(Guid deckId, int page, int pageSize)
	{
		var cards = await _deckRepository.GetCardsAsync(deckId, page, pageSize).ConfigureAwait(false);
		return cards.ToDtoList();
	}

	/// <inheritdoc />
	public async Task<int> GetCardCountAsync(Guid deckId)
	{
		return await _deckRepository.GetCardCountAsync(deckId).ConfigureAwait(false);
	}

	/// <inheritdoc />
	public async Task<IEnumerable<CardDto>> GetCardsToReview(Guid deckId, DateTime until, int pageSize, DateTime? cursor)
	{
		if (pageSize < 1)
		{
			throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");
		}

		var cards = await _deckRepository.GetCardsToReview(deckId, until, pageSize, cursor).ConfigureAwait(false);
		return cards.ToDtoList();
	}

	/// <inheritdoc />
	public async Task ReviewCardAsync(Guid deckId, Guid cardId, bool isCorrect)
	{
		var card = await _deckRepository.GetCardByIdAsync(deckId, cardId).ConfigureAwait(false);
		if (card is null)
		{
			throw new EntityNotFoundException("Card", cardId);
		}

		_reviewCardService.UpdateReview(card, isCorrect);
		_deckRepository.UpdateCard(card);
		await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
	}
}
