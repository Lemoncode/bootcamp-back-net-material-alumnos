using Microsoft.AspNetCore.Mvc;

using Repetify.Application.Abstractions.Services;
using Repetify.Application.Dtos;
using Repetify.Application.Exceptions;

using System.Net;

namespace Repetify.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
internal class DeckController : ControllerBase
{
	private readonly IDeckAppService _deckAppService;
	private readonly Guid _userId = Guid.Parse("f5b1b3b3-3b3b-4b3b-8b3b-3b3b3b3b3b3b");

	public DeckController(IDeckAppService deckAppService)
	{
		_deckAppService = deckAppService;
	}

	[HttpPost]
	public async Task<IActionResult> AddDeck([FromBody] DeckDto deck)
	{
		var deckId = await _deckAppService.AddDeckAsync(deck).ConfigureAwait(false);
		deck.Id = deckId;
		return CreatedAtAction(
			nameof(GetDeckById),
		new { id = deckId },
		deck);
	}

	[HttpPut]
	public async Task<IActionResult> UpdateDeck([FromBody] DeckDto deck)
	{
		await _deckAppService.UpdateDeckAsync(deck).ConfigureAwait(false);
		return NoContent();
	}

	[HttpDelete("{deckId}")]
	public async Task<IActionResult> DeleteDeck(Guid deckId)
	{
		var result = await _deckAppService.DeleteDeckAsync(deckId).ConfigureAwait(false);
		if (!result)
		{
			return NotFound();
		}
		return NoContent();
	}

	[HttpGet("{deckId}")]
	public async Task<IActionResult> GetDeckById(Guid deckId)
	{
		var deck = await _deckAppService.GetDeckByIdAsync(deckId).ConfigureAwait(false);
		if (deck == null)
		{
			return NotFound();
		}
		return Ok(deck);
	}

	[HttpGet("user-decks")]
	public async Task<IActionResult> GetUserDecks()
	{
		var decks = await _deckAppService.GetUserDecksAsync(_userId).ConfigureAwait(false);
		return Ok(decks);
	}

	[HttpPost("cards")]
	public async Task<IActionResult> AddCard([FromBody] CardDto card)
	{
		await _deckAppService.AddCardAsync(card).ConfigureAwait(false);
		return StatusCode((int)HttpStatusCode.Created);
	}

	[HttpPut("cards")]
	public async Task<IActionResult> UpdateCard([FromBody] CardDto card)
	{
		await _deckAppService.UpdateCardAsync(card).ConfigureAwait(false);
		return NoContent();
	}

	[HttpDelete("cards/{deckId}/{cardId}")]
	public async Task<IActionResult> DeleteCard(Guid deckId, Guid cardId)
	{
		var result = await _deckAppService.DeleteCardAsync(deckId, cardId).ConfigureAwait(false);
		if (!result)
		{
			return NotFound();
		}
		return NoContent();
	}

	[HttpGet("cards/{deckId}/{cardId}")]
	public async Task<IActionResult> GetCardById(Guid deckId, Guid cardId)
	{
		var card = await _deckAppService.GetCardByIdAsync(deckId, cardId).ConfigureAwait(false);
		if (card == null)
		{
			return NotFound();
		}
		return Ok(card);
	}

	[HttpGet("cards/{deckId}")]
	public async Task<IActionResult> GetCards(Guid deckId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
	{
		var cards = await _deckAppService.GetCardsAsync(deckId, page, pageSize).ConfigureAwait(false);
		return Ok(cards);
	}

	[HttpGet("cards/count/{deckId}")]
	public async Task<IActionResult> GetCardCount(Guid deckId)
	{
		var count = await _deckAppService.GetCardCountAsync(deckId).ConfigureAwait(false);
		return Ok(count);
	}

	[HttpGet("cards/review/{deckId}")]
	public async Task<IActionResult> GetCardsToReview(Guid deckId, [FromQuery] DateTime until, [FromQuery] int pageSize, [FromQuery] DateTime? cursor = null)
	{
		if (pageSize < 1)
		{
			return BadRequest("Page size must be greater than 0.");
		}

		var cards = await _deckAppService.GetCardsToReview(deckId, until, pageSize, cursor).ConfigureAwait(false);
		return Ok(cards);
	}

	[HttpPost("cards/review/{deckId}/{cardId}")]
	public async Task<IActionResult> ReviewCard(Guid deckId, Guid cardId, [FromQuery] bool isCorrect)
	{
		try
		{
			await _deckAppService.ReviewCardAsync(deckId, cardId, isCorrect).ConfigureAwait(false);
			return NoContent();
		}
		catch (EntityNotFoundException)
		{
			return NotFound();
		}
	}
}
