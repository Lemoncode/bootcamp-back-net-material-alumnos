using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetify.Domain.Abstractions;

/// <summary>
/// Interface for providing the current UTC date and time.
/// </summary>
public interface IClock
{
	/// <summary>
	/// Gets the current UTC date.
	/// </summary>
	DateTime UtcNow { get; }
}
