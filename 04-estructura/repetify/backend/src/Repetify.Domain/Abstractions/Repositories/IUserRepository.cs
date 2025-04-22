using Repetify.Domain.Entities;

namespace Repetify.Domain.Abstractions.Repositories;

/// <summary>
/// Represents a repository for managing user-related data operations.
/// </summary>
public interface IUserRepository
{
	/// <summary>
	/// Retrieves a user by their email address.
	/// </summary>
	/// <param name="email">The email address of the user to retrieve.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="User"/> object associated with the specified email.</returns>
	public Task<User?> GetUserByEmailAsync(string email);

	/// <summary>
	/// Adds a new user to the repository.
	/// </summary>
	/// <param name="user">The <see cref="User"/> object to add.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public Task AddUserAsync(User user);

	/// <summary>
	/// Updates an existing user's information in the repository.
	/// </summary>
	/// <param name="user">The <see cref="User"/> object with updated information.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public Task EditUserAsync(User user);

	/// <summary>
	/// Deletes a user from the repository by their unique identifier.
	/// </summary>
	/// <param name="userId">The unique identifier of the user to delete.</param>
	/// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
	public Task<bool> DeleteUserAsync(Guid userId);

	/// <summary>  
	/// Saves all changes made in the repository to the underlying data store.  
	/// </summary>  
	/// <returns>A task that represents the asynchronous save operation.</returns>  
	Task SaveChangesAsync();
}
