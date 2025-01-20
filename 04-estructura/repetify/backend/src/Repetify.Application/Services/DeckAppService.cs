using Repetify.Application.Abstractions.Repositories;
using Repetify.Application.Abstractions.Services;
using Repetify.Application.Dtos;
using Repetify.Application.Extensions.Mappings;
using Repetify.Domain.Entities;

namespace Repetify.Application.Services;

/// <summary>
/// Service for managing decks and cards.
/// </summary>
public class DeckAppService : IDeckAppService
{
	private readonly IDeckRepository _deckRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="DeckAppService"/> class.
	/// </summary>
	/// <param name="deckRepository">The repository for deck operations.</param>
	public DeckAppService(IDeckRepository deckRepository)
	{
		_deckRepository = deckRepository;
	}

	/// <inheritdoc />
	public async Task<IEnumerable<DeckDto>> GetUserDecksAsync(Guid userId)
	{
		var decks = await _deckRepository.GetDecksByUserIdAsync(userId).ConfigureAwait(false);
		return decks.ToDtoList();
	}

	/// <inheritdoc />
	public async Task<DeckDto?> GetDeckByIdAsync(Guid deckId)
	{
		var deck = await _deckRepository.GetByIdAsync(deckId).ConfigureAwait(false);
		return deck?.ToDto();
	}

	/// <inheritdoc />
	public async Task AddDeckAsync(string name, string? description, Guid userId, string originalLanguage, string translatedLanguage)
	{
		var newDeck = new Deck(Guid.NewGuid(), name, description, userId, originalLanguage, translatedLanguage);
		await _deckRepository.AddAsync(newDeck).ConfigureAwait(false);
		await _deckRepository.SaveChangesAsync().ConfigureAwait(false);
	}

	/// <inheritdoc />
	public async Task<bool> DeleteDeckAsync(Guid deckId)
	{
		var result = await _deckRepository.DeleteAsync(deckId).ConfigureAwait(false);
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
	public async Task AddCardAsync(Guid deckId, string originalWord, string translatedWord)
	{
		var newCard = new Card(originalWord, translatedWord);
		await _deckRepository.AddCardAsync(newCard, deckId).ConfigureAwait(false);
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
}
