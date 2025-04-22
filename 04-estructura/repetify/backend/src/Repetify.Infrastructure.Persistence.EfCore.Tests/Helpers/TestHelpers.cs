using Microsoft.EntityFrameworkCore;
using Repetify.Infrastructure.Persistence.EfCore.Context;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetify.Infrastructure.Persistence.EfCore.Tests.Helpers;
internal static class TestHelpers
{
	internal static RepetifyDbContext CreateInMemoryDbContext()
	{
		var options = new DbContextOptionsBuilder<RepetifyDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;
		return new(options);
	}

}
