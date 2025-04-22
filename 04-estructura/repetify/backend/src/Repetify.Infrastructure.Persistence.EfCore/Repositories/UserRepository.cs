﻿using Microsoft.EntityFrameworkCore;

using Repetify.Domain.Abstractions.Repositories;
using Repetify.Domain.Entities;
using Repetify.Infrastructure.Persistence.EfCore.Context;
using Repetify.Infrastructure.Persistence.EfCore.Entities;
using Repetify.Infrastructure.Persistence.EfCore.Extensions.Mappers;

namespace Repetify.Infrastructure.Persistence.EfCore.Repositories;

public class UserRepository(RepetifyDbContext dbContext) : RepositoryBase(dbContext), IUserRepository
{
	private readonly RepetifyDbContext _context = dbContext;

	/// <inheritdoc />  
	public async Task<User?> GetUserByEmailAsync(string email)
	{
		return (await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email).ConfigureAwait(false))?.ToDomain();
	}

	///  <inheritdoc/>
	public async Task<bool> EmailAlreadyExistsAsync(Guid userId, string email)
	{
		ArgumentNullException.ThrowIfNull(email);
		return await _context.Users.AnyAsync(u => u.Id != userId && u.Email.Equals(email)).ConfigureAwait(false);
	}

	///  <inheritdoc/>
	public async Task<bool> UsernameAlreadyExistsAsync(Guid userId, string username)
	{
		ArgumentNullException.ThrowIfNull(username);
		return await _context.Users.AnyAsync(u => u.Id != userId && u.Username.Equals(username)).ConfigureAwait(false);
	}

	/// <inheritdoc />  
	public async Task AddUserAsync(User user)
	{
		ArgumentNullException.ThrowIfNull(user);

		await _context.Users.AddAsync(user.ToDataEntity()).ConfigureAwait(false);
	}

	/// <inheritdoc />  
	public async Task UpdateUserAsync(User user)
	{
		var userEntity = await _context.Users.FindAsync(user.Id).ConfigureAwait(false);
		userEntity!.UpdateFromDomain(user);
	}

	/// <inheritdoc />  
	public async Task<bool> DeleteUserAsync(Guid userId)
	{
		if (!await _context.Users.AnyAsync(u => u.Id == userId).ConfigureAwait(false))
		{
			return false;
		}
		if (IsInMemoryDb())
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId).ConfigureAwait(false);
			_context.Users.Remove(user!);
		}
		else
		{
			await _context.Users.Where(u => u.Id == userId).ExecuteDeleteAsync().ConfigureAwait(false);
		}

		return true;
	}

	/// <inheritdoc />  
	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync().ConfigureAwait(false);
	}
}
