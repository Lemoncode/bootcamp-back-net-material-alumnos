using Microsoft.EntityFrameworkCore;

using Repetify.Domain.Abstractions.Repositories;
using Repetify.Domain.Entities;
using Repetify.Infrastructure.Persistence.EfCore.Context;
using Repetify.Infrastructure.Persistence.EfCore.Extensions.Mappers;

namespace Repetify.Infrastructure.Persistence.EfCore.Repositories;

/// <summary>
/// Implementation of the deck repository using EF Core.
/// </summary>
public class DeckRepository(RepetifyDbContext dbContext) : RepositoryBase(dbContext),  IDeckRepository
{

	private readonly RepetifyDbContext _context = dbContext;

	/// <inheritdoc />
	public async Task AddDeckAsync(Deck deck)
	{
		await _context.Decks.AddAsync(deck.ToDataEntity()).ConfigureAwait(false);
	}

	/// <inheritdoc />
	public async Task UpdateDeckAsync(Deck deck)
	{
		var deckEntity = await _context.Decks.FindAsync(deck.Id).ConfigureAwait(false);
		deckEntity!.UpdateFromDomain(deck);
	}

	/// <inheritdoc />
	public async Task<bool> DeleteDeckAsync(Guid deckId)
	{
		if (!await _context.Decks.AnyAsync(d => d.Id == deckId).ConfigureAwait(false))
		{
			return false;
		}

		if (IsInMemoryDb())
		{
			var deck = await _context.Decks
				.FirstOrDefaultAsync(d => d.Id == deckId).ConfigureAwait(false);
			_context.Decks.Remove(deck!);
		}
		else
		{
			await _context.Decks.Where(d => d.Id == deckId).ExecuteDeleteAsync().ConfigureAwait(false);
		}

		return true;
	}

	/// <inheritdoc />
	public async Task<Deck?> GetDeckByIdAsync(Guid deckId)
	{
		return
			(await _context.Decks
				.AsNoTracking()
				.FirstOrDefaultAsync(d => d.Id == deckId).ConfigureAwait(false))?.ToDomain();
	}

	/// <inheritdoc />
	public async Task<IEnumerable<Deck>> GetDecksByUserIdAsync(Guid userId)
	{
		return await _context.Decks
			.Where(d => d.UserId == userId)
			.AsNoTracking()
			.Select(d => d.ToDomain())
			.ToListAsync().ConfigureAwait(false);
	}

	///  <inheritdoc/>
	public Task<bool> DeckNameExistsForUserAsync(Guid deckId, string name, Guid userId)
	{
		ArgumentNullException.ThrowIfNull(name);

		return _context.Decks.AnyAsync(d => d.Name == name && d.UserId == userId && d.Id != deckId);
	}

	/// <inheritdoc />
	public async Task AddCardAsync(Card card)
	{
		ArgumentNullException.ThrowIfNull(card);

		var cardEntity = CardExtensions.ToDataEntity(card);
		await _context.Cards.AddAsync(cardEntity).ConfigureAwait(false);
	}

	/// <inheritdoc />
	public async Task UpdateCardAsync(Card card)
	{
		var cardEntity = await _context.Cards.FindAsync(card.Id).ConfigureAwait(false);
		cardEntity!.UpdateFromDomain(card);
	}

	/// <inheritdoc />
	public async Task<bool> DeleteCardAsync(Guid deckId, Guid cardId)
	{
		if (!await _context.Cards.AnyAsync(c => c.DeckId == deckId && c.Id == cardId).ConfigureAwait(false))
		{
			return false;
		}

		if (IsInMemoryDb())
		{
			var card = await _context.Cards
				.FirstOrDefaultAsync(c => c.DeckId == deckId && c.Id == cardId).ConfigureAwait(false);
			_context.Cards.Remove(card!);
		}
		else
		{
			await _context.Cards
				.Where(c => c.DeckId == deckId && c.Id == cardId)
				.ExecuteDeleteAsync().ConfigureAwait(false);
		}

		return true;
	}

	///  <inheritdoc/>
	public async Task<IEnumerable<Card>> GetCardsAsync(Guid deckId, int page, int pageSize)
	{
		return await _context.Cards
			.Where(c => c.DeckId == deckId)
			.OrderBy(c => c.NextReviewDate)
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.AsNoTracking()
			.Select(c => c.ToDomain()!)
			.ToListAsync().ConfigureAwait(false);
	}

	/// <inheritdoc />
	public Task<int> GetCardCountAsync(Guid deckId) => _context.Cards
			.CountAsync(c => c.DeckId == deckId);

	/// <inheritdoc />
	public async Task<Card?> GetCardByIdAsync(Guid deckId, Guid cardId)
	{
		return (await _context.Cards
			.AsNoTracking()
			.FirstOrDefaultAsync(c => c.DeckId == deckId && c.Id == cardId).ConfigureAwait(false))?.ToDomain();
	}

	/// <inheritdoc />
	public async Task<IEnumerable<Card>> GetCardsToReview(Guid deckId, DateTime until, int pageSize, DateTime? cursor)
	{
		var result = _context.Cards
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
	_context.SaveChangesAsync();

}
