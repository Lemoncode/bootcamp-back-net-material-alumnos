using FluentAssertions;

using Moq;

using Repetify.Application.Dtos;
using Repetify.Application.Enums;
using Repetify.Application.Services;
using Repetify.Domain.Abstractions.Repositories;
using Repetify.Domain.Abstractions.Services;
using Repetify.Domain.Entities;

namespace Repetify.Application.Tests.Services;

public class DeckAppServiceTests
{
	private readonly Mock<IDeckValidator> _deckValidatorMock;
	private readonly Mock<IDeckRepository> _deckRepositoryMock;
	private readonly Mock<ICardReviewService> _reviewCardServiceMock;
	private readonly DeckAppService _deckAppService;

	public DeckAppServiceTests()
	{
		_deckValidatorMock = new Mock<IDeckValidator>();
		_reviewCardServiceMock = new Mock<ICardReviewService>();
		_deckValidatorMock.Setup(m => m.EnsureIsValid(It.IsAny<Deck>())).Returns(Task.CompletedTask);
		_deckRepositoryMock = new Mock<IDeckRepository>();
		_deckAppService = new DeckAppService(_deckValidatorMock.Object, _reviewCardServiceMock.Object, _deckRepositoryMock.Object);
	}

	[Fact]
	public async Task AddDeckAsync_Should_Call_Repository_And_SaveChanges()
	{
		var deckDto = CreateFakeAddOrUpdateDeck();

		await _deckAppService.AddDeckAsync(deckDto, Guid.NewGuid());

		_deckRepositoryMock.Verify(r => r.AddDeckAsync(It.IsAny<Deck>()), Times.Once);
		_deckRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
	}

	[Fact]
	public async Task UpdateDeckAsync_Should_Call_Repository_And_SaveChanges()
	{
		var deckDto = CreateFakeAddOrUpdateDeck();

		await _deckAppService.UpdateDeckAsync(deckDto, Guid.NewGuid());

		_deckRepositoryMock.Verify(r => r.UpdateDeck(It.IsAny<Deck>()), Times.Once);
		_deckRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
	}

	[Fact]
	public async Task DeleteDeckAsync_Should_Call_Repository_And_SaveChanges_When_Deck_Exists()
	{
		var deckId = Guid.NewGuid();
		_deckRepositoryMock.Setup(r => r.DeleteDeckAsync(deckId)).ReturnsAsync(true);

		var result = await _deckAppService.DeleteDeckAsync(deckId);

		result.Value.Should()
			.BeTrue();
		_deckRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
	}

	[Fact]
	public async Task DeleteDeckAsync_Should_Return_False_When_Deck_Does_Not_Exist()
	{
		var deckId = Guid.NewGuid();
		_deckRepositoryMock.Setup(r => r.DeleteDeckAsync(deckId)).ReturnsAsync(false);

		var result = await _deckAppService.DeleteDeckAsync(deckId);

		result.Value.Should().BeFalse();
		_deckRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
	}

	[Fact]
	public async Task GetDeckByIdAsync_Should_Return_DeckDto_When_Deck_Exists()
	{
		var deckId = Guid.NewGuid();
		var deck = new Deck(deckId, "Deck", "description", Guid.NewGuid(), "english", "spanish");
		_deckRepositoryMock.Setup(r => r.GetDeckByIdAsync(deckId)).ReturnsAsync(deck);

		var result = await _deckAppService.GetDeckByIdAsync(deckId);

		result.Should().NotBeNull();
		result.Value!.Id.Should().Be(deckId);
	}

	[Fact]
	public async Task GetDeckByIdAsync_Should_Return_Null_When_Deck_Does_Not_Exist()
	{
		_deckRepositoryMock.Setup(r => r.GetDeckByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Deck?)null);

		var result = await _deckAppService.GetDeckByIdAsync(Guid.NewGuid());

		result.Value.Should().BeNull();
	}

	[Fact]
	public async Task AddCardAsync_Should_Call_Repository_And_SaveChanges()
	{
		var cardDto = new AddOrUpdateCardDto("Hola", "Hello");

		await _deckAppService.AddCardAsync(cardDto, Guid.NewGuid());

		_deckRepositoryMock.Verify(r => r.AddCardAsync(It.IsAny<Card>()), Times.Once);
		_deckRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
	}

	[Fact]
	public async Task ReviewCardAsync_Should_Update_Card_And_SaveChanges_When_Card_Exists()
	{
		var deckId = Guid.NewGuid();
		var card = new Card(deckId, "Hola", "Hello", 3, DateTime.UtcNow, DateTime.UtcNow);
		_deckRepositoryMock.Setup(r => r.GetCardByIdAsync(deckId, card.Id)).ReturnsAsync(card);

		await _deckAppService.ReviewCardAsync(deckId, card.Id, true);

		_reviewCardServiceMock.Verify(r => r.UpdateReview(card, true), Times.Once);
		_deckRepositoryMock.Verify(r => r.UpdateCard(card), Times.Once);
		_deckRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
	}

	[Fact]
	public async Task ReviewCardAsync_Returns_NotFoundStatusResult_When_Card_Not_Found()
	{
		var deckId = Guid.NewGuid();
		var cardId = Guid.NewGuid();

		_deckRepositoryMock.Setup(r => r.GetCardByIdAsync(deckId, cardId)).ReturnsAsync((Card?)null);

		var result = await _deckAppService.ReviewCardAsync(deckId, cardId, true);

		result.Status.Should().Be(ResultStatus.NotFound);
	}

	[Fact]
	public async Task GetCardsToReview_Should_Return_Correct_Cards()
	{
		var deckId = Guid.NewGuid();
		var untilDate = DateTime.UtcNow;
		var pageSize = 10;
		var cards = new List<Card>
		{
			new Card(deckId, "Hola", "Hello", 1, DateTime.UtcNow, DateTime.UtcNow)
		};

		_deckRepositoryMock.Setup(r => r.GetCardsToReview(deckId, untilDate, pageSize, null)).ReturnsAsync(cards);

		var result = await _deckAppService.GetCardsToReview(deckId, untilDate, pageSize, null);

		result.Value.Should().HaveCount(1);
		result.Value.First().OriginalWord.Should().Be("Hola");
	}

	[Fact]
	public void GetCardsToReview_Should_Throw_ArgumentOutOfRangeException_When_PageSize_Is_Less_Than_One()
	{
		var deckId = Guid.NewGuid();
		var untilDate = DateTime.UtcNow;
		var pageSize = 0;
		Func<Task> act = async () => await _deckAppService.GetCardsToReview(deckId, untilDate, pageSize, null).ConfigureAwait(false);
		act.Should().ThrowAsync<ArgumentOutOfRangeException>()
			.WithMessage("*pageSize*");
	}


	[Fact]
	public async Task GetCardCountAsync_Should_Return_Correct_Count()
	{
		var deckId = Guid.NewGuid();
		_deckRepositoryMock.Setup(r => r.GetCardCountAsync(deckId)).ReturnsAsync(5);

		var result = await _deckAppService.GetCardCountAsync(deckId);

		result.Value.Should().Be(5);
	}

	private static DeckDto CreateFakeDeck()
	{
		return new DeckDto(Guid.NewGuid(), "Test Deck", "Description", Guid.NewGuid(), "english", "spanish");
	}

	private static AddOrUpdateDeckDto CreateFakeAddOrUpdateDeck()
	{
		return new AddOrUpdateDeckDto("Test Deck", "Description", "english", "spanish");
	}

}
