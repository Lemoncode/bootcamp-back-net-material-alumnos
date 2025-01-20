using Repetify.Domain.Abstractions;
using Repetify.Domain.Abstractions.Services;
using Repetify.Domain.Entities;

namespace Repetify.Domain.Services;

public class CardReviewService : ICardReviewService
{

	private readonly IClock _clock;

	public CardReviewService(IClock clock)
	{
		_clock = clock;
	}

	/// <inheritdoc/>
	public void UpdateReview(Card card, bool isCorrect)
	{
		ArgumentNullException.ThrowIfNull(card);

		if (isCorrect)
		{
			card.SetCorrectReviewStreak(card.CorrectReviewStreak + 1);
			card.SetPreviousCorrectReview(_clock.UtcNow);

			// Adjust next review date based on streak
			card.SetNextReviewDate(CalculateNextReviewDate(card));
		}
		else
		{
			card.SetCorrectReviewStreak(0);
			card.SetNextReviewDate(_clock.UtcNow.AddDays(1)); // Reset to review tomorrow
		}
	}

	/// <summary>
	/// Calculates the next review date based on the current review streak.
	/// </summary>
	/// <returns>The next review date.</returns>
	private DateTime CalculateNextReviewDate(Card card)
	{
		// Simple algorithm to space reviews based on streak and difficulty
		int interval = card.CorrectReviewStreak;
		return _clock.UtcNow.AddDays(interval);
	}
}
