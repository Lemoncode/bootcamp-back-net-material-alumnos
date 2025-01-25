﻿using FluentAssertions;

using Repetify.Application.Dtos;
using Repetify.Application.Extensions.Mappings;
using Repetify.Domain.Entities;

namespace Repetify.Application.Tests.Extensions.Mappings;

public class CardExtensionsTests
{
	[Fact]
	public void ToDto_Should_Map_Card_To_CardDto()
	{
		var card = new Card(Guid.NewGuid(), Guid.NewGuid(), "Hola", "Hello", 3, DateTime.UtcNow, DateTime.UtcNow.AddDays(-1));

		var result = card.ToDto();

		result.Should().BeEquivalentTo(new CardDto(
			card.Id,
			card.DeckId,
			card.OriginalWord,
			card.TranslatedWord,
			card.CorrectReviewStreak,
			card.NextReviewDate,
			card.PreviousCorrectReview
		));
	}

	[Fact]
	public void ToDto_Should_Throw_ArgumentNullException_When_Card_Is_Null()
	{
		var card = (Card)null!;

		Assert.Throws<ArgumentNullException>(() => card.ToDto());
	}

	[Fact]
	public void ToEntity_Should_Map_CardDto_To_Card()
	{
		var cardDto = new CardDto(Guid.NewGuid(), Guid.NewGuid(), "Hola", "Hello", 3, DateTime.UtcNow, DateTime.UtcNow.AddDays(-1));

		var result = cardDto.ToEntity();

		result.Should().BeEquivalentTo(new Card(
			cardDto.Id,
			cardDto.DeckId,
			cardDto.OriginalWord,
			cardDto.TranslatedWord,
			cardDto.CorrectReviewStreak,
			cardDto.NextReviewDate,
			cardDto.PreviousCorrectReview
		));
	}

	[Fact]
	public void ToEntity_Should_Throw_ArgumentNullException_When_CardDto_Is_Null()
	{
		var card = (CardDto)null!;
		Assert.Throws<ArgumentNullException>(() => card.ToEntity());
	}

	[Fact]
	public void ToDtoList_Should_Map_List_Of_Cards_To_List_Of_CardDtos()
	{
		var cards = new List<Card>
		{
			new Card(Guid.NewGuid(), Guid.NewGuid(), "Hola", "Hello", 3, DateTime.UtcNow, DateTime.UtcNow.AddDays(-1)),
			new Card(Guid.NewGuid(), Guid.NewGuid(), "Adiós", "Goodbye", 2, DateTime.UtcNow, DateTime.UtcNow.AddDays(-2))
		};

		var result = cards.ToDtoList();

		result.Should().BeEquivalentTo(cards.Select(c => c.ToDto()));
	}

	[Fact]
	public void ToEntityList_Should_Map_List_Of_CardDtos_To_List_Of_Cards()
	{
		var cardDtos = new List<CardDto>
		{
			new CardDto(Guid.NewGuid(), Guid.NewGuid(), "Hola", "Hello", 3, DateTime.UtcNow, DateTime.UtcNow.AddDays(-1)),
			new CardDto(Guid.NewGuid(), Guid.NewGuid(), "Adiós", "Goodbye", 2, DateTime.UtcNow, DateTime.UtcNow.AddDays(-2))
		};

		var result = cardDtos.ToEntityList();

		result.Should().BeEquivalentTo(cardDtos.Select(dto => dto.ToEntity()));
	}
}