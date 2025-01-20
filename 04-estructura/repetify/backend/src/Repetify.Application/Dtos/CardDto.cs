namespace Repetify.Application.Dtos;

public class CardDto
{
	public Guid Id { get; set; }
	public string OriginalWord { get; set; }
	public string TranslatedWord { get; set; }
	public int CorrectReviewStreak { get; set; }
	public DateTime NextReviewDate { get; set; }
	public DateTime PreviousCorrectReview { get; set; }

	public CardDto(Guid id, string originalWord, string translatedWord, int correctReviewStreak, DateTime nextReviewDate, DateTime previousCorrectReview)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(originalWord);
		ArgumentException.ThrowIfNullOrWhiteSpace(translatedWord);

		Id = id;
		OriginalWord = originalWord;
		TranslatedWord = translatedWord;
		CorrectReviewStreak = correctReviewStreak;
		NextReviewDate = nextReviewDate;
		PreviousCorrectReview = previousCorrectReview;
	}
}