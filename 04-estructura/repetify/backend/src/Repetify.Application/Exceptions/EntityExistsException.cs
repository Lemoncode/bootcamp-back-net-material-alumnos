namespace Repetify.Application.Exceptions
{
	[Serializable]
	internal class EntityExistsException : Exception
	{
		private string v;
		private string name;

		public EntityExistsException()
		{
		}

		public EntityExistsException(string? message) : base(message)
		{
		}

		public EntityExistsException(string v, string name)
		{
			this.v = v;
			this.name = name;
		}

		public EntityExistsException(string? message, Exception? innerException) : base(message, innerException)
		{
		}
	}
}