using Repetify.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetify.Domain.Abstractions.Services;

/// <summary>
/// Interface for validating complex rules for decks.
/// These rules cannot be validated in the entity itself because they require repository dependencies.
/// </summary>
public interface IDeckValidator
{
	///<summary>
	/// Ensures that the provided deck is valid according to complex rules.
	/// </summary>
	/// <param name="deck">The deck to validate.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task EnsureIsValid(Deck deck);
}
