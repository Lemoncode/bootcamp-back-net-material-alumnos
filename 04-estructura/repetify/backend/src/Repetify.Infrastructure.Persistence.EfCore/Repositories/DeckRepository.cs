using Microsoft.EntityFrameworkCore;

using Repetify.Domain.Abstractions.Repositories;
using Repetify.Infrastructure.Persistence.EfCore.Context;
using Repetify.Infrastructure.Persistence.EfCore.Extensions.Mappers;
using Repetify.Domain.Entities;

namespace Repetify.Infrastructure.Persistence.EfCore.Repositories;

/// <summary>
/// Implementation of the deck repository using EF Core.
/// </summary>
public class DeckRepository(RepetifyDbContext dbContext) : IDeckRepository
{
	private const string InMemoryDBProviderName = "Microsoft.EntityFrameworkCore.InMemory";

	private readonly RepetifyDbContext _dbContext = dbContext;

	/// <inheritdoc />
	public async Task AddDeckAsync(Deck deck)
	{
		await _dbContext.Decks.AddAsync(deck.ToEntity()).ConfigureAwait(false);
	}

	/// <inheritdoc />
	public void UpdateDeck(Deck deck)
	{
		_dbContext.Decks.Update(deck.ToEntity());
	}

	/// <inheritdoc />
	public async Task<bool> DeleteDeckAsync(Guid deckId)
	{
		if (!await _dbContext.Decks.AnyAsync(d => d.Id == deckId).ConfigureAwait(false))
		{
			return false;
		}

		if (IsInMemoryDb())
		{
			var deck = await _dbContext.Decks
				.FirstOrDefaultAsync(d => d.Id == deckId).ConfigureAwait(false);
			_dbContext.Decks.Remove(deck!);
		}
		else
		{
			await _dbContext.Decks.Where(d => d.Id == deckId).ExecuteDeleteAsync().ConfigureAwait(false);
		}

		return true;
	}

	/// <inheritdoc />
	public async Task<Deck?> GetDeckByIdAsync(Guid deckId)
	{
		return
			(await _dbContext.Decks
				.AsNoTracking()
				.FirstOrDefaultAsync(d => d.Id == deckId).ConfigureAwait(false))?.ToDomain();
	}

	/// <inheritdoc />
	public async Task<IEnumerable<Deck>> GetDecksByUserIdAsync(Guid userId)
	{
		return await _dbContext.Decks
			.Where(d => d.UserId == userId)
			.AsNoTracking()
			.Select(d => d.ToDomain())
			.ToListAsync().ConfigureAwait(false);
	}

	///  <inheritdoc/>
	public Task<bool> DeckNameExistsForUser(string name, Guid userId)
	{
		ArgumentNullException.ThrowIfNull(name);

		return _dbContext.Decks.AnyAsync(d => d.Name == name && d.UserId == userId);
	}

	/// <inheritdoc />
	public async Task AddCardAsync(Card card)
	{
		ArgumentNullException.ThrowIfNull(card);

		var cardEntity = CardExtensions.ToEntity(card);
		await _dbContext.Cards.AddAsync(cardEntity).ConfigureAwait(false);
	}

	/// <inheritdoc />
	public void UpdateCard(Card card)
	{
		var cardEntity = CardExtensions.ToEntity(card);
		_dbContext.Cards.Update(cardEntity);
	}

	/// <inheritdoc />
	public async Task<bool> DeleteCardAsync(Guid deckId, Guid cardId)
	{
		if (!await _dbContext.Cards.AnyAsync(c => c.DeckId == deckId && c.Id == cardId).ConfigureAwait(false))
		{
			return false;
		}

		if (IsInMemoryDb())
		{
			var card = await _dbContext.Cards
				.FirstOrDefaultAsync(c => c.DeckId == deckId && c.Id == cardId).ConfigureAwait(false);
			_dbContext.Cards.Remove(card!);
		}
		else
		{
			await _dbContext.Cards
				.Where(c => c.DeckId == deckId && c.Id == cardId)
				.ExecuteDeleteAsync().ConfigureAwait(false);
		}

		return true;
	}

	///  <inheritdoc/>
	public async Task<IEnumerable<Card>> GetCardsAsync(Guid deckId, int page, int pageSize)
	{
		return await _dbContext.Cards
			.Where(c => c.DeckId == deckId)
			.OrderBy(c => c.NextReviewDate)
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.AsNoTracking()
			.Select(c => c.ToDomain()!)
			.ToListAsync().ConfigureAwait(false);
	}

	/// <inheritdoc />
	public Task<int> GetCardCountAsync(Guid deckId) => _dbContext.Cards
			.CountAsync(c => c.DeckId == deckId);

	/// <inheritdoc />
	public async Task<Card?> GetCardByIdAsync(Guid deckId, Guid cardId)
	{
		return (await _dbContext.Cards
			.AsNoTracking()
			.FirstOrDefaultAsync(c => c.DeckId == deckId && c.Id == cardId).ConfigureAwait(false))?.ToDomain();
	}

	/// <inheritdoc />
	public async Task<IEnumerable<Card>> GetCardsToReview(Guid deckId, DateTime until, int pageSize, DateTime? cursor)
	{
		var result = _dbContext.Cards
			.Where(c => c.DeckId == deckId && c.NextReviewDate <= until);

		if (cursor.HasValue)
		{
			result = result.Where(c => c.NextReviewDate > cursor);
		}

			return await result.OrderBy(c => c.NextReviewDate)
			.Take(pageSize)
			.AsNoTracking()
			.Select(d => d.ToDomain())
			.ToListAsync().ConfigureAwait(false);
	}

	/// <inheritdoc />
	public Task SaveChangesAsync() =>
	_dbContext.SaveChangesAsync();

	private bool IsInMemoryDb()
	{
		return _dbContext.Database.ProviderName?.Equals(InMemoryDBProviderName, StringComparison.Ordinal) ?? false;
	}
}
