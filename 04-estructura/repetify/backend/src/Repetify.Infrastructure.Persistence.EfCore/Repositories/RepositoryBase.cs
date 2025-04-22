using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetify.Infrastructure.Persistence.EfCore.Repositories;

public abstract class RepositoryBase(DbContext context)
{
	private const string InMemoryDBProviderName = "Microsoft.EntityFrameworkCore.InMemory";

	protected bool IsInMemoryDb() => context.Database.ProviderName?.Equals(InMemoryDBProviderName, StringComparison.Ordinal) ?? false;
}
