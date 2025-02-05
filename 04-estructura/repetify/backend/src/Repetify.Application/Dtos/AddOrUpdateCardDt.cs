using System.ComponentModel.DataAnnotations;

namespace Repetify.Application.Dtos;

/// <summary>
/// Data Transfer Object for add or update a Card.
/// </summary>
public class AddOrUpdateCardDto
{
	/// <summary>
	/// Gets or sets the unique identifier for the deck.
	/// </summary>
	[Required(ErrorMessage = "The deck ID is required.")]
	public Guid DeckId { get; set; }

	/// <summary>
	/// Gets or sets the original word.
	/// </summary>
	[Required(ErrorMessage = "The original word is required.")]
	[StringLength(500, ErrorMessage = "The original word cannot exceed 500 characters.")]
	public string OriginalWord { get; set; }

	/// <summary>
	/// Gets or sets the translated word.
	/// </summary>
	[Required(ErrorMessage = "The translated word is required.")]
	[StringLength(500, ErrorMessage = "The translated word cannot exceed 500 characters.")]
	public string TranslatedWord { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="AddOrUpdateCardDto"/> class.
	/// </summary>
	/// <param name="deckId">The unique identifier for the deck.</param>
	/// <param name="originalWord">The original word.</param>
	/// <param name="translatedWord">The translated word.</param>
	/// <exception cref="ArgumentException">Thrown when originalWord or translatedWord is null or whitespace.</exception>
	public AddOrUpdateCardDto(Guid deckId, string originalWord, string translatedWord)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(originalWord);
		ArgumentException.ThrowIfNullOrWhiteSpace(translatedWord);

		DeckId = deckId;
		OriginalWord = originalWord;
		TranslatedWord = translatedWord;
	}
}
