using Repetify.Application.Abstractions.Services;
using Repetify.Application.Common;
using Repetify.Application.Dtos;
using Repetify.Application.Extensions.Mappings;
using Repetify.Domain.Abstractions.Repositories;
using Repetify.Domain.Abstractions.Services;
using Repetify.Domain.Entities;
using Repetify.Domain.Exceptions;
using Repetify.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetify.Application.Services;

public class UserAppService : IUserAppService
{
	private readonly IUserRepository _userRepository;

	private readonly IUserValidator _userValidator;

	public UserAppService(IUserRepository repository, IUserValidator validator)
	{
		_userRepository = repository;
		_userValidator = validator;
	}

	public async Task<Result<Guid>> AddUserAsync(AddOrEditUserDto user)
	{
		try
		{
			var userDomain = user.ToEntity();
			await _userValidator.EnsureIsValid(userDomain).ConfigureAwait(false);
			await _userRepository.AddUserAsync(userDomain).ConfigureAwait(false);
			await _userRepository.SaveChangesAsync().ConfigureAwait(false);
			return ResultFactory.Success(userDomain.Id);
		}
		catch (EntityExistsException ex)
		{
			return ResultFactory.Conflict<Guid>(ex.Message);
		}
	}

	public async Task<Result<bool>> DeleteUserAsync(Guid userId)
	{
		var deleted = await _userRepository.DeleteUserAsync(userId).ConfigureAwait(false);
		if (deleted)
		{
			await _userRepository.SaveChangesAsync().ConfigureAwait(false);
			return ResultFactory.Success(true);
		}

		return ResultFactory.NotFound<bool>("Unable to find the user to delete.");
	}

	public async Task<Result> UpdateUserAsync(AddOrEditUserDto user, Guid userId)
	{
		try
		{
			var userDomain = user.ToEntity(userId);
			await _userValidator.EnsureIsValid(userDomain).ConfigureAwait(false);
			await _userRepository.UpdateUserAsync(userDomain).ConfigureAwait(false);
			await _userRepository.SaveChangesAsync().ConfigureAwait(false);
			return ResultFactory.Success();
		}
		catch (EntityExistsException ex)
		{
			return ResultFactory.Conflict(ex.Message);
		}
	}

	public async Task<Result<UserDto?>> GetUserByEmailAsync(string email)
	{
		var user = await _userRepository.GetUserByEmailAsync(email).ConfigureAwait(false);
		if (user is null)
		{
			return ResultFactory.NotFound<UserDto?>("Deck not found.");
		}

		return ResultFactory.Success<UserDto?>(user.ToDto());
	}
}
