namespace Repetify.Application.Exceptions;

/// <summary>
/// Exception thrown when an entity is not found in the application.
/// </summary>
public class EntityNotFoundException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
	/// </summary>
	public EntityNotFoundException()
		: base()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message.
	/// </summary>
	/// <param name="entityType">The type of the entity.</param>
	/// <param name="entityId">The ID of the entity.</param>
	public EntityNotFoundException(string entityType, Guid entityId)
		: base($"Entity of type {entityType} with ID {entityId} was not found.")
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The error message.</param>
	public EntityNotFoundException(string message)
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
	/// </summary>
	/// <param name="message">The error message.</param>
	/// <param name="innerException">The inner exception reference.</param>
	public EntityNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
