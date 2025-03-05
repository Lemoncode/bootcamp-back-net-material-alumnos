namespace Repetify.Domain.Exceptions;

[Serializable]
public class EntityExistsException : Exception
{

	private string _entity;

	private string _field;

	private string _value;

	public EntityExistsException(string entity, string field, string value) : base($"There is already a {entity} entity with the {field} {value}.")
	{
		_entity = entity;
		_field = field;
		_value = value;
	}

	public EntityExistsException()
	{
	}

	public EntityExistsException(string message) : base(message)
	{
	}

	public EntityExistsException(string message, Exception innerException) : base(message, innerException)
	{
	}
}