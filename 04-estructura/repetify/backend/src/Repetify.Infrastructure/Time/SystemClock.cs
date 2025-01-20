using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repetify.Domain.Abstractions;

namespace Repetify.Infrastructure.Time;

public class SystemClock : IClock
{
	/// <Inheritdoc/>
	public DateTime UtcNow => DateTime.UtcNow;
}
