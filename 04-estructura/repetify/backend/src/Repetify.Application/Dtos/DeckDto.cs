namespace Repetify.Application.Dtos;

public class DeckDto
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	
	public string? Description { get; set; }

	public Guid UserId { get; set; }
	
	public string OriginalLanguage { get; set; }

	public string TranslatedLanguage { get; set; }

	public DeckDto(Guid id, string name, string? description, Guid userId, string originalLanguage, string translatedLanguage)
	{
		ArgumentException.ThrowIfNullOrEmpty(name);
		ArgumentException.ThrowIfNullOrEmpty(originalLanguage);
		ArgumentException.ThrowIfNullOrWhiteSpace(TranslatedLanguage);

		Id = id;
		Name = name;
		Description = description;
		UserId = userId;
		OriginalLanguage = originalLanguage;
		TranslatedLanguage = translatedLanguage;
	}
}
