﻿using Repetify.Domain.Abstractions;

namespace Repetify.Infrastructure.Time;

public class SystemClock : IClock
{
	/// <Inheritdoc/>
	public DateTime UtcNow => DateTime.UtcNow;
}
