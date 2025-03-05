namespace Repetify.Domain.Entities;

/// <summary>
/// Represents a flashcard with an original word and its translation.
/// </summary>
public class Card
{
	/// <summary>
	/// Gets the unique identifier for the card.
	/// </summary>
	public Guid Id { get; private set; }

	/// <summary>
	///  Gets or sets the unique identifier for the deck.
	/// </summary>
	public Guid DeckId { get; private set; }

	/// <summary>
	/// Gets the original word on the card.
	/// </summary>
	public string OriginalWord { get; private set; }

	/// <summary>
	/// Gets the translated word on the card.
	/// </summary>
	public string TranslatedWord { get; private set; }

	/// <summary>
	/// Gets the number of consecutive correct reviews.
	/// </summary>
	public int CorrectReviewStreak { get; private set; }

	/// <summary>
	/// Gets the date when the card should be reviewed next.
	/// </summary>
	public DateTime NextReviewDate { get; private set; }

	/// <summary>
	/// Gets the date of the previous correct review.
	/// </summary>
	public DateTime PreviousCorrectReview { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="Card"/> class with specified original and translated words.
	/// </summary>
	/// <param name="deckId">The unique identifier for the deck.</param>
	/// <param name="originalWord">The original word on the card.</param>
	/// <param name="translatedWord">The translated word on the card.</param>
	public Card(Guid deckId, string originalWord, string translatedWord)
		: this(deckId,
			  originalWord,
			  translatedWord,
			  correctReviewStreak: 0,
			  nextReviewDate: DateTime.UtcNow.AddDays(1),
			  previousCorrectReview: DateTime.MinValue)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Card"/> class with specified parameters.
	/// </summary>
	/// <param name="deckId">The unique identifier for the deck.</param>
	/// <param name="originalWord">The original word on the card.</param>
	/// <param name="translatedWord">The translated word on the card.</param>
	/// <param name="correctReviewStreak">The number of consecutive correct reviews.</param>
	/// <param name="nextReviewDate">The date when the card should be reviewed next.</param>
	/// <param name="previousCorrectReview">The date of the previous correct review.</param>
	public Card(Guid deckId, string originalWord, string translatedWord, int correctReviewStreak,
						DateTime nextReviewDate, DateTime previousCorrectReview)
		: this(Guid.NewGuid(),
			  deckId,
			  originalWord,
			  translatedWord,
			  correctReviewStreak,
			  nextReviewDate,
			  previousCorrectReview)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Card"/> class with specified parameters.
	/// </summary>
	/// <param name="id">The unique identifier for the card.</param>
	/// <param name="deckId">The unique identifier for the deck.</param>
	/// <param name="originalWord">The original word on the card.</param>
	/// <param name="translatedWord">The translated word on the card.</param>
	/// <param name="correctReviewStreak">The number of consecutive correct reviews.</param>
	/// <param name="nextReviewDate">The date when the card should be reviewed next.</param>
	/// <param name="previousCorrectReview">The date of the previous correct review.</param>
	public Card(Guid id, Guid deckId, string originalWord, string translatedWord, int correctReviewStreak,
				DateTime nextReviewDate, DateTime previousCorrectReview)
	{
		ArgumentNullException.ThrowIfNullOrWhiteSpace(originalWord);
		ArgumentNullException.ThrowIfNullOrWhiteSpace(translatedWord);

		Id = id;
		DeckId = deckId;
		OriginalWord = originalWord;
		TranslatedWord = translatedWord;
		if (correctReviewStreak < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(correctReviewStreak), "The number of hits must be a number greater than or equal to zero.");
		}

		CorrectReviewStreak = correctReviewStreak;
		NextReviewDate = nextReviewDate;
		PreviousCorrectReview = previousCorrectReview;
	}

	/// <summary>
	/// Sets the date when the card should be reviewed next.
	/// </summary>
	/// <param name="nextReview">The date for the next review.</param>
	public void SetNextReviewDate(DateTime nextReview)
	{
		NextReviewDate = nextReview;
	}

	/// <summary>
	/// Sets the date of the previous correct review.
	/// </summary>
	/// <param name="previousCorrectReview">The date of the previous correct review.</param>
	public void SetPreviousCorrectReview(DateTime previousCorrectReview)
	{
		PreviousCorrectReview = previousCorrectReview;
	}

	/// <summary>
	/// Sets the number of consecutive correct reviews.
	/// </summary>
	/// <param name="streak">The number of consecutive correct reviews.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the streak is less than 0.</exception>
	public void SetCorrectReviewStreak(int streak)
	{
		if (streak < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(streak), "Streak must be greater than or equal to 0.");
		}

		CorrectReviewStreak = streak;
	}
}
