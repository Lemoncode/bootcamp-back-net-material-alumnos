using Repetify.Application.Enums;

namespace Repetify.Application.Common;

/// <summary>
/// Represents a result of an operation, containing a value and a status.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class Result<T>
{
	/// <summary>
	/// Gets the value of the result.
	/// </summary>
	public T? Value { get; }

	/// <summary>
	/// Gets the status of the result.
	/// </summary>
	public ResultStatus Status { get; }

	/// <summary>
	///  Gets the error message associated to the result
	/// </summary>
	public string? ErrorMessage { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="Result{T}"/> class.
	/// </summary>
	/// <param name="value">The value of the result.</param>
	public Result(T value)
	{
		Value = value;
		Status = ResultStatus.Success;
	}

	/// <summary>
	///  Initializes a new instance of the <see cref="Result{T}"/> class
	/// </summary>
	/// <param name="status">The operation status</param>
	/// <param name="errorMessage">The error message</param>
	public Result(ResultStatus status, string? errorMessage = null, T? value = default(T))
	{
		Status = status;
		ErrorMessage = errorMessage;
		Value = value;
	}
}