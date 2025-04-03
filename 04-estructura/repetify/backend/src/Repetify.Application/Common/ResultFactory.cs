using Repetify.Application.Enums;

namespace Repetify.Application.Common;

/// <summary>
/// Factory class for creating <see cref="Result"/> and <see cref="Result{T}"/> instances.
/// </summary>
internal static class ResultFactory
{
	/// <summary>
	/// Creates a successful <see cref="Result{T}"/> with the specified value.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="value">The value of the result.</param>
	/// <returns>A successful <see cref="Result{T}"/>.</returns>
	public static Result<T> Success<T>(T value) => new(value);

	/// <summary>
	/// Creates a successful <see cref="Result"/>.
	/// </summary>
	/// <returns>A successful <see cref="Result"/>.</returns>
	public static Result Success() => new();

	/// <summary>
	/// Creates an <see cref="Result"/> with a not found status and an optional error message.
	/// </summary>
	/// <param name="errorMessage">The optional error message.</param>
	/// <returns>An <see cref="Result"/> with a not found status.</returns>
	public static Result NotFound(string? errorMessage = null) => new(ResultStatus.NotFound, errorMessage);

	/// <summary>
	/// Creates an <see cref="Result"/> with an invalid argument status and an optional error message.
	/// </summary>
	/// <param name="errorMessage">The optional error message.</param>
	/// <returns>An <see cref="Result"/> with an invalid argument status.</returns>
	public static Result InvalidArgument(string? errorMessage = null) => new(ResultStatus.InvalidArguments, errorMessage);

	/// <summary>
	/// Creates an <see cref="Result{T}"/> with an Conflict status and an error message.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="errorMessage">The error message of the result.</param>
	/// <returns>An <see cref="Result{T}"/> with an Conflict status.</returns>
	public static Result Conflict(string? errorMessage) => new(ResultStatus.Conflict, errorMessage);

	/// <summary>
	/// Creates an <see cref="Result{T}"/> with a not found status and an optional error message.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="errorMessage">The optional error message.</param>
	/// <returns>An <see cref="Result{T}"/> with a not found status.</returns>
	public static Result<T> NotFound<T>(string? errorMessage = null) => new Result<T>(ResultStatus.NotFound, errorMessage, default(T));

	/// <summary>
	/// Creates an <see cref="Result{T}"/> with an invalid argument status and an error message.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="errorMessage">The error message of the result.</param>
	/// <returns>An <see cref="Result{T}"/> with an invalid argument status.</returns>
	public static Result<T> InvalidArgument<T>(string? errorMessage) => new(ResultStatus.InvalidArguments, errorMessage);

	/// <summary>
	/// Creates an <see cref="Result{T}"/> with an Conflict status and an error message.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="errorMessage">The error message of the result.</param>
	/// <returns>An <see cref="Result{T}"/> with an Conflict status.</returns>
	public static Result<T> Conflict<T>(string? errorMessage) => new(ResultStatus.Conflict, errorMessage);
}
