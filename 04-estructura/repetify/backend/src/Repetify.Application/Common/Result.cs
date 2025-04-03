using Repetify.Application.Enums;

namespace Repetify.Application.Common;

/// <summary>
/// Represents a result of a void operation, containing a status.
/// </summary>
public class Result
{
	/// <summary>
	/// Gets the status of the result.
	/// </summary>
	public ResultStatus Status { get; }

	/// <summary>
	///  Gets the error message of the result.
	/// </summary>
		public string? ErrorMessage { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="Result{T}"/> class.
	/// </summary>
	/// <param name="status">The status of the result.</param>
	/// <param name="errorMessage">The error message of the result</param>
	public Result(ResultStatus status, string? errorMessage)
	{
		Status = status;
		ErrorMessage = errorMessage;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Result"/> class with a default status of Success.
	/// </summary>
	public Result()
	{
		Status = ResultStatus.Success;
	}
}
