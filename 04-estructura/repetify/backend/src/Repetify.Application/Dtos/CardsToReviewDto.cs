using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetify.Application.Dtos;

public class CardsToReviewDto
{
	public int? Count { get; set; }

	public required IEnumerable<CardDto> Cards { get; set; }

}
