using Repetify.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetify.Domain.Abstractions.Services;

public interface ICardReviewService
{
	/// <summary>
	/// Updates the review status of the card based on whether the review was correct.
	/// </summary>
	/// <param name="isCorrect">Indicates whether the review was correct.</param>
	public void UpdateReview(Card card, bool isCorrect);
}
