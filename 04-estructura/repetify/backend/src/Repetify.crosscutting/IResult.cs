using Repetify.Crosscutting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetify.crosscutting;
public interface IResult
{
	public ResultStatus Status { get; }

	public string? ErrorMessage { get; }
	
	bool IsSuccess { get; }
}
