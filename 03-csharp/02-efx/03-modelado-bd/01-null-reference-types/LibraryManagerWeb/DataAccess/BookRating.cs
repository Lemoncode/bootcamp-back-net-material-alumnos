﻿namespace LibraryManagerWeb.DataAccess
{
	public class BookRating
	{
		public int BookRatingId { get; set; }

		public int BookId { get; set; }

		public required Book Book { get; set; }


		public int Starts { get; set; }
	}
}
