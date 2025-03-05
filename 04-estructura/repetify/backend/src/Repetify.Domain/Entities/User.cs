namespace Repetify.Domain.Entities;

public class User
{
	public Guid Id { get; private set; }
	public string Username { get; private set; }
	public string Email { get; private set; }

	public User(Guid id, string username, string email)
	{
		Id = id;
		Username = username ?? throw new ArgumentNullException(nameof(username));
		Email = email ?? throw new ArgumentNullException(nameof(email));
	}
}
